using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using Aerostrap.UI.Elements.Controls;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;

namespace Aerostrap.UI.Elements.Base
{
    public abstract class WpfUiWindow : UiWindow
    {
        private readonly IThemeService _themeService = new ThemeService();

        protected virtual bool ShowSnowfall => true;

        public WpfUiWindow()
        {
            ApplyTheme();
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            if (!ShowSnowfall || newContent is not UIElement element)
                return;

            if (element is Grid grid)
            {
                if (!grid.Children.OfType<SnowfallCanvas>().Any())
                {
                    var snowfall = new SnowfallCanvas();
                    Grid.SetRowSpan(snowfall, 100);
                    Grid.SetColumnSpan(snowfall, 100);
                    Panel.SetZIndex(snowfall, 8);
                    grid.Children.Add(snowfall);
                }

                return;
            }

            var wrapper = new Grid();
            Content = wrapper;
            wrapper.Children.Add(element);
        }

        public void ApplyTheme()
        {
            const int customThemeIndex = 2;

            var themeType = App.Settings.Prop.Theme.GetFinal() == Enums.Theme.Dark ? ThemeType.Dark : ThemeType.Light;
            _themeService.SetTheme(themeType);
            Accent.Apply(System.Windows.Media.Color.FromRgb(0x4F, 0x7C, 0xFF), themeType);

            var dict = new ResourceDictionary { Source = new Uri($"pack://application:,,,/UI/Style/{Enum.GetName(App.Settings.Prop.Theme.GetFinal())}.xaml") };
            Application.Current.Resources.MergedDictionaries[customThemeIndex] = dict;

#if QA_BUILD
            this.BorderBrush = System.Windows.Media.Brushes.Red;
            this.BorderThickness = new Thickness(4);
#endif
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            if (App.Settings.Prop.WPFSoftwareRender || App.LaunchSettings.NoGPUFlag.Active)
            {
                if (PresentationSource.FromVisual(this) is HwndSource hwndSource)
                    hwndSource.CompositionTarget.RenderMode = RenderMode.SoftwareOnly;
            }

            base.OnSourceInitialized(e);
        }
    }
}
