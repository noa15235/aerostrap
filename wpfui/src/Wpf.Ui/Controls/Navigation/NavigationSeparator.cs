using System.ComponentModel;
using System.Drawing;
using Wpf.Ui.Controls.Interfaces;

namespace Wpf.Ui.Controls.Navigation;

[ToolboxItem(true)]
[ToolboxBitmap(typeof(NavigationSeparator), "NavigationSeparator.bmp")]
public class NavigationSeparator : System.Windows.Controls.Separator, INavigationControl
{
}
