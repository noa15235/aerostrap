using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Wpf.Ui.Common;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Controls.Navigation;
using Wpf.Ui.Extensions;
using Brush = System.Windows.Media.Brush;
using SystemColors = System.Windows.SystemColors;

namespace Wpf.Ui.Controls;

[ToolboxItem(true)]
[ToolboxBitmap(typeof(NavigationItem), "NavigationItem.bmp")]
public class NavigationItem : System.Windows.Controls.Primitives.ButtonBase, IUriContext, INavigationItem, INavigationControl, IIconControl
{
    public static readonly DependencyProperty PageTagProperty = DependencyProperty.Register(nameof(PageTag),
        typeof(string), typeof(NavigationItem), new PropertyMetadata(String.Empty));

    public static readonly DependencyProperty PageSourceProperty = DependencyProperty.Register(nameof(PageSource),
        typeof(Uri), typeof(NavigationItem), new PropertyMetadata((Uri)null, OnPageSourceChanged));

    public static readonly DependencyProperty PageTypeProperty = DependencyProperty.Register(nameof(PageType),
        typeof(Type), typeof(NavigationItem), new PropertyMetadata((Type)null, OnPageTypeChanged));

    public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(nameof(IsActive),
        typeof(bool), typeof(NavigationItem), new PropertyMetadata(false));

    public static readonly DependencyProperty CacheProperty = DependencyProperty.Register(nameof(Cache),
        typeof(bool), typeof(NavigationItem), new PropertyMetadata(true));

    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon),
        typeof(Common.SymbolRegular), typeof(NavigationItem),
        new PropertyMetadata(Common.SymbolRegular.Empty));

    public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(nameof(IconSize),
        typeof(double), typeof(NavigationItem),
        new PropertyMetadata(18d));

    public static readonly DependencyProperty IconFilledProperty = DependencyProperty.Register(nameof(IconFilled),
        typeof(bool), typeof(NavigationItem), new PropertyMetadata(false));

    public static readonly DependencyProperty IconForegroundProperty = DependencyProperty.Register(nameof(IconForeground),
        typeof(Brush), typeof(NavigationItem), new FrameworkPropertyMetadata(SystemColors.ControlTextBrush,
            FrameworkPropertyMetadataOptions.Inherits));

    public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(nameof(Image),
        typeof(BitmapSource), typeof(NavigationItem),
        new PropertyMetadata(null));

    public static readonly RoutedEvent ActivatedEvent = EventManager.RegisterRoutedEvent(
        nameof(Activated), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NavigationItem));

    public static readonly RoutedEvent DeactivatedEvent = EventManager.RegisterRoutedEvent(
        nameof(Deactivated), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NavigationItem));

    public string PageTag
    {
        get => (string)GetValue(PageTagProperty);
        set => SetValue(PageTagProperty, value);
    }

    public Uri PageSource
    {
        get => (Uri)GetValue(PageSourceProperty);
        set => SetValue(PageSourceProperty, value);

    }

    public Type PageType
    {
        get => (Type)GetValue(PageTypeProperty);
        set => SetValue(PageTypeProperty, value);
    }

    public bool IsActive
    {
        get => (bool)GetValue(IsActiveProperty);
        set
        {
            if (value == IsActive)
                return;

            RaiseEvent(value
                ? new RoutedEventArgs(ActivatedEvent, this)
                : new RoutedEventArgs(DeactivatedEvent, this));

            SetValue(IsActiveProperty, value);
        }
    }

    public bool Cache
    {
        get => (bool)GetValue(CacheProperty);
        set => SetValue(CacheProperty, value);
    }

    [Bindable(true), Category("Appearance")]
    [Localizability(LocalizationCategory.None)]
    public Common.SymbolRegular Icon
    {
        get => (Common.SymbolRegular)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    [Bindable(true), Category("Appearance")]
    [Localizability(LocalizationCategory.None)]
    public bool IconFilled
    {
        get => (bool)GetValue(IconFilledProperty);
        set => SetValue(IconFilledProperty, value);
    }

    [TypeConverter(typeof(FontSizeConverter))]
    [Bindable(true), Category("Appearance")]
    [Localizability(LocalizationCategory.None)]
    public double IconSize
    {
        get => (double)GetValue(IconSizeProperty);
        set => SetValue(IconSizeProperty, value);
    }

    [Bindable(true), Category("Appearance")]
    public Brush IconForeground
    {
        get => (Brush)GetValue(IconForegroundProperty);
        set => SetValue(IconForegroundProperty, value);
    }

    [Bindable(true), Category("Appearance")]
    public BitmapSource Image
    {
        get => GetValue(ImageProperty) as BitmapSource;
        set => SetValue(ImageProperty, value);
    }

    public event RoutedEventHandler Activated
    {
        add => AddHandler(ActivatedEvent, value);
        remove => RemoveHandler(ActivatedEvent, value);
    }

    public event RoutedEventHandler Deactivated
    {
        add => AddHandler(DeactivatedEvent, value);
        remove => RemoveHandler(DeactivatedEvent, value);
    }

    [Bindable(false)]
    public Uri AbsolutePageSource { get; internal set; }

    Uri IUriContext.BaseUri
    {
        get => BaseUri;
        set => BaseUri = value;
    }

    protected virtual Uri BaseUri
    {
        get => (Uri)GetValue(BaseUriHelper.BaseUriProperty);
        set => SetValue(BaseUriHelper.BaseUriProperty, value);
    }

    protected override void OnContentChanged(object oldContent, object newContent)
    {
        base.OnContentChanged(oldContent, newContent);

        if (newContent is string && String.IsNullOrEmpty(PageTag))
            PageTag = newContent?.ToString()?.ToLower()?.Trim() ?? String.Empty;
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (Keyboard.Modifiers is not ModifierKeys.None)
        {
            return;
        }

        switch (e.Key)
        {
            case Key.Left:
                MoveFocus(this, FocusNavigationDirection.Left);
                e.Handled = true;
                break;

            case Key.Up:
                MoveFocus(this, FocusNavigationDirection.Up);
                e.Handled = true;
                break;

            case Key.Right:
                MoveFocus(this, FocusNavigationDirection.Right);
                e.Handled = true;
                break;

            case Key.Down:
                MoveFocus(this, FocusNavigationDirection.Down);
                e.Handled = true;
                break;

            case Key.Space:
            case Key.Enter:

                if (PageSource == null && PageType == null)
                    break;

                if (NavigationBase.GetNavigationParent(this) is { } navigation
                    && PageTag is { } pageTag
                    && !String.IsNullOrEmpty(pageTag))
                {
                    e.Handled = true;

                    navigation.Navigate(pageTag);
                }
                break;
        }

        if (!e.Handled)
            base.OnKeyDown(e);

        static void MoveFocus(FrameworkElement element, FocusNavigationDirection direction)
        {
            var request = new TraversalRequest(direction);
            element.MoveFocus(request);
        }
    }

    protected virtual void OnPageSourceChanged(Uri pageUri)
    {
        if (!pageUri.OriginalString.EndsWith(".xaml"))
            throw new ArgumentException($"URI in {typeof(NavigationItem)} must point to the XAML Page.");

        AbsolutePageSource = ResolvePageUri(pageUri);
    }

    private static void OnPageSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NavigationItem navigationItem)
            return;

        navigationItem.OnPageSourceChanged(e.NewValue as Uri);
    }

    protected virtual void OnPageTypeChanged(Type pageType)
    {
        if (pageType == null)
            return;

        if (!typeof(System.Windows.FrameworkElement).IsAssignableFrom(pageType))
            throw new ArgumentException($"{pageType} is not inherited from {typeof(System.Windows.FrameworkElement)}.");
    }

    private static void OnPageTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NavigationItem navigationItem)
            return;

        navigationItem.OnPageTypeChanged(e.NewValue as Type);
    }

    private Uri ResolvePageUri(Uri pageUri)
    {
        if (pageUri == null || pageUri.IsAbsoluteUri)
            return pageUri;

        if (!pageUri.EndsWith(".xaml"))
            throw new ArgumentException("PageSource must point to the .xaml file.");

        var baseUri = BaseUri;

        if (baseUri == null)
        {

            if (!DesignerHelper.IsInDesignMode)
                throw new UriFormatException("Unable to resolve absolute URI for selected page");

            PageType = typeof(System.Windows.Controls.Page);

            return null;
        }

        if (!baseUri.IsAbsoluteUri)
            throw new ApplicationException("Unable to resolve base URI for selected page");

        if (baseUri.EndsWith(".xaml"))
            baseUri = baseUri.TrimLastSegment();

        return baseUri.Append(pageUri);
    }
}
