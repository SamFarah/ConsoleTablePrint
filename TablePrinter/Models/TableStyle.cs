namespace TablePrinter.Models;

public class TableStyle
{
    /// <summary>
    /// Specifies the style of the table border. If <c>null</c>, <see cref="BorderStyles.SingleLine"/> is used.    
    /// </summary>
    public BorderStyles? BorderStyle { get; set; }

    /// <summary>
    /// Specifies the color of the table border. If <c>null</c>, the default console color is used.    
    /// </summary>
    public ConsoleColor? BorderColor { get; set; }


    /// <summary>
    /// Specifies the background color for the entire table. If <c>null</c>, no background color is applied.
    /// </summary>
    public ConsoleColor? BackgroundColor { get; set; }

    /// <summary>
    /// Number of spaces to pad inside each cell. Affects only vertical padding.
    /// </summary>
    public int? CellHorizontalPadding { get; set; }


    /// <summary>
    /// Number of horizontal spaces between the table and the left edge of the console.
    /// </summary>
    public int HorizontalPadding { get; set; }

    /// <summary>
    /// Number of vertical lines between the table and the top/bottom edge of the console.
    /// </summary>
    public int VerticalPadding { get; set; }

    /// <summary>
    /// Default style settings applied to header cells unless overridden by a column's <see cref="TablePrintColAttribute"/>.
    /// </summary>
    public CellStyle? HeaderCellStyle { get; set; }

    /// <summary>
    /// Default style settings applied to data cells unless overridden by a column's <see cref="TablePrintColAttribute"/>.
    /// </summary>
    public CellStyle? DataCellStyle { get; set; }

    /// <summary>
    /// If <c>true</c>, inserts a visual row separator between each table row.
    /// </summary>
    public bool UseRowSeperator { get; set; }

    /// <summary>
    /// Specifies the border style to use for row separators. Only used if <see cref="UseRowSeperator"/> is <c>true</c>.
    /// If not set, the value from <see cref="BorderStyle"/> will be used.
    /// <example>
    /// For example: It looks better to use <see cref="BorderStyles.SingleLine"/> for the row seperator when the table border style is <see cref="BorderStyles.SingleToDoubleLine"/>
    /// </example>
    /// </summary>
    public BorderStyles? RowSeperatorStyle { get; set; }


    /// <summary>
    /// If <c>true</c>, table rows will be printed one at a time with a delay between each, creating an animation effect.
    /// </summary>
    public bool UseAnimation { get; set; }

    /// <summary>
    /// Delay in milliseconds between each row when <see cref="UseAnimation"/> is enabled.
    /// Defaults to <c>100</c>ms. Value is clamped between <c>0</c>ms and <c>200</c>ms.
    /// </summary>
    public int AnimationDelay { get; set; } = 100;
}
