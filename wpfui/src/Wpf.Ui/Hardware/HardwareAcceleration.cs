using System.Windows.Media;

namespace Wpf.Ui.Hardware;

public static class HardwareAcceleration
{
    public static bool IsSupported(RenderingTier tier)
    {
        return RenderCapability.Tier >> 16 >= (int)tier;
    }
}
