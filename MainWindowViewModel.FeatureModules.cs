using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Minecheat;

public partial class MainWindowViewModel
{
    public IReadOnlyList<IFeatureModule> FeatureModules
        => global::Minecheat.Modules.FeatureModules.Modules;

    [ObservableProperty]
    public partial int SelectedFeatureModuleIndex { get; set; } = 0;

    [RelayCommand]
    private void InvokeFeature(IFeature feature)
    {
        if (this.SelectedSave != null)
        {
            try
            {
                this.AppendLog($"正在进行：{feature.FeatureTitle}。");
                feature.Invoke(this.SelectedSave, this.AppendLog);
            }
            catch (Exception e)
            {
                this.AppendLog($"发生异常：{e}");
            }
            finally
            {
                this.AppendLog($"结束：{feature.FeatureTitle}。");
            }
        }
    }
}
