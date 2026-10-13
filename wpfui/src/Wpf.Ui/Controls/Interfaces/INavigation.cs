using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using Wpf.Ui.Animations;
using Wpf.Ui.Common;
using Wpf.Ui.Mvvm.Contracts;

namespace Wpf.Ui.Controls.Interfaces;

public interface INavigation
{
    IPageService PageService { get; set; }

    int SelectedPageIndex { get; set; }

    int PreviousPageIndex { get; }

    bool Precache { get; set; }

    bool CanGoBack { get; }

    INavigationItem Current { get; }

    [Bindable(true)]
    Frame Frame { get; set; }

    [Bindable(true)]
    ObservableCollection<INavigationControl> Items { get; set; }

    [Bindable(true)]
    ObservableCollection<INavigationControl> Footer { get; set; }

    Orientation Orientation { get; set; }

    [Category("Behavior")]
    event RoutedNavigationEvent Navigated;

    [Category("Behavior")]
    event RoutedNavigationEvent NavigatedForward;

    [Category("Behavior")]
    event RoutedNavigationEvent NavigatedBackward;

    [Bindable(true), Category("Appearance")]
    int TransitionDuration { get; set; }

    [Bindable(true), Category("Appearance")]
    TransitionType TransitionType { get; set; }

    void Flush();

    void ClearCache();

    bool NavigateBack();

    bool Navigate(Type pageType);

    bool Navigate(Type pageType, object dataContext);

    bool Navigate(int pageIndex);

    bool Navigate(int pageIndex, object dataContext);

    bool Navigate(string pageTag);

    bool Navigate(string pageTag, object dataContext);

    bool NavigateExternal(object frameworkElement);

    bool NavigateExternal(object frameworkElement, object dataContext);

    bool NavigateExternal(Uri absolutePageUri);

    bool NavigateExternal(Uri absolutePageUri, object dataContext);

    bool SetContext(int pageId, object dataContext);

    bool SetContext(string pageTag, object dataContext);

    void SetCurrentContext(object dataContext);
}
