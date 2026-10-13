using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using Wpf.Ui.Common;
using Wpf.Ui.Interop;

namespace Wpf.Ui.Controls;

[ToolboxItem(true)]
[ToolboxBitmap(typeof(MessageBox), "MessageBox.bmp")]
public class MessageBox : System.Windows.Window
{
    public static readonly DependencyProperty FooterProperty = DependencyProperty.Register(nameof(Footer),
        typeof(object), typeof(MessageBox), new PropertyMetadata(null));

    public static readonly DependencyProperty ShowFooterProperty = DependencyProperty.Register(nameof(ShowFooter),
        typeof(bool), typeof(MessageBox), new PropertyMetadata(true));

    public static readonly DependencyProperty ShowTitleProperty = DependencyProperty.Register(nameof(ShowTitle),
        typeof(bool), typeof(MessageBox), new PropertyMetadata(true));

    public static readonly DependencyProperty MicaEnabledProperty = DependencyProperty.Register(nameof(MicaEnabled),
        typeof(bool), typeof(MessageBox), new PropertyMetadata(true));

    public static readonly DependencyProperty ButtonLeftNameProperty = DependencyProperty.Register(nameof(ButtonLeftName),
        typeof(string), typeof(MessageBox), new PropertyMetadata("Action"));

    public static readonly DependencyProperty ButtonLeftAppearanceProperty = DependencyProperty.Register(nameof(ButtonLeftAppearance),
        typeof(Common.ControlAppearance), typeof(MessageBox),
        new PropertyMetadata(Common.ControlAppearance.Primary));

    public static readonly RoutedEvent ButtonLeftClickEvent = EventManager.RegisterRoutedEvent(
        nameof(ButtonLeftClick), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MessageBox));

    public static readonly DependencyProperty ButtonRightNameProperty = DependencyProperty.Register(nameof(ButtonRightName),
        typeof(string), typeof(MessageBox), new PropertyMetadata("Close"));

    public static readonly DependencyProperty ButtonRightAppearanceProperty = DependencyProperty.Register(nameof(ButtonRightAppearance),
        typeof(Common.ControlAppearance), typeof(MessageBox),
        new PropertyMetadata(Common.ControlAppearance.Secondary));

    public static readonly RoutedEvent ButtonRightClickEvent = EventManager.RegisterRoutedEvent(
        nameof(ButtonRightClick), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MessageBox));

    public static readonly DependencyProperty TemplateButtonCommandProperty =
        DependencyProperty.Register(nameof(TemplateButtonCommand),
            typeof(Common.IRelayCommand), typeof(MessageBox), new PropertyMetadata(null));

    public object Footer
    {
        get => GetValue(FooterProperty);
        set => SetValue(FooterProperty, value);
    }

    public bool ShowFooter
    {
        get => (bool)GetValue(ShowFooterProperty);
        set => SetValue(ShowFooterProperty, value);
    }

    public bool ShowTitle
    {
        get => (bool)GetValue(ShowTitleProperty);
        set => SetValue(ShowTitleProperty, value);
    }

    public bool MicaEnabled
    {
        get => (bool)GetValue(MicaEnabledProperty);
        set => SetValue(MicaEnabledProperty, value);
    }

    public string ButtonLeftName
    {
        get => (string)GetValue(ButtonLeftNameProperty);
        set => SetValue(ButtonLeftNameProperty, value);
    }

    public Common.ControlAppearance ButtonLeftAppearance
    {
        get => (Common.ControlAppearance)GetValue(ButtonLeftAppearanceProperty);
        set => SetValue(ButtonLeftAppearanceProperty, value);
    }

    public event RoutedEventHandler ButtonLeftClick
    {
        add => AddHandler(ButtonLeftClickEvent, value);
        remove => RemoveHandler(ButtonLeftClickEvent, value);
    }

    public string ButtonRightName
    {
        get => (string)GetValue(ButtonRightNameProperty);
        set => SetValue(ButtonRightNameProperty, value);
    }

    public Common.ControlAppearance ButtonRightAppearance
    {
        get => (Common.ControlAppearance)GetValue(ButtonRightAppearanceProperty);
        set => SetValue(ButtonRightAppearanceProperty, value);
    }

    public event RoutedEventHandler ButtonRightClick
    {
        add => AddHandler(ButtonRightClickEvent, value);
        remove => RemoveHandler(ButtonRightClickEvent, value);
    }

    public Common.IRelayCommand TemplateButtonCommand => (Common.IRelayCommand)GetValue(TemplateButtonCommandProperty);

    public MessageBox()
    {
        SetWindowStartupLocation();
        Topmost = true;

        Height = 200;
        Width = 400;

        WindowStartupLocation = WindowStartupLocation.CenterOwner;

        SetValue(TemplateButtonCommandProperty, new Common.RelayCommand(o => Button_OnClick(this, o)));
    }

    public new void Show()
    {
        UnsafeNativeMethods.RemoveWindowTitlebar(this);

        Wpf.Ui.Appearance.Background.Apply(this, Wpf.Ui.Appearance.BackgroundType.Mica);

        base.Show();
    }

    public void Show(string title, object content)
    {
        Title = title;
        Content = content;

        Show();
    }

    private void SetWindowStartupLocation()
    {
        if (Application.Current?.MainWindow != null)
            Owner = Application.Current.MainWindow;
        else
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
    }

    private void Button_OnClick(object sender, object parameter)
    {
        if (parameter == null)
            return;

        string param = parameter as string ?? String.Empty;

#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO | {typeof(MessageBox)} button clicked with param: {param}", "Wpf.Ui.MessageBox");
#endif

        switch (param)
        {
            case "left":
                RaiseEvent(new RoutedEventArgs(ButtonLeftClickEvent, this));

                break;

            case "right":
                RaiseEvent(new RoutedEventArgs(ButtonRightClickEvent, this));

                break;
        }
    }
}
