using System.Diagnostics.CodeAnalysis;
using System.Windows;
using Wpf.Ui.Controls;

namespace Wpf.Ui.Common;

#if NET5_0_OR_GREATER
public delegate void RoutedNotifyIconEvent([NotNull] NotifyIcon sender, RoutedEventArgs e);
#else
public delegate void RoutedNotifyIconEvent(NotifyIcon sender, RoutedEventArgs e);
#endif
