using Aerostrap.Integrations;
using Aerostrap.UI.ViewModels.ContextMenu;

namespace Aerostrap.UI.Elements.ContextMenu
{
    public partial class ServerHistory
    {
        public ServerHistory(ActivityWatcher watcher)
        {
            var viewModel = new ServerHistoryViewModel(watcher);

            viewModel.RequestCloseEvent += (_, _) => Close();

            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
