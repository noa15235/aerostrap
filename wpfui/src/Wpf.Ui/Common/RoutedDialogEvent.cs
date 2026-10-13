using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace Wpf.Ui.Common;

#if NET5_0_OR_GREATER
public delegate void RoutedDialogEvent([NotNull] Controls.Dialog sender, RoutedEventArgs e);
#else
public delegate void RoutedDialogEvent(Controls.Dialog sender, RoutedEventArgs e);
#endif
