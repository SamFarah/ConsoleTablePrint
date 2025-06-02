namespace TablePrinter.Attributes;

/// <summary>
/// Specifies how a property should be displayed in the console table.
/// Use this attribute to customize the header, alignment, width, and formatting of the column.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class TablePrintColAttribute : Attribute
{
    /// <summary>
    /// Gets or sets the header text to display for the column.
    /// If not specified, the property name will be used as the header.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the format string used to format the column's values.
    /// This supports standard .NET format strings, e.g. "C2" for currency with two decimals.    
    /// </summary>
    public string? Format { get; set; }

    /// <summary>
    /// Indicate whether this column should be hidden from the output.
    /// If set to <c>true</c>, the property will not be displayed in the rendered table.
    /// </summary>
    public bool Hidden { get; set; }

    /// <summary>
    /// The background color of the column header.
    /// Set to <c>(ConsoleColor)(-1)</c> to use the default.
    /// </summary>
    public ConsoleColor HeaderBgColor { get; set; } = (ConsoleColor)(-1);

    /// <summary>
    /// The text color of the column header.
    /// Set to <c>(ConsoleColor)(-1)</c> to use the default.
    /// </summary>
    public ConsoleColor HeaderTextColor { get; set; } = (ConsoleColor)(-1);

    /// <summary>
    /// The text alignment of the column header.
    /// Set to <c>(TextAlignment)(-1)</c> to use the default.
    /// </summary>
    public TextAlignments HeaderTextAlignment { get; set; } = (TextAlignments)(-1);


    /// <summary>
    /// The background color of the cell content in this column.
    /// Set to <c>(ConsoleColor)(-1)</c> to use the default.
    /// </summary>
    public ConsoleColor CellBgColor { get; set; } = (ConsoleColor)(-1);

    /// <summary>
    /// The text color of the cell content in this column.
    /// Set to <c>(ConsoleColor)(-1)</c> to use the default.
    /// </summary>
    public ConsoleColor CellTextColor { get; set; } = (ConsoleColor)(-1);

    /// <summary>
    /// The text alignment of the cell content in this column.
    /// Set to <c>(TextAlignment)(-1)</c> to use the default.
    /// </summary>
    public TextAlignments CellTextAlignment { get; set; } = (TextAlignments)(-1);


    /// <summary>
    /// Initializes a new instance of the <see cref="TablePrintColAttribute"/> class with a display name.
    /// </summary>
    /// <param name="displayName">The name to be used as the column header instead of the property name.</param>
    public TablePrintColAttribute(string displayName)
    {
        DisplayName = displayName;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TablePrintColAttribute"/> class.
    /// </summary>
    public TablePrintColAttribute() { }
}