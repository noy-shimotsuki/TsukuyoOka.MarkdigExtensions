using Markdig.Renderers;
using Markdig.Renderers.Html;
using TsukuyoOka.MarkdigExtensions.RubyAnnotation.Syntax;

namespace TsukuyoOka.MarkdigExtensions.RubyAnnotation.Renderers;

/// <summary>
/// A HTML renderer for <see cref="RubyInline"/>.
/// </summary>
/// <seealso cref="HtmlObjectRenderer{RubyInline}" />
public class HtmlRubyRenderer : HtmlObjectRenderer<RubyInline>
{
    protected override void Write(HtmlRenderer renderer, RubyInline ruby)
    {
        if (!ruby.HasRubyText)
        {
            renderer.Write(ruby.Delimiter);
        }

        if (ruby.HasRubyText && renderer.EnableHtmlForInline)
        {
            renderer.Write("<ruby>");
        }

        renderer.WriteChildren(ruby);

        if (ruby.HasRubyText && renderer.EnableHtmlForInline)
        {
            renderer.Write("</ruby>");
        }
    }
}