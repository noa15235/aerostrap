namespace Wpf.Ui.Controls.Interfaces;

public interface IDpiAwareControl
{
    Dpi.Dpi CurrentWindowDpi { get; }
}
