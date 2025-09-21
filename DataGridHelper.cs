using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace Minecheat;

public static class DataGridHelper
{
    public static readonly DependencyProperty AutoScrollToEndProperty
        = DependencyProperty.RegisterAttached(
            "AutoScrollToEnd",
            typeof(bool),
            typeof(DataGridHelper),
            new PropertyMetadata(false, OnAutoScrollToEndChanged));

    public static bool GetAutoScrollToEnd(DependencyObject obj)
    {
        return (bool)obj.GetValue(AutoScrollToEndProperty);
    }

    public static void SetAutoScrollToEnd(DependencyObject obj, bool value)
    {
        obj.SetValue(AutoScrollToEndProperty, value);
    }

    private static void OnAutoScrollToEndChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is DataGrid dataGrid)
        {
            if ((bool)e.NewValue)
            {
                // 启用自动滚动到末尾
                dataGrid.Loaded += DataGrid_Loaded;
                dataGrid.Unloaded += DataGrid_Unloaded;
            }
            else
            {
                // 禁用自动滚动到末尾
                dataGrid.Loaded -= DataGrid_Loaded;
                dataGrid.Unloaded -= DataGrid_Unloaded;

                // 如果DataGrid已经加载，移除事件处理程序
                if (dataGrid.ItemsSource is INotifyCollectionChanged collection)
                {
                    collection.CollectionChanged -= (sender, args) => OnCollectionChanged(dataGrid, args);
                }
            }
        }
    }

    private static void DataGrid_Loaded(object sender, RoutedEventArgs e)
    {
        if (sender is DataGrid dataGrid)
        {
            // 监听ItemsSource的集合变化
            if (dataGrid.ItemsSource is INotifyCollectionChanged collection)
            {
                collection.CollectionChanged += (s, args) => OnCollectionChanged(dataGrid, args);
            }

            // 初始滚动到末尾
            ScrollToEnd(dataGrid);
        }
    }

    private static void DataGrid_Unloaded(object sender, RoutedEventArgs e)
    {
        if (sender is DataGrid dataGrid)
        {
            // 移除事件处理程序以防止内存泄漏
            if (dataGrid.ItemsSource is INotifyCollectionChanged collection)
            {
                collection.CollectionChanged -= (s, args) => OnCollectionChanged(dataGrid, args);
            }
        }
    }

    private static void OnCollectionChanged(DataGrid dataGrid, NotifyCollectionChangedEventArgs e)
    {
        // 当集合发生变化时，滚动到末尾
        if (e.Action == NotifyCollectionChangedAction.Add ||
            e.Action == NotifyCollectionChangedAction.Replace ||
            e.Action == NotifyCollectionChangedAction.Reset)
        {
            // 使用Dispatcher确保在UI线程上执行，并延迟执行以确保DataGrid已更新
            dataGrid.Dispatcher.BeginInvoke(() => ScrollToEnd(dataGrid));
        }
    }

    private static void ScrollToEnd(DataGrid dataGrid)
    {
        if (dataGrid.Items.Count > 0)
        {
            dataGrid.ScrollIntoView(dataGrid.Items[^1]);
        }
    }
}
