#nullable enable

using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Wpf.Ui.Common;

namespace Wpf.Ui.Controls;

public abstract class VirtualizingPanelBase : VirtualizingPanel, IScrollInfo
{
    #region Private properties

    private DependencyObject? _itemsOwner;

    private IRecyclingItemContainerGenerator? _itemContainerGenerator;

    private Visibility _previousVerticalScrollBarVisibility = Visibility.Collapsed;

    private Visibility _previousHorizontalScrollBarVisibility = Visibility.Collapsed;

    #endregion Private properties

    #region Protected properties

    protected override bool CanHierarchicallyScrollAndVirtualizeCore => true;

    protected ScrollUnit ScrollUnit => GetScrollUnit(ItemsControl);

    protected ScrollDirection MouseWheelScrollDirection { get; set; } = ScrollDirection.Vertical;

    protected bool IsVirtualizing => GetIsVirtualizing(ItemsControl);

    protected VirtualizationMode VirtualizationMode => GetVirtualizationMode(ItemsControl);

    protected bool IsRecycling => VirtualizationMode == VirtualizationMode.Recycling;

    protected VirtualizationCacheLength CacheLength { get; private set; }

    protected VirtualizationCacheLengthUnit CacheLengthUnit { get; private set; }

    protected ItemsControl ItemsControl => ItemsControl.GetItemsOwner(this);

    protected DependencyObject ItemsOwner
    {
        get
        {
            if (_itemsOwner is not null)
                return _itemsOwner;

            MethodInfo getItemsOwnerInternalMethod = typeof(ItemsControl).GetMethod(
                "GetItemsOwnerInternal",
                BindingFlags.Static | BindingFlags.NonPublic,
                null,
                new Type[] { typeof(DependencyObject) },
                null
            )!;

            _itemsOwner = (DependencyObject)getItemsOwnerInternalMethod.Invoke(null, new object[] { this })!;

            return _itemsOwner;
        }
    }

    protected ReadOnlyCollection<object> Items => ((ItemContainerGenerator)ItemContainerGenerator).Items;

    protected Point Offset { get; private set; } = new(0, 0);

    protected new IRecyclingItemContainerGenerator ItemContainerGenerator
    {
        get
        {
            if (_itemContainerGenerator is not null)
                return _itemContainerGenerator;

            var children = InternalChildren;
            _itemContainerGenerator = (IRecyclingItemContainerGenerator)base.ItemContainerGenerator;

            return _itemContainerGenerator;
        }
    }

    protected ItemRange ItemRange { get; set; }

    protected Size Extent { get; private set; } = new Size(0, 0);

    protected Size Viewport { get; private set; } = new Size(0, 0);

    #endregion Protected properties

    #region Public properties

    public static readonly DependencyProperty ScrollLineDeltaProperty =
        DependencyProperty.Register(nameof(ScrollLineDelta), typeof(double), typeof(VirtualizingPanelBase),
            new FrameworkPropertyMetadata(16.0));

    public static readonly DependencyProperty MouseWheelDeltaProperty =
        DependencyProperty.Register(nameof(MouseWheelDelta), typeof(double), typeof(VirtualizingPanelBase),
            new FrameworkPropertyMetadata(48.0));

    public static readonly DependencyProperty ScrollLineDeltaItemProperty =
        DependencyProperty.Register(nameof(ScrollLineDeltaItem), typeof(int), typeof(VirtualizingPanelBase),
            new FrameworkPropertyMetadata(1));

    public static readonly DependencyProperty MouseWheelDeltaItemProperty =
        DependencyProperty.Register(nameof(MouseWheelDeltaItem), typeof(int), typeof(VirtualizingPanelBase),
            new FrameworkPropertyMetadata(3));

    public ScrollViewer? ScrollOwner { get; set; }

    public bool CanVerticallyScroll { get; set; }

    public bool CanHorizontallyScroll { get; set; }

    public double ScrollLineDelta
    {
        get => (double)GetValue(ScrollLineDeltaProperty);
        set => SetValue(ScrollLineDeltaProperty, value);
    }

    public double MouseWheelDelta
    {
        get => (double)GetValue(MouseWheelDeltaProperty);
        set => SetValue(MouseWheelDeltaProperty, value);
    }

    public double ScrollLineDeltaItem
    {
        get => (int)GetValue(ScrollLineDeltaItemProperty);
        set => SetValue(ScrollLineDeltaItemProperty, value);
    }

    public int MouseWheelDeltaItem
    {
        get => (int)GetValue(MouseWheelDeltaItemProperty);
        set => SetValue(MouseWheelDeltaItemProperty, value);
    }

    public double ExtentWidth => Extent.Width;

    public double ExtentHeight => Extent.Height;

    public double HorizontalOffset => Offset.X;

    public double VerticalOffset => Offset.Y;

    public double ViewportWidth => Viewport.Width;

    public double ViewportHeight => Viewport.Height;

    #endregion Public properties

    #region Public methods

    public virtual Rect MakeVisible(Visual visual, Rect rectangle)
    {
        Point pos = visual.TransformToAncestor(this).Transform(Offset);

        var scrollAmountX = 0d;
        var scrollAmountY = 0d;

        if (pos.X < Offset.X)
        {
            scrollAmountX = -(Offset.X - pos.X);
        }
        else if ((pos.X + rectangle.Width) > (Offset.X + Viewport.Width))
        {
            var notVisibleX = (pos.X + rectangle.Width) - (Offset.X + Viewport.Width);
            var maxScrollX = pos.X - Offset.X;
            scrollAmountX = Math.Min(notVisibleX, maxScrollX);
        }

        if (pos.Y < Offset.Y)
        {
            scrollAmountY = -(Offset.Y - pos.Y);
        }
        else if ((pos.Y + rectangle.Height) > (Offset.Y + Viewport.Height))
        {
            var notVisibleY = (pos.Y + rectangle.Height) - (Offset.Y + Viewport.Height);
            var maxScrollY = pos.Y - Offset.Y;
            scrollAmountY = Math.Min(notVisibleY, maxScrollY);
        }

        SetHorizontalOffset(Offset.X + scrollAmountX);
        SetVerticalOffset(Offset.Y + scrollAmountY);

        var visibleRectWidth = Math.Min(rectangle.Width, Viewport.Width);
        var visibleRectHeight = Math.Min(rectangle.Height, Viewport.Height);

        return new Rect(scrollAmountX, scrollAmountY, visibleRectWidth, visibleRectHeight);
    }

    public void SetVerticalOffset(double offset)
    {
        if (offset < 0 || Viewport.Height >= Extent.Height)
            offset = 0;
        else if (offset + Viewport.Height >= Extent.Height)
            offset = Extent.Height - Viewport.Height;

        Offset = new Point(Offset.X, offset);
        ScrollOwner?.InvalidateScrollInfo();

        InvalidateMeasure();
    }

    public void SetHorizontalOffset(double offset)
    {
        if (offset < 0 || Viewport.Width >= Extent.Width)
            offset = 0;
        else if (offset + Viewport.Width >= Extent.Width)
            offset = Extent.Width - Viewport.Width;

        Offset = new Point(offset, Offset.Y);
        ScrollOwner?.InvalidateScrollInfo();
        InvalidateMeasure();
    }

    public void LineUp() =>
        ScrollVertical(ScrollUnit == ScrollUnit.Pixel ? -ScrollLineDelta : GetLineUpScrollAmount());

    public void LineDown() =>
        ScrollVertical(ScrollUnit == ScrollUnit.Pixel ? ScrollLineDelta : GetLineDownScrollAmount());

    public void LineLeft() =>
        ScrollHorizontal(ScrollUnit == ScrollUnit.Pixel ? -ScrollLineDelta : GetLineLeftScrollAmount());

    public void LineRight() =>
        ScrollHorizontal(ScrollUnit == ScrollUnit.Pixel ? ScrollLineDelta : GetLineRightScrollAmount());

    public void MouseWheelUp()
    {
        if (MouseWheelScrollDirection == ScrollDirection.Vertical)
            ScrollVertical(ScrollUnit == ScrollUnit.Pixel ? -MouseWheelDelta : GetMouseWheelUpScrollAmount());
        else
            MouseWheelLeft();
    }

    public void MouseWheelDown()
    {
        if (MouseWheelScrollDirection == ScrollDirection.Vertical)
            ScrollVertical(ScrollUnit == ScrollUnit.Pixel ? MouseWheelDelta : GetMouseWheelDownScrollAmount());
        else
            MouseWheelRight();
    }

    public void MouseWheelLeft() =>
        ScrollHorizontal(ScrollUnit == ScrollUnit.Pixel ? -MouseWheelDelta : GetMouseWheelLeftScrollAmount());

    public void MouseWheelRight() =>
        ScrollHorizontal(ScrollUnit == ScrollUnit.Pixel ? MouseWheelDelta : GetMouseWheelRightScrollAmount());

    public void PageUp() =>
        ScrollVertical(ScrollUnit == ScrollUnit.Pixel ? -ViewportHeight : GetPageUpScrollAmount());

    public void PageDown() =>
        ScrollVertical(ScrollUnit == ScrollUnit.Pixel ? ViewportHeight : GetPageDownScrollAmount());

    public void PageLeft() =>
        ScrollHorizontal(ScrollUnit == ScrollUnit.Pixel ? -ViewportHeight : GetPageLeftScrollAmount());

    public void PageRight() =>
        ScrollHorizontal(ScrollUnit == ScrollUnit.Pixel ? ViewportHeight : GetPageRightScrollAmount());

    #endregion Public methods

    #region Protected methods

    protected override void OnItemsChanged(object sender, ItemsChangedEventArgs args)
    {
        switch (args.Action)
        {
            case NotifyCollectionChangedAction.Remove:
            case NotifyCollectionChangedAction.Replace:
                RemoveInternalChildRange(args.Position.Index, args.ItemUICount);
                break;
            case NotifyCollectionChangedAction.Move:
                RemoveInternalChildRange(args.OldPosition.Index, args.ItemUICount);
                break;
        }
    }

    protected virtual void UpdateScrollInfo(Size availableSize, Size extent)
    {
        var invalidateScrollInfo = false;

        if (extent != Extent)
        {
            Extent = extent;
            invalidateScrollInfo = true;
        }

        if (availableSize != Viewport)
        {
            Viewport = availableSize;
            invalidateScrollInfo = true;
        }

        if (ViewportHeight != 0 && VerticalOffset != 0 && VerticalOffset + ViewportHeight + 1 >= ExtentHeight)
        {
            Offset = new Point(Offset.X, extent.Height - availableSize.Height);
            invalidateScrollInfo = true;
        }

        if (ViewportWidth != 0 && HorizontalOffset != 0 && HorizontalOffset + ViewportWidth + 1 >= ExtentWidth)
        {
            Offset = new Point(extent.Width - availableSize.Width, Offset.Y);
            invalidateScrollInfo = true;
        }

        if (invalidateScrollInfo)
            ScrollOwner?.InvalidateScrollInfo();
    }

    protected int GetItemIndexFromChildIndex(int childIndex)
    {
        var generatorPosition = GetGeneratorPositionFromChildIndex(childIndex);
        return ItemContainerGenerator.IndexFromGeneratorPosition(generatorPosition);
    }

    protected virtual GeneratorPosition GetGeneratorPositionFromChildIndex(int childIndex)
    {
        return new GeneratorPosition(childIndex, 0);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        if (ScrollOwner != null)
        {
            bool verticalScrollBarGotHidden = ScrollOwner.VerticalScrollBarVisibility == ScrollBarVisibility.Auto
                                              && ScrollOwner.ComputedVerticalScrollBarVisibility !=
                                              Visibility.Visible
                                              && ScrollOwner.ComputedVerticalScrollBarVisibility !=
                                              _previousVerticalScrollBarVisibility;

            bool horizontalScrollBarGotHidden =
                ScrollOwner.HorizontalScrollBarVisibility == ScrollBarVisibility.Auto
                && ScrollOwner.ComputedHorizontalScrollBarVisibility != Visibility.Visible
                && ScrollOwner.ComputedHorizontalScrollBarVisibility != _previousHorizontalScrollBarVisibility;

            _previousVerticalScrollBarVisibility = ScrollOwner.ComputedVerticalScrollBarVisibility;
            _previousHorizontalScrollBarVisibility = ScrollOwner.ComputedHorizontalScrollBarVisibility;

            if (!ScrollOwner.IsMeasureValid && verticalScrollBarGotHidden || horizontalScrollBarGotHidden)
                return availableSize;
        }

        var groupItem = ItemsOwner as IHierarchicalVirtualizationAndScrollInfo;

        Size extent;
        Size desiredSize;

        if (groupItem != null)
        {
            var viewportSize = groupItem.Constraints.Viewport.Size;
            var headerSize = groupItem.HeaderDesiredSizes.PixelSize;
            double availableWidth = Math.Max(viewportSize.Width - 5, 0);
            double availableHeight = Math.Max(viewportSize.Height - headerSize.Height, 0);
            availableSize = new Size(availableWidth, availableHeight);

            extent = CalculateExtent(availableSize);

            desiredSize = new Size(extent.Width, extent.Height);

            Extent = extent;
            Offset = groupItem.Constraints.Viewport.Location;
            Viewport = groupItem.Constraints.Viewport.Size;
            CacheLength = groupItem.Constraints.CacheLength;
            CacheLengthUnit = groupItem.Constraints.CacheLengthUnit;
        }
        else
        {
            extent = CalculateExtent(availableSize);
            double desiredWidth = Math.Min(availableSize.Width, extent.Width);
            double desiredHeight = Math.Min(availableSize.Height, extent.Height);
            desiredSize = new Size(desiredWidth, desiredHeight);

            UpdateScrollInfo(desiredSize, extent);
            CacheLength = GetCacheLength(ItemsOwner);
            CacheLengthUnit = GetCacheLengthUnit(ItemsOwner);
        }

        ItemRange = UpdateItemRange();

        RealizeItems();
        VirtualizeItems();

        return desiredSize;
    }

    protected virtual void RealizeItems()
    {
        var startPosition = ItemContainerGenerator.GeneratorPositionFromIndex(ItemRange.StartIndex);
        var childIndex = startPosition.Offset == 0 ? startPosition.Index : startPosition.Index + 1;

        using IDisposable at = ItemContainerGenerator.StartAt(startPosition, GeneratorDirection.Forward, true);

        for (int i = ItemRange.StartIndex; i <= ItemRange.EndIndex; i++, childIndex++)
        {
            var child = (UIElement)ItemContainerGenerator.GenerateNext(out bool isNewlyRealized);

            if (isNewlyRealized || !InternalChildren.Contains(child))
            {
                if (childIndex >= InternalChildren.Count)
                    AddInternalChild(child);
                else
                    InsertInternalChild(childIndex, child);

                ItemContainerGenerator.PrepareItemContainer(child);

                child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            }

            if (child is not IHierarchicalVirtualizationAndScrollInfo groupItem)
                continue;

            groupItem.Constraints = new HierarchicalVirtualizationConstraints(
                new VirtualizationCacheLength(0),
                VirtualizationCacheLengthUnit.Item,
                new Rect(0, 0, ViewportWidth, ViewportHeight));

            child.Measure(new Size(ViewportWidth, ViewportHeight));
        }
    }

    protected virtual void VirtualizeItems()
    {
        for (int childIndex = InternalChildren.Count - 1; childIndex >= 0; childIndex--)
        {
            var generatorPosition = GetGeneratorPositionFromChildIndex(childIndex);

            var itemIndex = ItemContainerGenerator.IndexFromGeneratorPosition(generatorPosition);

            if (itemIndex == -1 || ItemRange.Contains(itemIndex))
                continue;

            if (VirtualizationMode == VirtualizationMode.Recycling)
                ItemContainerGenerator.Recycle(generatorPosition, 1);
            else
                ItemContainerGenerator.Remove(generatorPosition, 1);

            RemoveInternalChildRange(childIndex, 1);
        }
    }

    protected void ScrollVertical(double amount)
    {
        SetVerticalOffset(VerticalOffset + amount);
    }

    protected void ScrollHorizontal(double amount)
    {
        SetHorizontalOffset(HorizontalOffset + amount);
    }

    #endregion Protected methods

    #region Protected abstract methods

    protected abstract Size CalculateExtent(Size availableSize);

    protected abstract ItemRange UpdateItemRange();

    protected abstract double GetLineUpScrollAmount();

    protected abstract double GetLineDownScrollAmount();

    protected abstract double GetLineLeftScrollAmount();

    protected abstract double GetLineRightScrollAmount();

    protected abstract double GetMouseWheelUpScrollAmount();

    protected abstract double GetMouseWheelDownScrollAmount();

    protected abstract double GetMouseWheelLeftScrollAmount();

    protected abstract double GetMouseWheelRightScrollAmount();

    protected abstract double GetPageUpScrollAmount();

    protected abstract double GetPageDownScrollAmount();

    protected abstract double GetPageLeftScrollAmount();

    protected abstract double GetPageRightScrollAmount();

    #endregion Protected abstract methods
}
