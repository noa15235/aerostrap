using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Wpf.Ui.Controls;

public class DataGrid : System.Windows.Controls.DataGrid
{
    public static readonly DependencyProperty CheckBoxColumnElementStyleProperty = DependencyProperty.Register(nameof(CheckBoxColumnElementStyle),
        typeof(Style), typeof(DataGrid), new FrameworkPropertyMetadata(null));

    public static readonly DependencyProperty CheckBoxColumnEditingElementStyleProperty = DependencyProperty.Register(nameof(CheckBoxColumnEditingElementStyle),
        typeof(Style), typeof(DataGrid), new FrameworkPropertyMetadata(null));

    public Style CheckBoxColumnElementStyle
    {
        get => (Style)GetValue(CheckBoxColumnElementStyleProperty);
        set => SetValue(CheckBoxColumnElementStyleProperty, value);
    }

    public Style CheckBoxColumnEditingElementStyle
    {
        get => (Style)GetValue(CheckBoxColumnEditingElementStyleProperty);
        set => SetValue(CheckBoxColumnEditingElementStyleProperty, value);
    }

    protected override void OnInitialized(EventArgs e)
    {
        Columns.CollectionChanged += ColumnsOnCollectionChanged;

        UpdateColumnElementStyles();

        base.OnInitialized(e);
    }

    private void ColumnsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        UpdateColumnElementStyles();
    }

    private void UpdateColumnElementStyles()
    {
        foreach (var singleColumn in Columns)
            UpdateSingleColumn(singleColumn);
    }

    private void UpdateSingleColumn(DataGridColumn dataGridColumn)
    {
        if (dataGridColumn is DataGridCheckBoxColumn checkBoxColumn)
        {
            if (checkBoxColumn.ReadLocalValue(DataGridCheckBoxColumn.ElementStyleProperty) == DependencyProperty.UnsetValue)
                BindingOperations.SetBinding(
                    checkBoxColumn,
                    DataGridCheckBoxColumn.ElementStyleProperty,
                    new Binding { Path = new PropertyPath(CheckBoxColumnElementStyleProperty), Source = this });

            if (checkBoxColumn.ReadLocalValue(DataGridCheckBoxColumn.EditingElementStyleProperty) == DependencyProperty.UnsetValue)
                BindingOperations.SetBinding(
                    checkBoxColumn,
                    DataGridCheckBoxColumn.EditingElementStyleProperty,
                    new Binding { Path = new PropertyPath(CheckBoxColumnEditingElementStyleProperty), Source = this });
        }
    }
}
