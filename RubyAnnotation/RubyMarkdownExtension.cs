using Markdig.Renderers;
using TsukuyoOka.MarkdigExtensions.RubyAnnotation.Parsers;
using TsukuyoOka.MarkdigExtensions.RubyAnnotation.Renderers;

namespace TsukuyoOka.MarkdigExtensions.RubyAnnotation;

public class RubyMarkdownExtension(RubyOptions? options = null) : IMarkdownExtension
{
    public RubyOptions Options { get; } = options ?? RubyOptions.Default;

    public void Setup(MarkdownPipelineBuilder pipeline)
    {
        if (!pipeline.InlineParsers.Contains<RubyParser>())
        {
            pipeline.InlineParsers.Insert(0, new RubyParser());
        }
    }

    public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
    {
        if (renderer is HtmlRenderer htmlRenderer)
        {
            htmlRenderer.ObjectRenderers.AddIfNotAlready(new HtmlRubyRenderer());
            htmlRenderer.ObjectRenderers.AddIfNotAlready(new HtmlRubyTextRenderer(Options));
        }
    }
}