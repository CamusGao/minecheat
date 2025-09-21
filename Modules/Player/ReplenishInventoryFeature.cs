using fNbt;
using Minecheat;
using System.IO;

namespace Minecheat.Modules.Player;

public class ReplenishInventoryFeature : PlayerBasedFeature
{
    public override string FeatureTitle => "补货";

    public override string Tooltip => "从异次元以极低的价格补充背包中的物品";

    protected override void EditPlayerTag(SaveInfo saveInfo, NbtCompound playerTag, Action<string> appendLog)
    {
        appendLog("正在补货");

        var minecraftData = ((App)App.Current).MinecraftData;
        var versionedItems = minecraftData?.VersionedItems[saveInfo.Version] 
            ?? throw new InvalidDataException($"无法获取版本 {saveInfo.Version} 的物品数据。");

        var offhandItemTag = playerTag.Get<NbtCompound>("equipment")
            ?.Get<NbtCompound>("offhand");
        foreach (var itemTag in playerTag
            .Get<NbtList>("Inventory")
            ?.Append(offhandItemTag)
            .Where(x => x != null)
            .Cast<NbtCompound>()
            ?? [])
        {
            var itemId = itemTag.Get<NbtString>("id")!.Value;
            var nonnamespacedItemId = itemId.Contains(':') ? itemId.Split(':')[1] : itemId;
            var item = versionedItems.FirstOrDefault(x => x.Name == nonnamespacedItemId);
            if (item == null)
            {
                continue;
            }

            var countTag = itemTag.Get<NbtInt>("count")!;
            if (item.StackSize > countTag.Value)
            {
                appendLog($"补充物品 {item.DisplayName}，当前数量 {countTag.Value}，补充至 {item.StackSize}");
                countTag.Value = item.StackSize;
            }
        }

        appendLog("补货完成");
    }
}
