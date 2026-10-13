namespace Wpf.Ui.Services.Internal;

internal struct NavigationServiceExtraData
{
    public int PageId { get; set; }

    public bool Cache { get; set; }

    public object DataContext { get; set; }
}
