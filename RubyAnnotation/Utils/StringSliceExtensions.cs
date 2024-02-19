using Markdig.Helpers;

namespace TsukuyoOka.MarkdigExtensions.RubyAnnotation.Utils;

internal static class StringSliceExtensions
{
    public static int FindWithoutEscaped(this in StringSlice slice, char c, int start = -1)
    {
        return FindWithoutEscaped(slice, [c], start);
    }

    public static int FindWithoutEscaped(this StringSlice slice, ReadOnlySpan<char> chars, int start = -1)
    {
        var result = -1;
        if (start < 0)
        {
            start = slice.Start;
        }

        foreach (var c in chars)
        {
            slice.Start = start;
            while (slice.IndexOf(c) is >= 0 and var index)
            {
                slice.Start = index;
                if (!IsEscaped(slice))
                {
                    if (result < 0 || result > index)
                    {
                        result = index;
                    }
                    break;
                }
                slice.SkipChar();
            }
        }

        return result;
    }

    private static bool IsEscaped(this in StringSlice slice)
    {
        var peek = 0;
        while (true)
        {
            if (slice.PeekCharExtra(--peek) is not '\\')
            {
                return peek % 2 == 0;
            }
        }
    }
}