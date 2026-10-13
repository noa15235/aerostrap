using System.Diagnostics.CodeAnalysis;
using Wpf.Ui.Controls.Interfaces;

namespace Wpf.Ui.Common;

#if NET5_0_OR_GREATER
public delegate void RoutedNavigationEvent([NotNull] INavigation sender, RoutedNavigationEventArgs e);
#else
public delegate void RoutedNavigationEvent(INavigation sender, RoutedNavigationEventArgs e);
#endif
