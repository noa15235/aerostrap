using System;
using Wpf.Ui.Extensions;

namespace Wpf.Ui.Common;

internal class EventIdentifier
{
    public long Current { get; internal set; } = 0;

    public long GetNext()
    {
        UpdateIdentifier();

        return Current;
    }

    public bool IsEqual(long storedId) => Current == storedId;

    private void UpdateIdentifier() => Current = DateTime.Now.GetMicroTimestamp();
}
