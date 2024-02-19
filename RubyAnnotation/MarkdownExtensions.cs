namespace TsukuyoOka.MarkdigExtensions.RubyAnnotation;

public static class MarkdownExtensions
{
    public static MarkdownPipelineBuilder UseRuby(this MarkdownPipelineBuilder pipeline, RubyOptions? options = null)
    {
        pipeline.Extensions.AddIfNotAlready(new RubyMarkdownExtension(options));
        return pipeline;
    }
}