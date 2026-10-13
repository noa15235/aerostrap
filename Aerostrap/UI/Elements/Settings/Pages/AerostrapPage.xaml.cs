using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aerostrap.UI.ViewModels.Settings;

namespace Aerostrap.UI.Elements.Settings.Pages
{
    public partial class AerostrapPage
    {
        public AerostrapPage()
        {
            DataContext = new AerostrapViewModel();
            InitializeComponent();
        }
    }
}
