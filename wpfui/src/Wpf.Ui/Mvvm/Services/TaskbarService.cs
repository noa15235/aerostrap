using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Interop;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.TaskBar;

namespace Wpf.Ui.Mvvm.Services;

public partial class TaskBarService : ITaskBarService
{
    private volatile Dictionary<IntPtr, TaskBarProgressState> _progressStates;

    public TaskBarService()
    {
        _progressStates = new Dictionary<IntPtr, TaskBarProgressState>();
    }

    public virtual TaskBarProgressState GetState(IntPtr hWnd)
    {
        if (!_progressStates.TryGetValue(hWnd, out var progressState))
            return TaskBarProgressState.None;

        return progressState;
    }

    public virtual TaskBarProgressState GetState(Window window)
    {
        if (window == null)
            return TaskBarProgressState.None;

        var windowHandle = new WindowInteropHelper(window).Handle;

        if (!_progressStates.TryGetValue(windowHandle, out var progressState))
            return TaskBarProgressState.None;

        return progressState;
    }

    public virtual bool SetState(Window window, TaskBarProgressState taskBarProgressState)
    {
        if (window == null)
            return false;

        return TaskBarProgress.SetState(window, taskBarProgressState);
    }

    public virtual bool SetValue(Window window, TaskBarProgressState taskBarProgressState, int current, int total)
    {
        if (window == null)
            return false;

        return TaskBarProgress.SetValue(window, taskBarProgressState, current, total);
    }

    public virtual bool SetValue(Window window, int current, int total)
    {
        if (window == null)
            return false;

        var windowHandle = new WindowInteropHelper(window).Handle;

        if (!_progressStates.TryGetValue(windowHandle, out var progressState))
            return TaskBarProgress.SetValue(window, TaskBarProgressState.Normal, current, total);

        return TaskBarProgress.SetValue(window, progressState, current, total);
    }

    public virtual bool SetState(IntPtr hWnd, TaskBarProgressState taskBarProgressState)
    {
        return TaskBarProgress.SetState(hWnd, taskBarProgressState);
    }

    public virtual bool SetValue(IntPtr hWnd, TaskBarProgressState taskBarProgressState, int current, int total)
    {
        return TaskBarProgress.SetValue(hWnd, taskBarProgressState, current, total);
    }

    public virtual bool SetValue(IntPtr hWnd, int current, int total)
    {
        if (!_progressStates.TryGetValue(hWnd, out var progressState))
            return TaskBarProgress.SetValue(hWnd, TaskBarProgressState.Normal, current, total);

        return TaskBarProgress.SetValue(hWnd, progressState, current, total);
    }
}
