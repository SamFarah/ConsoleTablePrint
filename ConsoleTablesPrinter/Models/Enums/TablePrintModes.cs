
using ConsoleTablesPrinter;

/// <summary>
/// Specifies the rendering mode for console table output.
/// </summary>
public enum TablePrintModes
{
    /// <summary>
    /// Default rendering mode.
    /// Applies all styling, colors, alignment, padding, and custom attributes.
    /// This mode outputs a richly formatted console table using the specified <see cref="TableStyle"/>.
    /// </summary>
    Normal,

    /// <summary>
    /// Markdown rendering mode.
    /// Ignores all styling defined in <see cref="TableStyle"/>, including borders, padding,
    /// foreground/background colors, and text alignment. 
    /// Outputs a plain Markdown-compatible table suitable for use in GitHub, documentation,
    /// or other plain-text environments.
    /// 
    /// Attribute-based customizations (via <see cref="TablePrintColAttribute"/>) are respected 
    /// for display names and value formatting, but not for style-related settings.
    /// </summary>
    Markdown
}