using System.Windows.Media;

namespace Wpf.Ui.Controls.Interfaces;

public interface IIconControl
{
    Common.SymbolRegular Icon { get; set; }

    bool IconFilled { get; set; }

    Brush IconForeground { get; set; }
}
