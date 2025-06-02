namespace TablePrinter.Models;

/// <summary>
/// Defines styling options for a table cell, including foreground and background colors,
/// and text alignment. These settings are used unless overridden by attributes applied
/// directly to the property representing the table column using <see cref="TablePrintColAttribute"/>.
/// </summary>
public class CellStyle
{
    /// <summary>
    /// Gets or sets the foreground (text) color of the cell.
    /// If <c>null</c>, the current console foreground color is used,
    /// unless overridden by an attribute on the property using <see cref="TablePrintColAttribute"/>.
    /// </summary>
    public ConsoleColor? ForegroundColor { get; set; }

    /// <summary>
    /// Gets or sets the background color of the cell.
    /// If <c>null</c>, the current console background color is used,
    /// unless overridden by an attribute on the property using <see cref="TablePrintColAttribute"/>.
    /// </summary>
    public ConsoleColor? BackgroundColor { get; set; }

    /// <summary>
    /// Gets or sets the text alignment within the cell.
    /// If <c>null</c>, the default console text alignment is used,
    /// unless overridden by an attribute on the property using <see cref="TablePrintColAttribute"/>.
    /// </summary>
    public TextAlignments? TextAlignment { get; set; }
}