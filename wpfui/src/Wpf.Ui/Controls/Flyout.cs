using System;
using System.Windows;

namespace Wpf.Ui.Controls;

[TemplatePart(Name = "PART_Popup", Type = typeof(System.Windows.Controls.Primitives.Popup))]
public class Flyout : System.Windows.Controls.ContentControl
{
    private const string ElementPopup = "PART_Popup";

    private System.Windows.Controls.Primitives.Popup _popup = null;

    public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen),
        typeof(bool), typeof(Flyout), new PropertyMetadata(false));

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _popup = GetTemplateChild(ElementPopup) as System.Windows.Controls.Primitives.Popup;

        if (_popup != null)
        {
            _popup.Opened -= OnPopupOpened;
            _popup.Opened += OnPopupOpened;

            _popup.Closed -= OnPopupClosed;
            _popup.Closed += OnPopupClosed;
        }
    }

    public bool IsOpen
    {
        get => (bool)GetValue(IsOpenProperty);
        protected set => SetValue(IsOpenProperty, value);
    }

    public void Show()
    {
        if (!IsOpen)
            IsOpen = true;
    }

    public void Hide()
    {
        if (IsOpen)
            IsOpen = false;
    }

    protected virtual void OnPopupOpened(object sender, EventArgs e)
    {

    }

    protected virtual void OnPopupClosed(object sender, EventArgs e)
    {
        Hide();
    }
}
