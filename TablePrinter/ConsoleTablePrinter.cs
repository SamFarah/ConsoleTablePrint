global using TablePrinter.Attributes;
global using TablePrinter.Models;
global using TablePrinter.Models.Enums;

namespace TablePrinter;

public static partial class ConsoleTablePrinter
{
    /// <summary>
    /// Gets or sets the default <see cref="TableStyle"/> to use when printing tables.
    /// If <c>PrintAsTable</c> is called without an explicit style, this style will be used instead.
    /// If not set, a built-in fallback style will be applied.
    /// </summary>
    public static TableStyle? DefaultStyle { get; set; }

    /// <summary>
    /// Prints the given object as a formatted table to the console,
    /// applying the specified table configuration.
    /// </summary>
    /// <typeparam name="T">The type of the object to print.</typeparam>
    /// <param name="item">The object to print as a table.</param>
    /// <param name="tableConfig">An action to configure the table style before printing.</param>
    public static void PrintAsTable<T>(this T item, Action<TableStyle> tableConfig)
    {
        var style = new TableStyle();
        tableConfig?.Invoke(style);
        PrintObjectAsTable(item, style);
    }

    /// <summary>
    /// Prints the given object as a formatted table to the console,
    /// using an optional <see cref="TableStyle"/> for styling.
    /// </summary>
    /// <typeparam name="T">The type of the object to print.</typeparam>
    /// <param name="item">The object to print as a table.</param>
    /// <param name="tableStyle">An optional table style to apply.</param>
    public static void PrintAsTable<T>(this T item, TableStyle? tableStyle = null) => PrintObjectAsTable(item, tableStyle);

    /// <summary>
    /// Prints the given list of items as a formatted table to the console,
    /// applying the specified table configuration.
    /// </summary>
    /// <typeparam name="T">The type of the items in the list.</typeparam>
    /// <param name="items">The list of items to print as a table.</param>
    /// <param name="tableConfig">An action to configure the table style before printing.</param>
    public static void PrintAsTable<T>(this List<T> items, Action<TableStyle> tableConfig)
    {
        var style = new TableStyle();
        tableConfig?.Invoke(style);
        PrintListAsTable(items, style);
    }

    /// <summary>
    /// Prints the given list of items as a formatted table to the console,
    /// using an optional <see cref="TableStyle"/> for styling.
    /// </summary>
    /// <typeparam name="T">The type of the items in the list.</typeparam>
    /// <param name="items">The list of items to print as a table.</param>
    /// <param name="tableStyle">An optional table style to apply.</param>    
    public static void PrintAsTable<T>(this List<T> items, TableStyle? tableStyle = null) => PrintListAsTable(items, tableStyle);








}