using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Wpf.Ui.Controls;

[ToolboxItem(true)]
[ToolboxBitmap(typeof(AutoSuggestBox), "AutoSuggestBox.bmp")]
[TemplatePart(Name = "PART_Popup", Type = typeof(System.Windows.Controls.Primitives.Popup))]
[TemplatePart(Name = "PART_SuggestionsPresenter", Type = typeof(System.Windows.Controls.ListView))]
public class AutoSuggestBox : Wpf.Ui.Controls.TextBox
{
    private string _currentText;

    private const string ElementPopup = "PART_Popup";

    private const string ElementSuggestionsPresenter = "PART_SuggestionsPresenter";

    protected Popup Popup { get; private set; }

    protected ListView SuggestionsPresenter { get; private set; }

    public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource),
        typeof(IEnumerable<string>), typeof(AutoSuggestBox),
        new PropertyMetadata((IEnumerable<string>)null, OnItemsSourceChanged));

    public static readonly DependencyProperty FilteredItemsSourceProperty = DependencyProperty.Register(nameof(FilteredItemsSource),
        typeof(IEnumerable<string>), typeof(AutoSuggestBox),
        new PropertyMetadata((IEnumerable<string>)null));

    public static readonly DependencyProperty IsSuggestionListOpenProperty = DependencyProperty.Register(nameof(IsSuggestionListOpen),
        typeof(bool), typeof(AutoSuggestBox),
        new PropertyMetadata(false));

    public static readonly DependencyProperty MaxDropDownHeightProperty = DependencyProperty.Register(nameof(MaxDropDownHeight),
        typeof(double), typeof(AutoSuggestBox),
        new PropertyMetadata(240d));

    public static readonly RoutedEvent QuerySubmittedEvent = EventManager.RegisterRoutedEvent(
        nameof(QuerySubmitted), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AutoSuggestBox));

    public static readonly RoutedEvent SuggestionChosenEvent = EventManager.RegisterRoutedEvent(
        nameof(SuggestionChosen), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AutoSuggestBox));

    public new bool AcceptsReturn
    {
        get => false;
        set => throw new NotImplementedException($"{typeof(AutoSuggestBox)} does not accept returns.");
    }

    public new int MaxLines
    {
        get => 1;
        set => throw new NotImplementedException($"{typeof(AutoSuggestBox)} does not accept changes to the number of lines.");
    }

    public new int MinLines
    {
        get => 1;
        set => throw new NotImplementedException($"{typeof(AutoSuggestBox)} does not accept changes to the number of lines.");
    }

    public IEnumerable<string> ItemsSource
    {
        get => (IEnumerable<string>)GetValue(ItemsSourceProperty);
        set
        {
            if (value == null)
                ClearValue(ItemsSourceProperty);
            else
                SetValue(ItemsSourceProperty, value);
        }
    }

    public IEnumerable<string> FilteredItemsSource
    {
        get => (IEnumerable<string>)GetValue(FilteredItemsSourceProperty);
        private set
        {
            if (value == null)
                ClearValue(FilteredItemsSourceProperty);
            else
                SetValue(FilteredItemsSourceProperty, value);
        }
    }

    public bool IsSuggestionListOpen
    {
        get => (bool)GetValue(IsSuggestionListOpenProperty);
        set => SetValue(IsSuggestionListOpenProperty, value);
    }

    public double MaxDropDownHeight
    {
        get => (double)GetValue(MaxDropDownHeightProperty);
        set => SetValue(MaxDropDownHeightProperty, value);
    }

    public event RoutedEventHandler QuerySubmitted
    {
        add => AddHandler(QuerySubmittedEvent, value);
        remove => RemoveHandler(QuerySubmittedEvent, value);
    }

    public event RoutedEventHandler SuggestionChosen
    {
        add => AddHandler(SuggestionChosenEvent, value);
        remove => RemoveHandler(SuggestionChosenEvent, value);
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        Popup = GetTemplateChild(ElementPopup) as Popup;
        SuggestionsPresenter = GetTemplateChild(ElementSuggestionsPresenter) as ListView;

        if (SuggestionsPresenter == null)
            return;

        SuggestionsPresenter.SelectionChanged += OnSuggestionsPresenterSelectionChanged;
        SuggestionsPresenter.LostFocus += OnSuggestionsPresenterLostFocus;
    }

    protected override void OnTextChanged(TextChangedEventArgs e)
    {
        base.OnTextChanged(e);

        if (ItemsSource == null || !ItemsSource.Any())
            return;

        var newText = Text;

        if (_currentText == newText)
            return;

        if (String.IsNullOrEmpty(newText))
        {
            FilteredItemsSource = ItemsSource;
        }
        else
        {
            var formattedNewText = newText.ToLower();

            FilteredItemsSource = ItemsSource.Where(elem => elem.ToLower().Contains(formattedNewText)).ToArray();
        }

        OnQuerySubmitted();

        IsSuggestionListOpen = true;
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.Key == Key.Down && Popup != null && SuggestionsPresenter != null && Popup.IsOpen)
        {
            SuggestionsPresenter.Focus();

            e.Handled = true;

            return;
        }

        base.OnKeyDown(e);
    }

    protected override void OnLostFocus(RoutedEventArgs e)
    {
        IsSuggestionListOpen = false;

        base.OnLostFocus(e);
    }

    protected virtual void OnSuggestionsPresenterLostFocus(object sender, RoutedEventArgs e)
    {
        if (!IsFocused)
            IsSuggestionListOpen = false;
    }

    protected virtual void OnSuggestionsPresenterSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is not ListView listView)
            return;

        var selected = listView.SelectedItem;

        listView.UnselectAll();

        _currentText = selected?.ToString() ?? String.Empty;

        Text = _currentText;
        CaretIndex = _currentText.Length;
        IsSuggestionListOpen = false;

        Focus();

        OnSuggestionChosen();
    }

    protected virtual void OnQuerySubmitted()
    {
        var newEvent = new RoutedEventArgs(QuerySubmittedEvent, this);
        RaiseEvent(newEvent);
    }

    protected virtual void OnSuggestionChosen()
    {
        var newEvent = new RoutedEventArgs(SuggestionChosenEvent, this);
        RaiseEvent(newEvent);
    }

    protected virtual void OnItemsSourceChanged(IEnumerable<string> itemsSource)
    {
        FilteredItemsSource = itemsSource;
    }

    private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not AutoSuggestBox autoSuggestBox)
            return;

        autoSuggestBox.OnItemsSourceChanged(e.NewValue as IEnumerable<string>);
    }
}

