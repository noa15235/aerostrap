using System.Reflection;
using System.Windows;

namespace Wpf.Ui.Styles.Controls
{
    partial class Menu : ResourceDictionary
    {
        public Menu() => Initialize();

        private void Initialize()
        {
            if (!SystemParameters.MenuDropAlignment)
                return;

            var fieldInfo = typeof(SystemParameters).GetField("_menuDropAlignment", BindingFlags.NonPublic | BindingFlags.Static);
            fieldInfo?.SetValue(null, false);
        }

    }
}
