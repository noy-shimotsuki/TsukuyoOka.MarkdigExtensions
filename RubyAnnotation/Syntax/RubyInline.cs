using Markdig.Helpers;
using Markdig.Syntax.Inlines;

namespace TsukuyoOka.MarkdigExtensions.RubyAnnotation.Syntax;

/// <summary>
/// Ruby annotation
/// </summary>
public class RubyInline : ContainerInline
{
    public StringSlice Delimiter { get; init; }
    public bool HasRubyText { get; internal set; }
}