using System;
using System.Windows.Controls;
using Wpf.Ui.Controls.Interfaces;

namespace Wpf.Ui.Mvvm.Contracts;

public interface INavigationService
{
    Frame GetFrame();

    void SetFrame(Frame frame);

    INavigation GetNavigationControl();

    void SetNavigationControl(INavigation navigation);

    void SetPageService(IPageService pageService);

    bool Navigate(Type pageType);

    bool Navigate(int pageId);

    bool Navigate(string pageTag);
}
