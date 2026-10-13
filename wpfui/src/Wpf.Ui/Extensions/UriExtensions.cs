using System;
using System.Linq;

namespace Wpf.Ui.Extensions;

public static class UriExtensions
{
    public static Uri TrimLastSegment(this Uri uri)
    {
        if (uri.Segments.Length < 2)
            return uri;

#if NET5_0_OR_GREATER
        var uriLastSegmentLength = uri.Segments[^1].Length;
#else
        var uriLastSegmentLength = uri.Segments[uri.Segments.Length - 1].Length;
#endif
        var uriOriginalString = uri.ToString();

        return new Uri(uriOriginalString.Substring(0, uriOriginalString.Length - uriLastSegmentLength), UriKind.RelativeOrAbsolute);
    }

    public static bool EndsWith(this Uri uri, string value)
    {
        var uriString = uri.ToString();

        return uri.ToString().EndsWith(value);
    }

    public static Uri Append(this Uri uri, params string[] segments)
    {
        if (!uri.IsAbsoluteUri)
            return uri;

        return new Uri(segments.Aggregate(uri.AbsoluteUri, (current, path) => String.Format("{0}/{1}", current.TrimEnd('/').TrimEnd('\\'), path.TrimStart('/').TrimStart('\\'))));
    }

    public static Uri Append(this Uri uri, Uri value)
    {
        return new Uri(String.Format("{0}/{1}", uri.ToString().TrimEnd('/').TrimEnd('\\'), value.ToString().TrimStart('/').TrimStart('\\')), UriKind.RelativeOrAbsolute);
    }
}

