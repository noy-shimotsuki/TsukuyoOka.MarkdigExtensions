namespace TsukuyoOka.MarkdigExtensions.RubyAnnotation;

/// <summary>
/// Ruby options.
/// </summary>
public class RubyOptions
{
    /// <summary>
    /// Default options
    /// </summary>
    public static readonly RubyOptions Default = new RubyOptions();

    /// <summary>
    /// Whether to render ruby text on plain text
    /// </summary>
    public bool EnableRubyRenderToPlainText { get; set; }

    /// <summary>
    /// The opening parenthesis used to render ruby text
    /// </summary>
    public string OpenParenthesisForRendering { get; set; } = "(";

    /// <summary>
    /// The closing parenthesis used to render ruby text
    /// </summary>
    public string CloseParenthesisForRendering { get; set; } = ")";
}
