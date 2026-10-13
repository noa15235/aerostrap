using System;
using System.Threading;

namespace Wpf.Ui.Common;

public static class Clipboard
{
    public static void SetText(string text)
    {
        try
        {
            Thread thread = new(() => System.Windows.Clipboard.SetText(text));

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}

