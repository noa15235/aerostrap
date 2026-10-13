#nullable enable
#pragma warning disable CS8601
#pragma warning disable CS8625

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Wpf.Ui.Win32;

internal class Utilities
{
    private static readonly PlatformID _osPlatform = Environment.OSVersion.Platform;

    private static readonly Version _osVersion =
#if NET5_0_OR_GREATER
        Environment.OSVersion.Version;
#else
        GetOSVersionFromRegistry();
#endif

    public static bool IsNT => _osPlatform == PlatformID.Win32NT;

    public static bool IsOSVistaOrNewer => _osVersion >= new Version(6, 0);

    public static bool IsOSWindows7OrNewer => _osVersion >= new Version(6, 1);

    public static bool IsOSWindows8OrNewer => _osVersion >= new Version(6, 2);

    public static bool IsOSWindows10OrNewer => _osVersion.Build >= 10240;

    public static bool IsOSWindows11OrNewer => _osVersion.Build >= 22000;

    public static bool IsOSWindows11Insider1OrNewer => _osVersion.Build >= 22523;

    public static bool IsOSWindows11Insider2OrNewer => _osVersion.Build >= 22557;

    public static bool IsCompositionEnabled
    {
        get
        {
            if (!IsOSVistaOrNewer)
                return false;

            Interop.Dwmapi.DwmIsCompositionEnabled(out var pfEnabled);

            return pfEnabled != 0;
        }
    }

    public static void SafeDispose<T>(ref T disposable) where T : IDisposable
    {
        IDisposable t = disposable;
        disposable = default(T);

        if (null == t)
            return;

        t.Dispose();
    }

    public static void SafeRelease<T>(ref T comObject) where T : class
    {
        T t = comObject;
        comObject = default(T);

        if (null == t)
            return;

        Debug.Assert(Marshal.IsComObject(t));
        Marshal.ReleaseComObject(t);
    }

#if !NET5_0_OR_GREATER
    private static Version GetOSVersionFromRegistry()
    {
        int major = 0;
        {
            if (TryGetRegistryKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CurrentMajorVersionNumber",
                    out var majorObj))
            {
                majorObj ??= 0;

                major = (int)majorObj;
            }

            else if (TryGetRegistryKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CurrentVersion",
                         out var version))
            {
                version ??= String.Empty;

                var versionParts = ((string)version).Split('.');

                if (versionParts.Length >= 2)
                    major = int.TryParse(versionParts[0], out int majorAsInt) ? majorAsInt : 0;
            }
        }

        int minor = 0;
        {
            if (TryGetRegistryKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CurrentMinorVersionNumber",
                    out var minorObj))
            {
                minorObj ??= String.Empty;

                minor = (int)minorObj;
            }

            else if (TryGetRegistryKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CurrentVersion",
                         out var version))
            {
                version ??= String.Empty;

                var versionParts = ((string)version).Split('.');

                if (versionParts.Length >= 2)
                    minor = int.TryParse(versionParts[1], out int minorAsInt) ? minorAsInt : 0;
            }
        }

        int build = 0;
        {
            if (TryGetRegistryKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CurrentBuildNumber",
                    out var buildObj))
            {
                buildObj ??= String.Empty;

                build = int.TryParse((string)buildObj, out int buildAsInt) ? buildAsInt : 0;
            }
        }

        return new(major, minor, build);
    }

    private static bool TryGetRegistryKey(string path, string key, out object? value)
    {
        value = null;

        try
        {
            using var rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(path);

            if (rk == null)
                return false;

            value = rk.GetValue(key);

            return value != null;
        }
        catch
        {
            return false;
        }
    }

#endif
}
