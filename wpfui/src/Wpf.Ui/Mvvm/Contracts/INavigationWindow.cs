using System;
using System.Windows.Controls;
using Wpf.Ui.Controls.Interfaces;

namespace Wpf.Ui.Mvvm.Contracts;

public interface INavigationWindow
{
    Frame GetFrame();

    INavigation GetNavigation();

    bool Navigate(Type pageType);

    void SetPageService(IPageService pageService);

    void ShowWindow();

    void CloseWindow();
}
