using System.ComponentModel;
using System.Drawing;
using System.Windows;
using Wpf.Ui.Controls.States;

namespace Wpf.Ui.Controls;

[ToolboxItem(true)]
[ToolboxBitmap(typeof(ThumbRate), "ThumbRate.bmp")]
public class ThumbRate : System.Windows.Controls.Control
{
    public static readonly DependencyProperty StateProperty = DependencyProperty.Register(nameof(State),
        typeof(ThumbRateState), typeof(ThumbRate),
        new PropertyMetadata(ThumbRateState.None, OnStateChanged));

    public static readonly RoutedEvent StateChangedEvent = EventManager.RegisterRoutedEvent(nameof(StateChanged),
        RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ThumbRate));

    public event RoutedEventHandler StateChanged
    {
        add => AddHandler(StateChangedEvent, value);
        remove => RemoveHandler(StateChangedEvent, value);
    }

    public static readonly DependencyProperty TemplateButtonCommandProperty =
        DependencyProperty.Register(nameof(TemplateButtonCommand),
            typeof(Common.IRelayCommand), typeof(ThumbRate), new PropertyMetadata(null));

    public ThumbRateState State
    {
        get => (ThumbRateState)GetValue(StateProperty);
        set => SetValue(StateProperty, value);
    }

    public Common.IRelayCommand TemplateButtonCommand => (Common.IRelayCommand)GetValue(TemplateButtonCommandProperty);

    public ThumbRate()
    {
        SetValue(TemplateButtonCommandProperty, new Common.RelayCommand(o => OnButtonClick(this, o)));
    }

    protected virtual void OnButtonClick(object sender, object parameter)
    {
        if (parameter is not string)
            return;

        var param = parameter as string;

        switch (param)
        {
            case "up":
                State = State == ThumbRateState.Liked ? ThumbRateState.None : ThumbRateState.Liked;
                break;

            case "down":
                State = State == ThumbRateState.Disliked ? ThumbRateState.None : ThumbRateState.Disliked;
                break;
        }
    }

    protected virtual void OnStateChanged(ThumbRateState previousState, ThumbRateState currentState)
    {
        RaiseEvent(new RoutedEventArgs(StateChangedEvent, this));
    }

    private static void OnStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not ThumbRate thumbRate)
            return;

        thumbRate.OnStateChanged((ThumbRateState)e.OldValue, (ThumbRateState)e.NewValue);
    }
}
