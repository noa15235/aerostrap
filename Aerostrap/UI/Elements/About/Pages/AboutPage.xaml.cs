using Aerostrap.UI.ViewModels.About;

namespace Aerostrap.UI.Elements.About.Pages
{
    public partial class AboutPage
    {
        public AboutPage()
        {
            DataContext = new AboutViewModel();
            InitializeComponent();
        }
    }
}
