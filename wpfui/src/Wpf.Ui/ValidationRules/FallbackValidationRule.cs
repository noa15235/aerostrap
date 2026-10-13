using System.Globalization;
using System.Windows.Controls;

namespace Wpf.Ui.ValidationRules;

internal class FallbackValidationRule : ValidationRule
{
    public FallbackValidationRule()
    {
    }

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        return ValidationResult.ValidResult;
    }
}
