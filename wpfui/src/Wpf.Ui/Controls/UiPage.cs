using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;

namespace Wpf.Ui.Controls;

[TemplatePart(Name = "PART_ScrollViewer", Type = typeof(Wpf.Ui.Controls.DynamicScrollViewer))]
[ToolboxItem(true)]
[ToolboxBitmap(typeof(UiPage), "UiPage.bmp")]
public class UiPage : System.Windows.Controls.Page
{
    private const string ElementScrollViewer = "PART_ScrollViewer";

    public static readonly DependencyProperty ScrollableProperty = DependencyProperty.Register(nameof(Scrollable),
        typeof(bool), typeof(UiPage), new PropertyMetadata(false));

    public static readonly DependencyProperty ScrollHostProperty = DependencyProperty.Register(nameof(ScrollHost),
        typeof(ScrollViewer), typeof(UiPage), new PropertyMetadata((ScrollViewer)null));

    [Bindable(true), Category("Appearance")]
    public bool Scrollable
    {
        get => (bool)GetValue(ScrollableProperty);
        set => SetValue(ScrollableProperty, value);
    }

    [Bindable(false)]
    public ScrollViewer ScrollHost
    {
        get
        {
            if (!Scrollable)
                throw new InvalidOperationException($"{nameof(ScrollHost)} can only be taken from a page that has {nameof(Scrollable)} set to true.");

            return (ScrollViewer)GetValue(ScrollHostProperty);
        }
        private set => SetValue(ScrollHostProperty, value);
    }

    public UiPage()
    {
        DataContext = this;

        SetResourceReference(StyleProperty, typeof(UiPage));
    }

    static UiPage()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(UiPage), new FrameworkPropertyMetadata(typeof(UiPage)));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (!Scrollable)
            return;

        var scrollHost = GetTemplateChild(ElementScrollViewer);

        if (scrollHost is ScrollViewer)
            ScrollHost = scrollHost as ScrollViewer;
    }
}
