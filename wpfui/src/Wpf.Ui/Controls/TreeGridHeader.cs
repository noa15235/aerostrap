using System;
using System.ComponentModel;
using System.Windows;

namespace Wpf.Ui.Controls;

public class TreeGridHeader : System.Windows.FrameworkElement
{
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title),
        typeof(string), typeof(TreeGridHeader), new PropertyMetadata(String.Empty, OnTitleChanged));

    public static readonly DependencyProperty GroupProperty = DependencyProperty.Register(nameof(Group),
        typeof(string), typeof(TreeGridHeader), new PropertyMetadata(String.Empty));

    public string Title
    {
        get => (string)GetValue(NameProperty);
        set => SetValue(NameProperty, value);
    }

    [Localizability(LocalizationCategory.NeverLocalize)]
    [MergableProperty(false)]
    public string Group
    {
        get => (string)GetValue(GroupProperty);
        set => SetValue(GroupProperty, value);
    }

    protected virtual void OnTitleChanged()
    {
        var title = Title;

        if (!String.IsNullOrEmpty(Group) || String.IsNullOrEmpty(title))
            return;

        Group = title.ToLower().Trim();
    }

    private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not TreeGridHeader header)
            return;

        header.OnTitleChanged();
    }
}
