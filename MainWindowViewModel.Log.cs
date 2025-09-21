using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Minecheat;

public partial class MainWindowViewModel
{
    [ObservableProperty]
    public partial ObservableCollection<Log> Logs { get; private set; } = [];

    private void AppendLog(string log)
    {
        this.Logs.Add(new (log));
    }
}
