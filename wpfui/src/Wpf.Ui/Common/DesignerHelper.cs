using System.ComponentModel;
using System.Windows;

namespace Wpf.Ui.Common;

public static class DesignerHelper
{
    private static bool _validated = false;

    private static bool _isInDesignMode = false;

    public static bool IsInDesignMode
    {
        get
        {
            if (_isInDesignMode)
                return true;

            if (_validated)
                _isInDesignMode = (bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject))
                    ?.DefaultValue ?? false);

            _validated = true;

            return _isInDesignMode;
        }
    }

    public static bool IsDebugging => System.Diagnostics.Debugger.IsAttached;
}
