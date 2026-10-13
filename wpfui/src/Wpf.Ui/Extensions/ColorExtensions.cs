using System;
using System.Windows.Media;

namespace Wpf.Ui.Extensions;

public static class ColorExtensions
{
    private static float ByteMax = (float)Byte.MaxValue;

    public static SolidColorBrush ToBrush(this Color color)
    {
        return new SolidColorBrush(color);
    }

    public static SolidColorBrush ToBrush(this Color color, double opacity)
    {
        return new SolidColorBrush { Color = color, Opacity = opacity };
    }

    public static double GetLuminance(this Color color)
    {
        var (hue, saturation, luminance) = color.ToHsl();

        return (double)luminance;
    }

    public static double GetBrightness(this Color color)
    {
        var (hue, saturation, brightness) = color.ToHsv();

        return (double)brightness;
    }

    public static double GetHue(this Color color)
    {
        var (hue, saturation, brightness) = color.ToHsv();

        return (double)hue;
    }

    public static double GetSaturation(this Color color)
    {
        var (hue, saturation, brightness) = color.ToHsv();

        return (double)saturation;
    }

    public static Color UpdateLuminance(this Color color, float factor)
    {
        if (factor > 100f || factor < -100f)
            throw new ArgumentOutOfRangeException(nameof(factor));

        var (hue, saturation, rawLuminance) = color.ToHsl();

        var (red, green, blue) = FromHslToRgb(
            hue,
            saturation,
            ToPercentage(rawLuminance + factor));

        return Color.FromArgb(
            color.A,
            ToColorByte(red),
            ToColorByte(green),
            ToColorByte(blue)
        );
    }

    public static Color UpdateSaturation(this Color color, float factor)
    {
        if (factor > 100f || factor < -100f)
            throw new ArgumentOutOfRangeException(nameof(factor));

        var (hue, rawSaturation, brightness) = color.ToHsl();

        var (red, green, blue) = FromHslToRgb(
            hue,
            ToPercentage(rawSaturation + factor),
            brightness
        );

        return Color.FromArgb(
            color.A,
            ToColorByte(red),
            ToColorByte(green),
            ToColorByte(blue)
        );
    }

    public static Color UpdateBrightness(this Color color, float factor)
    {
        if (factor > 100f || factor < -100f)
            throw new ArgumentOutOfRangeException(nameof(factor));

        var (hue, saturation, rawBrightness) = color.ToHsv();

        var (red, green, blue) = FromHsvToRgb(
            hue,
            saturation,
            ToPercentage(rawBrightness + factor)
        );

        return Color.FromArgb(
            color.A,
            ToColorByte(red),
            ToColorByte(green),
            ToColorByte(blue)
        );
    }

    public static Color Update(this Color color, float brightnessFactor, float saturationFactor = 0,
        float luminanceFactor = 0)
    {
        if (brightnessFactor > 100f || brightnessFactor < -100f)
            throw new ArgumentOutOfRangeException(nameof(brightnessFactor));

        if (saturationFactor > 100f || saturationFactor < -100f)
            throw new ArgumentOutOfRangeException(nameof(saturationFactor));

        if (luminanceFactor > 100f || luminanceFactor < -100f)
            throw new ArgumentOutOfRangeException(nameof(luminanceFactor));

        var (hue, rawSaturation, rawBrightness) = color.ToHsv();

        var (red, green, blue) = FromHsvToRgb(hue, ToPercentage(rawSaturation + saturationFactor),
            ToPercentage(rawBrightness + brightnessFactor));

        if (luminanceFactor == 0)
            return Color.FromArgb(
                color.A,
                ToColorByte(red),
                ToColorByte(green),
                ToColorByte(blue)
            );

        (hue, var saturation, var rawLuminance) = Color.FromArgb(
            color.A,
            ToColorByte(red),
            ToColorByte(green),
            ToColorByte(blue)
        ).ToHsl();

        (red, green, blue) = FromHslToRgb(hue, saturation, ToPercentage(rawLuminance + luminanceFactor));

        return Color.FromArgb(
            color.A,
            ToColorByte(red),
            ToColorByte(green),
            ToColorByte(blue)
        );
    }

    public static (float, float, float) ToHsl(this Color color)
    {
        int red = color.R;
        int green = color.G;
        int blue = color.B;

        var max = Math.Max(red, Math.Max(green, blue));
        var min = Math.Min(red, Math.Min(green, blue));

        float fDelta = (max - min) / ByteMax;

        float hue, saturation, lightness;

        if (max <= 0)
            return (0f, 0f, 0f);

        saturation = 0.0f;
        lightness = ((max + min) / ByteMax) / 2.0f;

        if (fDelta <= 0.0)
            return (0f, saturation * 100f, lightness * 100f);

        saturation = fDelta / (max / ByteMax);

        if (max == red)
            hue = ((green - blue) / ByteMax) / fDelta;
        else if (max == green)
            hue = 2f + (((blue - red) / ByteMax) / fDelta);
        else
            hue = 4f + (((red - green) / ByteMax) / fDelta);

        if (hue < 0)
            hue += 360;

        return (hue * 60f, saturation * 100f, lightness * 100f);
    }

    public static (float, float, float) ToHsv(this Color color)
    {
        int red = color.R;
        int green = color.G;
        int blue = color.B;

        var max = Math.Max(red, Math.Max(green, blue));
        var min = Math.Min(red, Math.Min(green, blue));

        float fDelta = (max - min) / ByteMax;

        float hue, saturation, value;

        if (max <= 0)
            return (0f, 0f, 0f);

        saturation = fDelta / (max / ByteMax);
        value = max / ByteMax;

        if (fDelta <= 0.0)
            return (0f, saturation * 100f, value * 100f);

        if (max == red)
            hue = ((green - blue) / ByteMax) / fDelta;
        else if (max == green)
            hue = 2f + (((blue - red) / ByteMax) / fDelta);
        else
            hue = 4f + (((red - green) / ByteMax) / fDelta);

        if (hue < 0)
            hue += 360;

        return (hue * 60f, saturation * 100f, value * 100f);
    }

    public static (int, int, int) FromHslToRgb(float hue, float saturation, float lightness)
    {
        if (AlmostEquals(saturation, 0, 0.01f))
        {
            var color = (int)(lightness * ByteMax);

            return (color, color, color);
        }

        lightness /= 100f;
        saturation /= 100f;

        float hueAngle = hue / 360f;

        return (
            CalcHslChannel(hueAngle + 0.333333333f, saturation, lightness),
            CalcHslChannel(hueAngle, saturation, lightness),
            CalcHslChannel(hueAngle - 0.333333333f, saturation, lightness)
        );
    }

    public static (int, int, int) FromHsvToRgb(float hue, float saturation, float brightness)
    {
        int red = 0, green = 0, blue = 0;

        if (AlmostEquals(saturation, 0, 0.01f))
        {
            red = green = blue = (int)(((brightness / 100f) * ByteMax) + 0.5f);

            return (red, green, blue);
        }

        hue /= 360f;
        brightness /= 100f;
        saturation /= 100f;

        float hueAngle = (hue - (float)Math.Floor(hue)) * 6.0f;
        float f = hueAngle - (float)Math.Floor(hueAngle);

        float p = brightness * (1.0f - saturation);
        float q = brightness * (1.0f - saturation * f);
        float t = brightness * (1.0f - (saturation * (1.0f - f)));

        switch ((int)hueAngle)
        {
            case 0:
                red = (int)(brightness * 255.0f + 0.5f);
                green = (int)(t * 255.0f + 0.5f);
                blue = (int)(p * 255.0f + 0.5f);

                break;
            case 1:
                red = (int)(q * 255.0f + 0.5f);
                green = (int)(brightness * 255.0f + 0.5f);
                blue = (int)(p * 255.0f + 0.5f);

                break;
            case 2:
                red = (int)(p * 255.0f + 0.5f);
                green = (int)(brightness * 255.0f + 0.5f);
                blue = (int)(t * 255.0f + 0.5f);

                break;
            case 3:
                red = (int)(p * 255.0f + 0.5f);
                green = (int)(q * 255.0f + 0.5f);
                blue = (int)(brightness * 255.0f + 0.5f);

                break;
            case 4:
                red = (int)(t * 255.0f + 0.5f);
                green = (int)(p * 255.0f + 0.5f);
                blue = (int)(brightness * 255.0f + 0.5f);

                break;
            case 5:
                red = (int)(brightness * 255.0f + 0.5f);
                green = (int)(p * 255.0f + 0.5f);
                blue = (int)(q * 255.0f + 0.5f);

                break;
        }

        return (red, green, blue);
    }

    private static int CalcHslChannel(float color, float saturation, float lightness)
    {
        float num1, num2;

        if (color > 1)
            color -= 1f;

        if (color < 0)
            color += 1f;

        if (lightness < 0.5f)
            num1 = lightness * (1f + saturation);
        else
            num1 = lightness + saturation - lightness * saturation;

        num2 = 2f * lightness - num1;

        if (color * 6f < 1)
            return (int)((num2 + (num1 - num2) * 6f * color) * ByteMax);

        if (color * 2f < 1)
            return (int)(num1 * ByteMax);

        if (color * 3f < 2)
            return (int)((num2 + (num1 - num2) * (0.666666666f - color) * 6f) * ByteMax);

        return (int)(num2 * ByteMax);
    }

    private static bool AlmostEquals(float numberOne, float numberTwo, float precision = 0)
    {
        if (precision <= 0)
            precision = Single.Epsilon;

        return numberOne >= (numberTwo - precision) && numberOne <= (numberTwo + precision);
    }

    private static float ToPercentage(float value)
    {
        return value switch
        {
            > 100f => 100f,
            < 0f => 0f,
            _ => value
        };
    }

    private static byte ToColorByte(int value)
    {
        if (value > Byte.MaxValue)
            value = Byte.MaxValue;

        if (value < Byte.MinValue)
            value = Byte.MinValue;

        return Convert.ToByte(value);
    }
}
