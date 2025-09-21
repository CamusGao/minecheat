using System.Windows;

namespace Minecheat;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly MainWindowViewModel ViewModel = new();

    public MainWindow()
    {
        this.InitializeComponent();
        this.DataContext = this.ViewModel;
    }
}
