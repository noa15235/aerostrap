using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Wpf.Ui.Controls;

[ToolboxItem(true)]
[ToolboxBitmap(typeof(NumberBox), "NumberBox.bmp")]
public class NumberBox : Wpf.Ui.Controls.TextBox
{

    private readonly string _decimalExpression = @"^\-?(\d+(?:[\.\,]|[\.\,]\d+)?)?$";

    private readonly string _integerExpression = @"^\-?(\d+)*$";

    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value),
        typeof(double), typeof(NumberBox), new PropertyMetadata(0.0d, OnValuePropertyChanged));

    public static readonly DependencyProperty StepProperty = DependencyProperty.Register(nameof(Step),
        typeof(double), typeof(NumberBox), new PropertyMetadata(1.0d));

    public static readonly DependencyProperty MaxProperty = DependencyProperty.Register(nameof(Max),
        typeof(double), typeof(NumberBox), new PropertyMetadata(Double.MaxValue));

    public static readonly DependencyProperty MinProperty = DependencyProperty.Register(nameof(Min),
        typeof(double), typeof(NumberBox), new PropertyMetadata(Double.MinValue));

    public static readonly DependencyProperty DecimalPlacesProperty = DependencyProperty.Register(nameof(DecimalPlaces),
        typeof(int), typeof(NumberBox), new PropertyMetadata(2, OnDecimalPlacesChanged));

    public static readonly DependencyProperty MaskProperty = DependencyProperty.Register(nameof(Mask),
        typeof(string), typeof(NumberBox), new PropertyMetadata(String.Empty));

    public static readonly DependencyProperty SpinButtonsEnabledProperty = DependencyProperty.Register(nameof(SpinButtonsEnabled),
        typeof(bool), typeof(NumberBox), new PropertyMetadata(true));

    public static readonly DependencyProperty IntegersOnlyProperty = DependencyProperty.Register(nameof(IntegersOnly),
        typeof(bool), typeof(NumberBox), new PropertyMetadata(false));

    public static readonly RoutedEvent IncrementedEvent = EventManager.RegisterRoutedEvent(
        nameof(Incremented), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NumberBox));

    public static readonly RoutedEvent DecrementedEvent = EventManager.RegisterRoutedEvent(
        nameof(Decremented), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NumberBox));

    public new bool AcceptsReturn
    {
        get => false;
        set => throw new NotImplementedException($"{typeof(NumberBox)} does not accept returns.");
    }

    public new int MaxLines
    {
        get => 1;
        set => throw new NotImplementedException($"{typeof(NumberBox)} does not accept changes to the number of lines.");
    }

    public new int MinLines
    {
        get => 1;
        set => throw new NotImplementedException($"{typeof(NumberBox)} does not accept changes to the number of lines.");
    }

    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public double Step
    {
        get => (double)GetValue(StepProperty);
        set => SetValue(StepProperty, value);
    }

    public double Max
    {
        get => (double)GetValue(MaxProperty);
        set => SetValue(MaxProperty, value);
    }

    public double Min
    {
        get => (double)GetValue(MinProperty);
        set => SetValue(MinProperty, value);
    }

    public int DecimalPlaces
    {
        get => (int)GetValue(DecimalPlacesProperty);
        set => SetValue(DecimalPlacesProperty, value);
    }

    public string Mask
    {
        get => (string)GetValue(MaskProperty);
        set => SetValue(MaskProperty, value);
    }

    public bool SpinButtonsEnabled
    {
        get => (bool)GetValue(SpinButtonsEnabledProperty);
        set => SetValue(SpinButtonsEnabledProperty, value);
    }

    public bool IntegersOnly
    {
        get => (bool)GetValue(IntegersOnlyProperty);
        set => SetValue(IntegersOnlyProperty, value);
    }

    public event RoutedEventHandler Incremented
    {
        add => AddHandler(IncrementedEvent, value);
        remove => RemoveHandler(IncrementedEvent, value);
    }

    public event RoutedEventHandler Decremented
    {
        add => AddHandler(DecrementedEvent, value);
        remove => RemoveHandler(DecrementedEvent, value);
    }

    static NumberBox()
    {
        AcceptsReturnProperty.OverrideMetadata(typeof(PasswordBox), new FrameworkPropertyMetadata(false));
        MaxLinesProperty.OverrideMetadata(typeof(PasswordBox), new FrameworkPropertyMetadata(1));
        MinLinesProperty.OverrideMetadata(typeof(PasswordBox), new FrameworkPropertyMetadata(1));
    }

    public NumberBox()
    {
        DataObject.AddPastingHandler(this, OnClipboardPaste);

        Loaded += OnLoaded;
    }

    protected virtual void OnValueChanged()
    {

    }

    protected override void OnTemplateButtonClick(object sender, object parameter)
    {
        base.OnTemplateButtonClick(sender, parameter);

        if (sender is not NumberBox)
            return;

        if (parameter is not string parameterString)
            return;

        switch (parameterString)
        {
            case "increment":
                IncrementValue();
                break;

            case "decrement":
                DecrementValue();
                break;
        }
    }

    private void UpdateValue(double value, bool updateText)
    {
        Value = value;

        if (!updateText)
            return;

        var newText = FormatDoubleToString(value);

        Text = newText;
        CaretIndex = newText.Length;
    }

    private void UpdateValue(double value, string updateText)
    {
        Value = value;

        Text = updateText;
        CaretIndex = updateText.Length;
    }

    private void IncrementValue()
    {
        var currentText = Text;
        var parsedNumber = ParseStringToDouble(currentText) + Step;

        if (String.IsNullOrWhiteSpace(currentText) || parsedNumber > Max)
        {
            UpdateValue(Max, true);

            return;
        }

        UpdateValue(parsedNumber, true);

        OnIncremented();
    }

    private void DecrementValue()
    {
        var currentText = Text;
        var parsedNumber = ParseStringToDouble(currentText) - Step;

        if (String.IsNullOrWhiteSpace(currentText) || parsedNumber < Min)
        {
            UpdateValue(Min, true);

            return;
        }

        UpdateValue(parsedNumber, true);

        OnDecremented();
    }

    private string FormatDoubleToString(double number)
    {
        if (IntegersOnly || DecimalPlaces < 1)
            return number.ToString("F0", CultureInfo.InvariantCulture);

        if (DecimalPlaces < 5)
            return number.ToString($"F{DecimalPlaces}", CultureInfo.InvariantCulture);

        return number.ToString(CultureInfo.InvariantCulture);
    }

    private bool IsNumberTextValid(string inputText)
    {

        var decimalPlaces = DecimalPlaces;
        var integerRegex = new Regex(_integerExpression);
        var decimalRegex = new Regex(_decimalExpression);

        if (IntegersOnly || decimalPlaces < 1)
            return integerRegex.IsMatch(inputText);

        if (!decimalRegex.IsMatch(inputText))
            return false;

        if (inputText.Contains(",") && inputText.Substring(inputText.IndexOf(",")).Length > decimalPlaces)
            return false;

        if (inputText.Contains(".") && inputText.Substring(inputText.IndexOf(".")).Length > decimalPlaces)
            return false;

        return true;
    }

    private string FormatWithMask(string currentInput, string newInput)
    {

        return currentInput;
    }

    private double ParseStringToDouble(string inputText)
    {
        Double.TryParse(inputText, NumberStyles.Any, CultureInfo.InvariantCulture, out double number);

        return number;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        Text = FormatDoubleToString(Value);
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
        if (e.Key == Key.Up)
        {
            IncrementValue();

            e.Handled = true;
        }

        if (e.Key == Key.Down)
        {
            DecrementValue();

            e.Handled = true;
        }

        base.OnKeyUp(e);
    }

    protected override void OnTextChanged(TextChangedEventArgs e)
    {
        base.OnTextChanged(e);

        var currentText = Text;
        var parsedNumber = ParseStringToDouble(currentText);

        PlaceholderEnabled = currentText.Length < 1;

        if (parsedNumber > Max)
        {
            UpdateValue(Max, true);

            return;
        }

        if (parsedNumber < Min)
        {
            UpdateValue(Min, true);

            return;
        }

        UpdateValue(parsedNumber, true);
    }

    protected override void OnPreviewTextInput(TextCompositionEventArgs e)
    {
        var newText = Text + (e.Text ?? String.Empty);

        if (!String.IsNullOrEmpty(newText))
            e.Handled = !IsNumberTextValid(newText);

        if (Min >= 0 && newText.StartsWith("-"))
            e.Handled = true;

        base.OnPreviewTextInput(e);
    }

    protected virtual void OnIncremented()
    {
        RaiseEvent(new RoutedEventArgs(IncrementedEvent, this));
    }

    protected virtual void OnDecremented()
    {
        RaiseEvent(new RoutedEventArgs(DecrementedEvent, this));
    }

    protected virtual void OnDecimalPlacesChanged(int decimalPlaces)
    {
        if (decimalPlaces < 0)
            DecimalPlaces = 0;
    }

    private void OnClipboardPaste(object sender, DataObjectPastingEventArgs e)
    {
        if (sender is not NumberBox control)
            return;

        var clipboardText = (string)e.DataObject.GetData(typeof(string));

        if (!IsNumberTextValid(clipboardText))
            e.CancelCommand();
    }

    private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NumberBox numberBox)
            return;

        numberBox.OnValueChanged();
    }

    private static void OnDecimalPlacesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NumberBox control)
            return;

        if (e.NewValue is not int newValue)
            return;

        control.OnDecimalPlacesChanged(newValue);
    }
}
