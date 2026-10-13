using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace Wpf.Ui.Common;

#if NET5_0_OR_GREATER
public delegate void RoutedSnackbarEvent([NotNull] Controls.Snackbar sender, RoutedEventArgs e);
#else
public delegate void RoutedSnackbarEvent(Controls.Snackbar sender, RoutedEventArgs e);
#endif
