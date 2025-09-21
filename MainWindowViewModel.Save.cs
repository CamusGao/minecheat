using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using fNbt;
using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace Minecheat;

public partial class MainWindowViewModel
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SaveImageVisibility))]
    [NotifyPropertyChangedFor(nameof(LevelName))]
    [NotifyPropertyChangedFor(nameof(LevelIcon))]
    [NotifyPropertyChangedFor(nameof(SavePath))]
    [NotifyPropertyChangedFor(nameof(FeaturesEnabled))]
    public partial SaveInfo? SelectedSave { get; private set; } = null;

    public Visibility SaveImageVisibility => this.SelectedSave is null ? Visibility.Collapsed : Visibility.Visible;

    public string LevelName => this.SelectedSave?.LevelName ?? "<未打开存档文件夹>";

    public string? LevelIcon => this.SelectedSave is null ? null : Path.Combine(this.SelectedSave.Path, "icon.png");

    public string SavePath => this.SelectedSave?.Path ?? "请先点击右侧按钮打开存档。";

    public bool FeaturesEnabled => this.SelectedSave is not null;

    [RelayCommand]
    public void SelectSave()
    {
        try
        {
            var officalSavesPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                ".minecraft",
                "saves");
            var defaultDirectory = Directory.Exists(officalSavesPath)
                ? officalSavesPath
                : Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            var openFileDialog = new OpenFileDialog
            {
                Title = "选择存档文件夹中的 level.dat 文件",
                Filter = "level.dat|level.dat",
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false,
                RestoreDirectory = true,
                DefaultDirectory = defaultDirectory,
            };

            openFileDialog.ShowDialog();
            var selectedFile = openFileDialog.FileName;
            var path = Path.GetDirectoryName(selectedFile)
                ?? throw new InvalidDataException("无法获取存档文件夹路径。");

            var dataTag = new NbtFile(selectedFile).RootTag.Get<NbtCompound>("Data")!;

            var levelName = dataTag.Get<NbtString>("LevelName")?.Value
                ?? throw new InvalidDataException("无法读取存档名称。");

            var version = dataTag.Get<NbtCompound>("Version")?.Get<NbtString>("Name")?.Value
                ?? throw new InvalidDataException("无法读取存档版本信息。");
            var saveInfo = new SaveInfo
            {
                LevelName = levelName,
                Path = path,
                Version = version,
            };
            this.AppendLog($"选中存档：{saveInfo}。");
            this.SelectedSave = saveInfo;
        }
        catch (Exception e)
        {
            MessageBox.Show($"打开存档失败：{e.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
