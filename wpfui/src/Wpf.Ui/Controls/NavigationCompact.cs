using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;

namespace Wpf.Ui.Controls;

[ToolboxItem(true)]
[ToolboxBitmap(typeof(NavigationCompact), "NavigationCompact.bmp")]
public class NavigationCompact : Wpf.Ui.Controls.Navigation.NavigationBase
{
    public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(
        nameof(IsExpanded),
        typeof(bool), typeof(NavigationCompact),
        new PropertyMetadata(false));

    public static readonly DependencyProperty TemplateButtonCommandProperty =
        DependencyProperty.Register(nameof(TemplateButtonCommand),
            typeof(Common.IRelayCommand), typeof(NavigationCompact), new PropertyMetadata(null));

    public bool IsExpanded
    {
        get => (bool)GetValue(IsExpandedProperty);
        set => SetValue(IsExpandedProperty, value);
    }

    public Common.IRelayCommand TemplateButtonCommand => (Common.IRelayCommand)GetValue(TemplateButtonCommandProperty);

    public NavigationCompact() : base() =>
        SetValue(TemplateButtonCommandProperty, new Common.RelayCommand(o => Button_OnClick(this, o)));

    private void Button_OnClick(object sender, object parameter)
    {
        if (parameter == null)
            return;

        string param = parameter as string ?? String.Empty;

#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO: {typeof(NavigationCompact)} button clicked with param: {param}", "Wpf.Ui.NavigationCompact");
#endif
        if (param == "hamburger")
            IsExpanded = !IsExpanded;
    }
}
