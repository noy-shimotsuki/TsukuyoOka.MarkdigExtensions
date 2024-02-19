using Markdig.Helpers;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using TsukuyoOka.MarkdigExtensions.RubyAnnotation.Syntax;

namespace TsukuyoOka.MarkdigExtensions.RubyAnnotation.Renderers;

/// <summary>
/// A HTML renderer for <see cref="RubyTextInline"/>.
/// </summary>
/// <seealso cref="HtmlObjectRenderer{RubyTextInline}" />
public class HtmlRubyTextRenderer(RubyOptions options) : HtmlObjectRenderer<RubyTextInline>
{
    private readonly RubyOptions _options = options;

    protected override void Write(HtmlRenderer renderer, RubyTextInline rubyText)
    {
        if (renderer.EnableHtmlForInline)
        {
            renderer.Write($"<rp>{_options.OpenParenthesisForRendering}</rp><rt>");
            var rt = rubyText.Content;
            for (var index = rt.IndexOf('\\'); index >= 0; index = rt.IndexOf('\\'))
            {
                renderer.WriteEscape(new StringSlice(rt.Text, rt.Start, index - 1));
                rt.Start = index + 1;
            }
            renderer.WriteEscape(rt);
            renderer.Write($"</rt><rp>{_options.CloseParenthesisForRendering}</rp>");
        }
        else if (_options.EnableRubyRenderToPlainText)
        {
            renderer.Write(_options.OpenParenthesisForRendering);
            var rt = rubyText.Content;
            for (var index = rt.IndexOf('\\'); index >= 0; index = rt.IndexOf('\\'))
            {
                renderer.WriteEscape(new StringSlice(rt.Text, rt.Start, index - 1));
                rt.Start = index + 1;
            }
            renderer.WriteEscape(rt);
            renderer.Write(_options.CloseParenthesisForRendering);
        }
    }
}