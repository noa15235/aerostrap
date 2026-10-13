#nullable enable

using System;
using System.Windows;
using Wpf.Ui.Controls.Interfaces;

namespace Wpf.Ui.Mvvm.Contracts;

public interface IPageService
{
    public T? GetPage<T>() where T : class;

    public FrameworkElement? GetPage(Type pageType);
}
