#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Wpf.Ui.Animations;
using Wpf.Ui.Common;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Interfaces;

namespace Wpf.Ui.Services.Internal;

internal sealed class NavigationService : IDisposable
{
    #region Private properties

    private bool _disposed = false;

    private int _currentPageIndex = -1;

    private int _previousPageIndex = -1;

    private Frame? _frame;

    private IPageService? _pageService;

    private long _currentActionIdentifier { get; set; }

    private readonly EventIdentifier _eventIdentifier;

    private NavigationServiceItem[] _navigationServiceItems;

    private readonly List<int> _history;

    private bool _isBackNavigated;

    #endregion Private properties

    #region Public properties

    public bool Precache { get; set; } = false;

    public int TransitionDuration { get; set; }

    public TransitionType TransitionType { get; set; }

    public bool CanGoBack => _history.Count > 1;

    #endregion Public properties

    #region Constructors

    public NavigationService()
    {
        _eventIdentifier = new EventIdentifier();
        _navigationServiceItems = new NavigationServiceItem[] { };
        _history = new List<int>();
    }

    ~NavigationService()
    {
        Dispose(false);
    }

    #endregion Constructors

    #region Public methods

    public bool NavigateBack()
    {
        if (_history.Count <= 1)
            return false;

        _isBackNavigated = true;

        return NavigateInternal(_history[_history.Count - 2], null!);
    }

    public bool Navigate(int pageId, object? dataContext)
    {
        return NavigateInternal(pageId, dataContext);
    }

    public bool Navigate(Type pageType, object? dataContext)
    {
        var selectedIndex = -1;

        for (var i = 0; i < _navigationServiceItems.Length; i++)
        {
            if (_navigationServiceItems[i].Type != pageType)
                continue;

            selectedIndex = i;

            break;
        }

        if (selectedIndex >= 0)
            return NavigateInternal(selectedIndex, dataContext);

        if (_pageService == null)
            return false;

        var servicePageInstance = _pageService.GetPage(pageType);

        if (servicePageInstance == null)
            throw new InvalidOperationException($"The {pageType} has not been registered in the {typeof(IPageService)} service.");

        _previousPageIndex = _currentPageIndex;
        _currentPageIndex = -1;

        _currentActionIdentifier = _eventIdentifier.GetNext();

        _frame?.Navigate(servicePageInstance);

        return true;
    }

    public bool Navigate(string pageTag, object? dataContext)
    {
        var selectedIndex = -1;

        for (var i = 0; i < _navigationServiceItems.Length; i++)
        {
            if (_navigationServiceItems[i].Tag != pageTag)
                continue;

            selectedIndex = i;

            break;
        }

        if (selectedIndex < 0)
            return false;

        return NavigateInternal(selectedIndex, dataContext);
    }

    public bool NavigateExternal(object frameworkElement, object? dataContext)
    {
        if (_frame == null)
            return false;

        if (frameworkElement is not FrameworkElement)
            throw new InvalidOperationException($"Only class derived {typeof(FrameworkElement)} can be used for navigation.");

        _previousPageIndex = _currentPageIndex;
        _currentPageIndex = -1;

        _currentActionIdentifier = _eventIdentifier.GetNext();

        _frame.Navigate(
            frameworkElement,
            new NavigationServiceExtraData
            {
                PageId = -1,
                Cache = false,
                DataContext = dataContext
            });

        return true;
    }

    public bool NavigateExternal(Uri frameworkElementUri, object? dataContext)
    {
        if (_frame == null)
            return false;

        if (!frameworkElementUri.IsAbsoluteUri)
            throw new InvalidOperationException($"Navigation Uri must be absolute Uri pointing to an element derived from {typeof(FrameworkElement)}.");

        _previousPageIndex = _currentPageIndex;
        _currentPageIndex = -1;

        _currentActionIdentifier = _eventIdentifier.GetNext();

        _frame.Navigate(
            frameworkElementUri,
            new NavigationServiceExtraData
            {
                PageId = -1,
                Cache = false,
                DataContext = dataContext
            });

        return true;
    }

    public bool SetContext(string pageTag, object dataContext)
    {
        for (var i = 0; i < _navigationServiceItems.Length; i++)
        {
            if (_navigationServiceItems[i].Tag != pageTag)
                continue;

            if (_navigationServiceItems[i].Instance is not FrameworkElement)
                return false;

            ((FrameworkElement)_navigationServiceItems[i].Instance).DataContext = dataContext;

            return true;
        }

        return false;
    }

    public bool SetContext(int serviceItemId, object dataContext)
    {
        if (_navigationServiceItems.Length - 1 < serviceItemId)
            return false;

        if (_navigationServiceItems[serviceItemId].Instance is not FrameworkElement)
            return false;

        ((FrameworkElement)_navigationServiceItems[serviceItemId].Instance).DataContext = dataContext;

        return true;
    }

    public void UpdateItems(IEnumerable<INavigationControl>? mainItems, IEnumerable<INavigationControl>? additionalItems)
    {
        var serviceItemCollection = new List<NavigationServiceItem> { };

        if (mainItems != null)
            foreach (var singleNavigationControl in mainItems)
            {
                if (singleNavigationControl is not INavigationItem navigationItem)
                    continue;

                serviceItemCollection.Add(NavigationServiceItem.Create(navigationItem));
            }

        if (additionalItems != null)
            foreach (var singleNavigationControl in additionalItems)
            {
                if (singleNavigationControl is not INavigationItem navigationItem)
                    continue;

                serviceItemCollection.Add(NavigationServiceItem.Create(navigationItem));
            }

        _navigationServiceItems = serviceItemCollection.ToArray();

    }

    public void ClearCache()
    {
        foreach (var singleServiceItem in _navigationServiceItems)
            singleServiceItem.Instance = null;
    }

    public void SetFrame(Frame frame)
    {
        if (frame == null)
            return;

        _frame = frame;

        _frame.NavigationUIVisibility = NavigationUIVisibility.Hidden;

        _frame.Navigating -= OnFrameNavigating;
        _frame.Navigating += OnFrameNavigating;

        _frame.Navigated -= OnFrameNavigated;
        _frame.Navigated += OnFrameNavigated;
    }

    public void SetService(IPageService? pageService)
    {
        _pageService = pageService;
    }

    public IPageService? GetService()
    {
        return _pageService ?? null;
    }

    public string GetCurrentTag()
    {
        if (_currentPageIndex < 0)
            return "__external__";

        if (_navigationServiceItems.Length == 0)
            return string.Empty;

        if (_navigationServiceItems.Length - 1 < _currentPageIndex)
            return string.Empty;

        return _navigationServiceItems[_currentPageIndex].Tag;
    }

    public int GetCurrentId()
    {
        return _currentPageIndex;
    }

    public int GetPreviousId()
    {
        return _previousPageIndex;
    }

    #endregion Public methods

    #region Disposing

    public void Dispose()
    {
        Dispose(true);

        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        _disposed = true;

        if (!disposing)
            return;

#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO | {typeof(NavigationService)} disposed.", "Wpf.Ui.Navigation");
#endif

        _navigationServiceItems = null!;
    }

    #endregion Disposing

    #region Internal navigation

    private bool NavigateInternal(int serviceItemId, object? dataContext)
    {
        if (!_navigationServiceItems.Any())
            return false;

        _currentActionIdentifier = _eventIdentifier.GetNext();

        if (_navigationServiceItems.Length - 1 < serviceItemId)
            return false;

        if (_currentPageIndex == serviceItemId)
            return false;

        if (_navigationServiceItems[serviceItemId].Type == null &&
            _navigationServiceItems[serviceItemId].Source == null)
            return false;

        _previousPageIndex = _currentPageIndex;
        _currentPageIndex = serviceItemId;

        if (_pageService != null)
            return NavigateInternalByService(serviceItemId);

        if (!_navigationServiceItems[serviceItemId].Cache)
            return NavigateInternalByItemWithoutCache(serviceItemId, dataContext);

        return NavigateInternalByItemWithCache(serviceItemId, dataContext);
    }

    private bool NavigateInternalByItemWithCache(int serviceItemId, object? dataContext)
    {
        if (_frame == null)
            return false;

        if (_navigationServiceItems.Length - 1 < serviceItemId)
            return false;

        if (_navigationServiceItems[serviceItemId].Instance != null)
        {
            if (dataContext != null && _navigationServiceItems[serviceItemId].Instance is FrameworkElement)
                ((FrameworkElement)_navigationServiceItems[serviceItemId].Instance).DataContext = dataContext;

            _frame.Navigate(
                _navigationServiceItems[serviceItemId].Instance,
                new NavigationServiceExtraData
                {
                    PageId = serviceItemId,
                    Cache = true,
                    DataContext = dataContext
                });

#if DEBUG
            System.Diagnostics.Debug.WriteLine(
                $"DEBUG | {_navigationServiceItems[serviceItemId].Tag} navigated internally, with cache by it's instance.");
#endif
            AddToHistory(serviceItemId);
            return true;
        }

        if (_navigationServiceItems[serviceItemId].Type != null)
        {
            _navigationServiceItems[serviceItemId].Instance = CreateFrameworkElementInstance(_navigationServiceItems[serviceItemId].Type, dataContext);

            _frame.Navigate(
                _navigationServiceItems[serviceItemId].Instance,
                new NavigationServiceExtraData
                {
                    PageId = serviceItemId,
                    Cache = true,
                    DataContext = null
                });

#if DEBUG
            System.Diagnostics.Debug.WriteLine(
                $"DEBUG | {_navigationServiceItems[serviceItemId].Tag} navigated internally, with cache by it's type.");
#endif
            AddToHistory(serviceItemId);
            return true;
        }

        if (_navigationServiceItems[serviceItemId].Source != null)
        {
            _frame.Navigate(
                _navigationServiceItems[serviceItemId].Source,
                new NavigationServiceExtraData
                {
                    PageId = serviceItemId,
                    Cache = true,
                    DataContext = dataContext
                });

#if DEBUG
            System.Diagnostics.Debug.WriteLine(
                $"DEBUG | {_navigationServiceItems[serviceItemId].Tag} navigated internally, with cache by it's source.");
#endif

            AddToHistory(serviceItemId);
            return true;
        }

        return false;
    }

    private bool NavigateInternalByItemWithoutCache(int serviceItemId, object? dataContext)
    {
        if (_frame == null)
            return false;

        if (_navigationServiceItems.Length - 1 < serviceItemId)
            return false;

        if (_navigationServiceItems[serviceItemId].Type != null)
        {
            _frame.Navigate(
                CreateFrameworkElementInstance
                (
                    _navigationServiceItems[serviceItemId].Type,
                    dataContext
                ),
                new NavigationServiceExtraData
                {
                    PageId = serviceItemId,
                    Cache = false,
                    DataContext = null
                });
#if DEBUG
            System.Diagnostics.Debug.WriteLine(
                $"DEBUG | {_navigationServiceItems[serviceItemId].Tag} navigated internally, without cache by it's type.");
#endif
            AddToHistory(serviceItemId);
            return true;
        }

        if (_navigationServiceItems[serviceItemId].Source != null)
        {
            _frame.Navigate(
                _navigationServiceItems[serviceItemId].Source,
                new NavigationServiceExtraData
                {
                    PageId = serviceItemId,
                    Cache = false,
                    DataContext = dataContext
                });

#if DEBUG
            System.Diagnostics.Debug.WriteLine(
                $"DEBUG | {_navigationServiceItems[serviceItemId].Tag} navigated internally, without cache by it's source.");
#endif

            AddToHistory(serviceItemId);
            return true;
        }

        return false;
    }

    private bool NavigateInternalByService(int serviceItemId)
    {
        if (_frame == null)
            return false;

        if (_navigationServiceItems.Length - 1 < serviceItemId)
            return false;

        var servicePageInstance = _pageService?.GetPage(_navigationServiceItems[serviceItemId].Type);

        if (servicePageInstance == null)
            throw new InvalidOperationException($"The {_navigationServiceItems[serviceItemId].Type} has not been registered in the {typeof(IPageService)} service.");

        _frame.Navigate(servicePageInstance);
        AddToHistory(serviceItemId);

        return true;
    }

    private void AddToHistory(int serviceItemId)
    {
        if (_isBackNavigated)
        {
            _isBackNavigated = false;
            _history.RemoveAt(_history.LastIndexOf(_history[_history.Count - 2]));
            _history.RemoveAt(_history.LastIndexOf(_history[_history.Count - 1]));
        }

        _history.Add(serviceItemId);
    }

    #endregion Internal navigation

    #region Instance management

    private FrameworkElement CreateFrameworkElementInstance(Type pageType, object? dataContext)
    {
        return NavigationServiceActivator.CreateInstance(pageType, dataContext);
    }

    #endregion Instance management

    #region Frame events

    private void OnFrameNavigated(object sender, NavigationEventArgs e)
    {
        if (_frame == null)
            return;

        if (_frame.CanGoBack)
            _frame.RemoveBackEntry();

        if (_frame.NavigationService.CanGoBack)
            _frame.NavigationService?.RemoveBackEntry();

        if (TransitionDuration > 0 && e.Content != null)
            Transitions.ApplyTransition(e.Content, TransitionType, TransitionDuration);

        if (_pageService != null)
        {
            NotifyFrameContentAboutEnter();

            return;
        }

        if (e.ExtraData is not NavigationServiceExtraData extraData)
        {
            NotifyFrameContentAboutEnter();

            return;
        }

        if (!_currentActionIdentifier.Equals(_currentActionIdentifier))
        {
            NotifyFrameContentAboutEnter();

            return;
        }

        if (extraData.DataContext != null && _frame.Content is FrameworkElement)
        {
            ((FrameworkElement)_frame.Content).DataContext = extraData.DataContext;

            if (extraData.DataContext is IViewModel)
                ((IViewModel)extraData.DataContext).OnMounted((FrameworkElement)_frame.Content);
        }

        if (!extraData.Cache)
        {
            NotifyFrameContentAboutEnter();

            return;
        }

        if (_navigationServiceItems.Length - 1 < extraData.PageId || extraData.PageId < 0)
        {
            NotifyFrameContentAboutEnter();

            return;
        }

        if (_navigationServiceItems[extraData.PageId].Instance != null)
        {
            NotifyFrameContentAboutEnter();

            return;
        }

        _navigationServiceItems[extraData.PageId].Instance = _frame.Content;

        NotifyFrameContentAboutEnter();
    }

    private void OnFrameNavigating(object sender, NavigatingCancelEventArgs e)
    {
        if (_frame == null)
            return;

        NotifyFrameContentAboutLeave();

        switch (e.NavigationMode)
        {
            case NavigationMode.Back:
                e.Cancel = true;

                if (_currentPageIndex > 0)
                    Navigate(_currentPageIndex - 1, null);
                break;

            case NavigationMode.Forward:
                e.Cancel = true;

                if (_currentPageIndex < _navigationServiceItems.Length - 1)
                    Navigate(_currentPageIndex + 1, null);
                break;
        }
    }

    private void NotifyFrameContentAboutEnter()
    {
        if (_frame == null)
            return;

        if (_frame.Content is INavigationAware)
            ((INavigationAware)_frame.Content).OnNavigatedTo();

        if (_frame.Content is INavigableView<object> navigableView && navigableView.ViewModel is INavigationAware)
            ((INavigationAware)navigableView.ViewModel).OnNavigatedTo();

        if (_frame.Content is FrameworkElement && ((FrameworkElement)_frame.Content).DataContext is INavigationAware)
            ((INavigationAware)((FrameworkElement)_frame.Content).DataContext).OnNavigatedTo();
    }

    private void NotifyFrameContentAboutLeave()
    {
        if (_frame == null)
            return;

        if (_frame.Content is INavigationAware)
            ((INavigationAware)_frame.Content).OnNavigatedFrom();

        if (_frame.Content is INavigableView<object> navigableView && navigableView.ViewModel is INavigationAware)
            ((INavigationAware)navigableView.ViewModel).OnNavigatedFrom();

        if (_frame.Content is FrameworkElement && ((FrameworkElement)_frame.Content).DataContext is INavigationAware)
            ((INavigationAware)((FrameworkElement)_frame.Content).DataContext).OnNavigatedFrom();
    }

    #endregion Frame events

    #region Preache

    private void PrecacheItems()
    {
        if (DesignerHelper.IsInDesignMode)
            return;

        if (_pageService != null)
            return;
    }

    #endregion
}
