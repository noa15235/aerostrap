using System;
using System.Windows;
using Wpf.Ui.TaskBar;

namespace Wpf.Ui.Mvvm.Contracts;

public interface ITaskBarService
{
    TaskBarProgressState GetState(IntPtr hWnd);

    TaskBarProgressState GetState(Window window);

    bool SetState(IntPtr hWnd, TaskBarProgressState taskBarProgressState);

    bool SetValue(IntPtr hWnd, TaskBarProgressState taskBarProgressState, int current, int total);

    bool SetValue(IntPtr hWnd, int current, int max);

    bool SetState(Window window, TaskBarProgressState taskBarProgressState);

    bool SetValue(Window window, TaskBarProgressState taskBarProgressState, int current, int total);

    bool SetValue(Window window, int current, int total);
}

