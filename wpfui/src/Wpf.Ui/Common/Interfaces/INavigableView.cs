using Wpf.Ui.Controls.Interfaces;

namespace Wpf.Ui.Common.Interfaces;

public interface INavigableView<out T>
{
    T ViewModel { get; }
}
