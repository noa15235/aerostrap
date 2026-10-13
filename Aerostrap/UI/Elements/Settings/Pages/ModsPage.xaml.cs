using Aerostrap.UI.ViewModels.Settings;

namespace Aerostrap.UI.Elements.Settings.Pages
{
    public partial class ModsPage
    {
        public ModsPage()
        {
            DataContext = new ModsViewModel();
            InitializeComponent();
        }
    }
}
