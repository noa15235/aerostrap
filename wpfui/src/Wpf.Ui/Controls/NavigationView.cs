using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common;
using Wpf.Ui.Controls.Interfaces;

namespace Wpf.Ui.Controls;

[TemplatePart(Name = "PART_Navigation", Type = typeof(Wpf.Ui.Controls.Navigation.NavigationBase))]
[TemplatePart(Name = "PART_Breadcrumb", Type = typeof(Wpf.Ui.Controls.Breadcrumb))]
[TemplatePart(Name = "PART_Frame", Type = typeof(System.Windows.Controls.Frame))]
public class NavigationView : System.Windows.Controls.Control
{
    private const string ElementNavigation = "PART_Navigation";

    private const string ElementBreadcrumb = "PART_Breadcrumb";

    private const string ElementFrame = "PART_Frame";

    public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(nameof(Type),
        typeof(NavigationType), typeof(NavigationView),
        new PropertyMetadata(NavigationType.Compact));

    public INavigation Navigation { get; protected set; }

    public Breadcrumb Breadcrumb { get; protected set; }

    public Frame Frame { get; protected set; }

    public NavigationType Type
    {
        get => (NavigationType)GetValue(TypeProperty);
        set => SetValue(TypeProperty, value);
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        Navigation = GetTemplateChild(ElementNavigation) as INavigation;
        Breadcrumb = GetTemplateChild(ElementBreadcrumb) as Breadcrumb;
        Frame = GetTemplateChild(ElementFrame) as Frame;
    }
}
