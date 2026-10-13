using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Wpf.Ui.Mvvm.Contracts;

namespace Wpf.Ui.Mvvm.Services;

public class NotifyIconService : INotifyIconService
{
    private readonly Wpf.Ui.Services.Internal.NotifyIconService _notifyIconService;

    public Window ParentWindow { get; internal set; } = (Window)null!;

    public int Id => _notifyIconService.Id;

    public bool IsRegistered => _notifyIconService.IsRegistered;

    public string TooltipText
    {
        get => _notifyIconService.TooltipText;
        set => _notifyIconService.TooltipText = value;
    }

    public ContextMenu ContextMenu
    {
        get => _notifyIconService.ContextMenu;
        set => _notifyIconService.ContextMenu = value;
    }

    public ImageSource Icon
    {
        get => _notifyIconService.Icon;
        set => _notifyIconService.Icon = value;
    }

    public NotifyIconService()
    {
        _notifyIconService = new Ui.Services.Internal.NotifyIconService();

        RegisterHandlers();
    }

    public bool Register()
    {
        if (ParentWindow != null)
            return _notifyIconService.Register(ParentWindow);

        return _notifyIconService.Register();
    }

    public bool Unregister()
    {
        return _notifyIconService.Unregister();
    }

    public void SetParentWindow(Window parentWindow)
    {
        if (ParentWindow != null)
            ParentWindow.Closing -= OnParentWindowClosing;

        ParentWindow = parentWindow;
        ParentWindow.Closing += OnParentWindowClosing;
    }

    protected virtual void OnLeftClick()
    {
    }

    protected virtual void OnLeftDoubleClick()
    {
    }

    protected virtual void OnRightClick()
    {
    }

    protected virtual void OnRightDoubleClick()
    {
    }

    protected virtual void OnMiddleClick()
    {
    }

    protected virtual void OnMiddleDoubleClick()
    {
    }

    private void OnParentWindowClosing(object sender, CancelEventArgs e)
    {
        _notifyIconService.Dispose();
    }

    private void RegisterHandlers()
    {
        _notifyIconService.LeftClick += OnLeftClick;
        _notifyIconService.LeftDoubleClick += OnLeftDoubleClick;
        _notifyIconService.RightClick += OnRightClick;
        _notifyIconService.RightDoubleClick += OnRightDoubleClick;
        _notifyIconService.MiddleClick += OnMiddleClick;
        _notifyIconService.MiddleDoubleClick += OnMiddleDoubleClick;
    }
}

