using System.Windows;

namespace Minecheat;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public MinecraftData? MinecraftData { get; private set; }

    public async Task LoadMinecraftDataAsync()
    {
        this.MinecraftData = await MinecraftData.LoadMinecraftDataAsync();
    }
}
