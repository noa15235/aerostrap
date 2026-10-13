using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using Wpf.Ui.Interop;

namespace Wpf.Ui.Dpi;

internal static class DpiHelper
{
    [ThreadStatic]
    private static Matrix _transformToDevice;

    [ThreadStatic]
    private static Matrix _transformToDip;

    internal const int DefaultDpi = 96;

    public static Dpi GetWindowDpi(Window window)
    {
        if (window == null)
            return new Dpi(DefaultDpi, DefaultDpi);

        return GetWindowDpi(new WindowInteropHelper(window).Handle);
    }

    public static Dpi GetWindowDpi(IntPtr windowHandle)
    {
        if (windowHandle == IntPtr.Zero || !UnsafeNativeMethods.IsValidWindow(windowHandle))
            return new Dpi(DefaultDpi, DefaultDpi);

        var windowDpi = (int)User32.GetDpiForWindow(windowHandle);

        return new Dpi(windowDpi, windowDpi);
    }

    public static Dpi GetSystemDpi()
    {
        var dpiXProperty = typeof(SystemParameters).GetProperty("DpiX",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

        if (dpiXProperty == null)
            return new Dpi(DefaultDpi, DefaultDpi);

        var dpiYProperty = typeof(SystemParameters).GetProperty("Dpi",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

        if (dpiYProperty == null)
            return new Dpi(DefaultDpi, DefaultDpi);

        return new Dpi(
            (int)dpiXProperty.GetValue(null, null)!,
            (int)dpiYProperty.GetValue(null, null)!);
    }

    public static Point LogicalPixelsToDevice(Point logicalPoint, double dpiScaleX, double dpiScaleY)
    {
        _transformToDevice = Matrix.Identity;
        _transformToDevice.Scale(dpiScaleX, dpiScaleY);

        return _transformToDevice.Transform(logicalPoint);
    }

    public static Point DevicePixelsToLogical(Point devicePoint, double dpiScaleX, double dpiScaleY)
    {
        _transformToDip = Matrix.Identity;
        _transformToDip.Scale(1d / dpiScaleX, 1d / dpiScaleY);

        return _transformToDip.Transform(devicePoint);
    }

    public static Rect LogicalRectToDevice(Rect logicalRectangle, double dpiScaleX, double dpiScaleY)
    {
        var topLeft = LogicalPixelsToDevice(new Point(logicalRectangle.Left, logicalRectangle.Top), dpiScaleX, dpiScaleY);
        var bottomRight = LogicalPixelsToDevice(new Point(logicalRectangle.Right, logicalRectangle.Bottom), dpiScaleX, dpiScaleY);

        return new Rect(topLeft, bottomRight);
    }

    public static Rect DeviceRectToLogical(Rect deviceRectangle, double dpiScaleX, double dpiScaleY)
    {
        var topLeft = DevicePixelsToLogical(new Point(deviceRectangle.Left, deviceRectangle.Top), dpiScaleX, dpiScaleY);
        var bottomRight = DevicePixelsToLogical(new Point(deviceRectangle.Right, deviceRectangle.Bottom), dpiScaleX, dpiScaleY);

        return new Rect(topLeft, bottomRight);
    }

    public static Size LogicalSizeToDevice(Size logicalSize, double dpiScaleX, double dpiScaleY)
    {
        var pt = LogicalPixelsToDevice(new Point(logicalSize.Width, logicalSize.Height), dpiScaleX, dpiScaleY);

        return new Size { Width = pt.X, Height = pt.Y };
    }

    public static Size DeviceSizeToLogical(Size deviceSize, double dpiScaleX, double dpiScaleY)
    {
        var pt = DevicePixelsToLogical(new Point(deviceSize.Width, deviceSize.Height), dpiScaleX, dpiScaleY);

        return new Size(pt.X, pt.Y);
    }

    public static Thickness LogicalThicknessToDevice(Thickness logicalThickness, double dpiScaleX, double dpiScaleY)
    {
        var topLeft = LogicalPixelsToDevice(new Point(logicalThickness.Left, logicalThickness.Top), dpiScaleX, dpiScaleY);
        var bottomRight = LogicalPixelsToDevice(new Point(logicalThickness.Right, logicalThickness.Bottom), dpiScaleX, dpiScaleY);

        return new Thickness(topLeft.X, topLeft.Y, bottomRight.X, bottomRight.Y);
    }
}

