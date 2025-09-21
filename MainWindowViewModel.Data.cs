using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace Minecheat;

public partial class MainWindowViewModel
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DataLoadingMaskVisibility))]
    public partial bool DataLoading { get; private set; } = false;

    public Visibility DataLoadingMaskVisibility => this.DataLoading ? Visibility.Visible : Visibility.Collapsed;

    [RelayCommand]
    public async Task OnWindowLoadedAsync()
    {
        this.DataLoading = true;
        try
        {
            var app = (App) App.Current;
            await app.LoadMinecraftDataAsync();
        }
        catch (Exception e)
        {
            this.AppendLog($"加载 minecraft-data 发生异常：{e}");
        }
        finally
        {
            this.DataLoading = false;
        }
    }
}
