using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shapes;

using Windows.Win32;
using Windows.Win32.Graphics.Gdi;

namespace Aerostrap.UI.Elements.Controls
{
    public class SnowfallCanvas : Canvas
    {
        private sealed class Flake
        {
            public Ellipse Shape = null!;
            public TranslateTransform Transform = null!;
            public double BaseX;
            public double Y;
            public double Size;
            public double Speed;
            public double SwayAmplitude;
            public double SwayFrequency;
            public double Phase;
        }

        private const string LOG_IDENT = "SnowfallCanvas";

        private readonly List<Flake> _flakes = new();

        private readonly Random _random = new();

        private TimeSpan _lastRenderTime = TimeSpan.Zero;

        private double _elapsed;

        private int _measuredFrames;

        private double _measureStart;

        private bool _measured;

        public int FlakeCount { get; set; } = 70;

        public int RefreshRate { get; private set; } = 60;

        public SnowfallCanvas()
        {
            IsHitTestVisible = false;
            ClipToBounds = true;

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        public static int GetRefreshRate(Window window)
        {
            var handle = new WindowInteropHelper(window).Handle;
            var screen = System.Windows.Forms.Screen.FromHandle(handle);

            var mode = new DEVMODEW { dmSize = (ushort)Marshal.SizeOf<DEVMODEW>() };

            if (PInvoke.EnumDisplaySettings(screen.DeviceName, ENUM_DISPLAY_SETTINGS_MODE.ENUM_CURRENT_SETTINGS, ref mode) && mode.dmDisplayFrequency > 1)
                return (int)mode.dmDisplayFrequency;

            return 60;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is Window window)
                RefreshRate = GetRefreshRate(window);

            App.Logger.WriteLine(LOG_IDENT, $"Display refresh rate is {RefreshRate} Hz");

            CreateFlakes();

            _lastRenderTime = TimeSpan.Zero;
            CompositionTarget.Rendering += OnRendering;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            CompositionTarget.Rendering -= OnRendering;
        }

        private Brush CreateFlakeBrush()
        {
            var color = App.Settings.Prop.Theme.GetFinal() == Enums.Theme.Dark
                ? Colors.White
                : Color.FromRgb(0x4F, 0x7C, 0xFF);

            var brush = new RadialGradientBrush(
                new GradientStopCollection
                {
                    new GradientStop(color, 0.0),
                    new GradientStop(Color.FromArgb(0xB0, color.R, color.G, color.B), 0.45),
                    new GradientStop(Color.FromArgb(0x00, color.R, color.G, color.B), 1.0)
                });

            brush.Freeze();
            return brush;
        }

        private void CreateFlakes()
        {
            Children.Clear();
            _flakes.Clear();

            var brush = CreateFlakeBrush();

            for (int i = 0; i < FlakeCount; i++)
            {
                double depth = _random.NextDouble();

                var flake = new Flake
                {
                    Size = 2 + depth * 5,
                    Speed = 18 + depth * 42,
                    SwayAmplitude = 6 + _random.NextDouble() * 14,
                    SwayFrequency = 0.4 + _random.NextDouble() * 0.8,
                    Phase = _random.NextDouble() * Math.PI * 2,
                    BaseX = _random.NextDouble() * Math.Max(ActualWidth, 1),
                    Y = _random.NextDouble() * Math.Max(ActualHeight, 1)
                };

                flake.Transform = new TranslateTransform();
                flake.Shape = new Ellipse
                {
                    Width = flake.Size,
                    Height = flake.Size,
                    Fill = brush,
                    Opacity = 0.35 + depth * 0.65,
                    RenderTransform = flake.Transform
                };

                Children.Add(flake.Shape);
                _flakes.Add(flake);
            }
        }

        private void OnRendering(object? sender, EventArgs e)
        {
            var renderingTime = ((RenderingEventArgs)e).RenderingTime;

            if (renderingTime == _lastRenderTime)
                return;

            double delta = _lastRenderTime == TimeSpan.Zero ? 0 : (renderingTime - _lastRenderTime).TotalSeconds;
            _lastRenderTime = renderingTime;

            if (delta > 0.25)
                delta = 0.25;

            _elapsed += delta;

            MeasureFrameRate();

            double width = ActualWidth;
            double height = ActualHeight;

            if (width <= 0 || height <= 0)
                return;

            foreach (var flake in _flakes)
            {
                flake.Y += flake.Speed * delta;

                if (flake.Y > height + flake.Size)
                {
                    flake.Y = -flake.Size;
                    flake.BaseX = _random.NextDouble() * width;
                }

                double x = flake.BaseX + Math.Sin(_elapsed * flake.SwayFrequency * Math.PI * 2 + flake.Phase) * flake.SwayAmplitude;

                flake.Transform.X = x;
                flake.Transform.Y = flake.Y;
            }
        }

        private void MeasureFrameRate()
        {
            if (_measured)
                return;

            if (_measuredFrames == 0)
                _measureStart = _elapsed;

            _measuredFrames++;

            double span = _elapsed - _measureStart;

            if (span >= 2)
            {
                _measured = true;
                App.Logger.WriteLine(LOG_IDENT, $"Animating at {_measuredFrames / span:0} fps");
            }
        }
    }
}
