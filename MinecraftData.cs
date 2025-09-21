using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Formats.Tar;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Minecheat;

public class MinecraftData
{
    public IReadOnlyDictionary<string, IReadOnlyList<Item>> VersionedItems { get; private set; }

    private MinecraftData(
        IReadOnlyDictionary<string, IReadOnlyList<Item>> versionedItems)
    {
        this.VersionedItems = versionedItems;
    }

    public static async Task<MinecraftData> LoadMinecraftDataAsync()
    {
        // version means the version of minecraft-data package
        var versionDir = await EnsureLatestMinecraftDataAsync();
        var dataPath = Path.Combine(versionDir, "package", "minecraft-data", "data");
        var dataPathsFilePath = Path.Combine(dataPath, "dataPaths.json");
    
        using var dataPathsFileStream = File.OpenRead(dataPathsFilePath);
        var dataPaths = await JsonSerializer.DeserializeAsync<JsonObject>(dataPathsFileStream, JsonSerializerOptions.Web)
            ?? throw new InvalidDataException("无法解析 dataPaths.json");

        var versionedItems = new ConcurrentDictionary<string, IReadOnlyList<Item>>();

        await Parallel.ForEachAsync(dataPaths["pc"]!.AsObject(), async (p, _) =>
        {
            var (gameVersion, versionedDataPath) = p;
            var versiondItemsPath = versionedDataPath!.AsObject()["items"]?.GetValue<string>();
            if (versiondItemsPath == null)
            {
                return;
            }
            var versiondItemsFilePath = Path.Combine(dataPath, versiondItemsPath, "items.json");
            var versiondItems = await LoadVersiondItemsAsync(versiondItemsFilePath);
            versionedItems[gameVersion] = versiondItems;
        });

        return new MinecraftData(new Dictionary<string, IReadOnlyList<Item>>(versionedItems).AsReadOnly());
    }

    private static async Task<IReadOnlyList<Item>> LoadVersiondItemsAsync(string versiondItemsFilePath)
    {
        using var itemsStream = File.OpenRead(versiondItemsFilePath);
        var items = await JsonSerializer.DeserializeAsync<List<Item>>(itemsStream, JsonSerializerOptions.Web)
            ?? throw new InvalidDataException("无法解析 items.json");
        return items.AsReadOnly();
    }

    private static async Task<string> EnsureLatestMinecraftDataAsync()
    {
        using var httpClient = new HttpClient();

        var npmRegistry = ConfigurationManager.AppSettings["NpmRegistry"]
            ?? "https://registry.npmjs.org/";
        httpClient.BaseAddress = new Uri(npmRegistry);

        var packageInfo = await httpClient.GetFromJsonAsync<JsonObject>("/minecraft-data/latest");
        var latestVersion = packageInfo!["version"]!.GetValue<string>();

        if (EnsureMinecraftDataDir(latestVersion, out var versionDir))
        {
            return versionDir;
        }

        var tarbarFilePath = Path.Combine(versionDir, "tarball.tgz");
        using var tarbarFileStream = File.Open(tarbarFilePath, new FileStreamOptions
        {
            Mode = FileMode.Create,
            Access = FileAccess.ReadWrite,
            Options = FileOptions.Asynchronous,
            BufferSize = 16 * 1024,
            Share = FileShare.None,
        });

        var tarballUrl = packageInfo["dist"]!["tarball"]!.GetValue<string>();
        using (var tarballDownloadStream = await httpClient.GetStreamAsync(tarballUrl))
        {
            await tarballDownloadStream.CopyToAsync(tarbarFileStream);
        }

        tarbarFileStream.Seek(0, SeekOrigin.Begin);
        using var gzipStream = new GZipStream(tarbarFileStream, CompressionMode.Decompress, leaveOpen: true);
        await TarFile.ExtractToDirectoryAsync(gzipStream, versionDir, true);

        return versionDir;
    }

    private static bool EnsureMinecraftDataDir(string version, out string versionDir)
    {
        var minecraftDataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "minecraft-data");
        if (!Directory.Exists(minecraftDataDir))
        {
            Directory.CreateDirectory(minecraftDataDir);
        }

        versionDir = Path.Combine(minecraftDataDir, version);
        if (!Directory.Exists(versionDir))
        {
            Directory.CreateDirectory(versionDir);
            return false;
        }

        return true;
    }

    public class Item
    {
        public int Id { get; set; }

        public string DisplayName { get; set; }

        public int StackSize { get; set; }

        public string Name { get; set; }

        public string[]? EnchantCategories { get; set; }

        public string[]? RepairWith { get; set; }

        public int? MaxDurability { get; set; }

        public int? Durability { get; set; }

        public int? Metadata { get; set; }

        public int? BlockStateId { get; set; }

        public ItemVariation[]? Variations { get; set; }

        public Item(
            int id,
            string displayName,
            int stackSize,
            string name,
            string[]? enchantCategories,
            string[]? repairWith,
            int? maxDurability,
            int? durability,
            int? metadata,
            int? blockStateId,
            ItemVariation[]? variations)
        {
            Id = id;
            DisplayName = displayName;
            StackSize = stackSize;
            Name = name;
            EnchantCategories = enchantCategories;
            RepairWith = repairWith;
            MaxDurability = maxDurability;
            Durability = durability;
            Metadata = metadata;
            BlockStateId = blockStateId;
            Variations = variations;
        }
    }

    public class ItemVariation
    {
        public int Metadata { get; set; }

        public string DisplayName { get; set; } = string.Empty;

        public int? Id { get; set; }

        public string? Name { get; set; }

        public int? StackSize { get; set; }

        public string[]? EnchantCategories { get; set; }

        public ItemVariation(
            int metadata,
            string displayName,
            int? id,
            string? name,
            int? stackSize,
            string[]? enchantCategories)
        {
            Metadata = metadata;
            DisplayName = displayName;
            Id = id;
            Name = name;
            StackSize = stackSize;
            EnchantCategories = enchantCategories;
        }
    }
}
