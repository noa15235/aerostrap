using System;
using System.Windows.Controls;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace Wpf.Ui.Mvvm.Services;

public partial class NavigationService : INavigationService
{
    private IPageService _pageService;

    protected INavigation NavigationControl;

    public Frame GetFrame()
    {
        return NavigationControl?.Frame;
    }

    public void SetFrame(Frame frame)
    {
        if (NavigationControl == null)
            return;

        NavigationControl.Frame = frame;
    }

    public INavigation GetNavigationControl()
    {
        return NavigationControl;
    }

    public void SetNavigationControl(INavigation navigation)
    {
        NavigationControl = navigation;

        if (_pageService != null)
            NavigationControl.PageService = _pageService;
    }

    public void SetPageService(IPageService pageService)
    {
        if (NavigationControl == null)
        {
            _pageService = pageService;

            return;
        }

        NavigationControl.PageService = pageService;
    }

    public bool Navigate(Type pageType)
    {
        if (NavigationControl == null)
            return false;

        return NavigationControl.Navigate(pageType);
    }

    public bool Navigate(int pageId)
    {
        if (NavigationControl == null)
            return false;

        return NavigationControl.Navigate(pageId);
    }

    public bool Navigate(string pageTag)
    {
        if (NavigationControl == null)
            return false;

        return NavigationControl.Navigate(pageTag);
    }
}
