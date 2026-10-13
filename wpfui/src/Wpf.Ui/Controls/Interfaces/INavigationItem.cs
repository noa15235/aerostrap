using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Wpf.Ui.Controls.Interfaces;

public interface INavigationItem
{
    string PageTag { get; set; }

    object Content { get; }

    bool IsActive { get; set; }

    bool Cache { get; set; }

    Uri PageSource { get; set; }

    Type PageType { get; set; }

    Uri AbsolutePageSource { get; }

    [Category("Behavior")]
    event RoutedEventHandler Click;
}
