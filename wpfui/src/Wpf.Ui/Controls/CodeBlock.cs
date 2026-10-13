using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using Wpf.Ui.Appearance;
using Color = System.Windows.Media.Color;

namespace Wpf.Ui.Controls;

[ToolboxItem(true)]
[ToolboxBitmap(typeof(CodeBlock), "CodeBlock.bmp")]
public class CodeBlock : System.Windows.Controls.ContentControl
{
    private string _sourceCode = String.Empty;

    public static readonly DependencyProperty SyntaxContentProperty = DependencyProperty.Register(nameof(SyntaxContent),
        typeof(object), typeof(CodeBlock),
        new PropertyMetadata(null));

    public static readonly DependencyProperty ButtonCommandProperty =
        DependencyProperty.Register(nameof(NumberBox),
            typeof(Common.IRelayCommand), typeof(CodeBlock), new PropertyMetadata(null));

    public object SyntaxContent
    {
        get => GetValue(SyntaxContentProperty);
        internal set => SetValue(SyntaxContentProperty, value);
    }

    public Common.IRelayCommand ButtonCommand => (Common.IRelayCommand)GetValue(ButtonCommandProperty);

    public CodeBlock()
    {
        SetValue(ButtonCommandProperty, new Common.RelayCommand(o => Button_Click(this, o)));

        Appearance.Theme.Changed += ThemeOnChanged;
    }

    private void ThemeOnChanged(ThemeType currentTheme, Color systemAccent)
    {
        UpdateSyntax();
    }

    protected override void OnContentChanged(object oldContent, object newContent)
    {
        UpdateSyntax();
    }

    protected virtual void UpdateSyntax()
    {
        _sourceCode = Syntax.Highlighter.Clean(Content as string ?? String.Empty);
        SyntaxContent = Syntax.Highlighter.Format(_sourceCode);
    }

    private void Button_Click(object sender, object parameter)
    {
#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO | CodeBlock source: \n{_sourceCode}", "Wpf.Ui.CodeBlock");
#endif
        Wpf.Ui.Common.Clipboard.SetText(_sourceCode);
    }
}
