using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Wpf.Ui.Tray;

internal static class Hicon
{
    public static IntPtr FromApp()
    {
        try
        {
            var processName = Process.GetCurrentProcess().MainModule?.FileName;

            if (String.IsNullOrEmpty(processName))
                return IntPtr.Zero;

            var appIconsExtractIcon = System.Drawing.Icon.ExtractAssociatedIcon(processName);

            if (appIconsExtractIcon == null)
                return IntPtr.Zero;

            return appIconsExtractIcon.Handle;
        }
#if DEBUG
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine($"ERROR | Unable to get application hIcon - {e}", "Wpf.Ui.Hicon");
            throw;
#else
        catch
        {
            return IntPtr.Zero;
#endif
        }
    }

    public static IntPtr FromSource(ImageSource source)
    {
        var hIcon = IntPtr.Zero;
        var bitmapFrame = source as BitmapFrame;

        if (bitmapFrame?.Decoder == null || bitmapFrame.Decoder.Frames.Count < 1)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"ERROR | Unable to allocate hIcon, Decoder source is empty", "Wpf.Ui.Hicon");
#endif

            return IntPtr.Zero;
        }

        bitmapFrame = bitmapFrame.Decoder.Frames[0];

        var stride = bitmapFrame.PixelWidth * ((bitmapFrame.Format.BitsPerPixel + 7) / 8);
        var pixels = new byte[bitmapFrame.PixelHeight * stride];

        bitmapFrame.CopyPixels(pixels, stride, 0);

        var gcHandle = GCHandle.Alloc(pixels, GCHandleType.Pinned);

        if (!gcHandle.IsAllocated)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"ERROR | Unable to allocate hIcon, allocation failed.", "Wpf.Ui.Hicon");
#endif

            return IntPtr.Zero;
        }

        var bitmap = new Bitmap(bitmapFrame.PixelWidth, bitmapFrame.PixelHeight, stride,
            System.Drawing.Imaging.PixelFormat.Format32bppPArgb, gcHandle.AddrOfPinnedObject());

        hIcon = bitmap.GetHicon();

        gcHandle.Free();

        return hIcon;
    }
}

