using Markdig.Helpers;
using Markdig.Syntax.Inlines;
using System.Diagnostics;

namespace TsukuyoOka.MarkdigExtensions.RubyAnnotation.Syntax;

/// <summary>
/// Ruby annotation text
/// </summary>
[DebuggerDisplay("Content: {Content}")]
public class RubyTextInline : LeafInline
{
    public StringSlice Content { get; set; }
}