using System;
using System.Collections.Generic;

namespace ConsoleTablesPrinter
{
    /// <summary>
    /// Provides functionality to print structured tabular data to the console using customizable styles and attributes.
    /// </summary>
    /// <remarks>
    /// This is the main entry point for rendering console tables. 
    /// It supports a variety of table styles, formatting options, and custom column metadata via attributes.
    /// Use <c>PrintAsTable()</c> to render objects as tables with optional styling.
    ///
    /// If <see cref="DefaultStyle"/> is set and no style is passed to <c>PrintAsTable</c>, the default will be applied automatically.
    /// </remarks>
    public static partial class ConsoleTablePrinter
    {
        /// <summary>
        /// Gets or sets the default <see cref="TableStyle"/> to use when printing tables.
        /// If <c>PrintAsTable</c> is called without an explicit style, this style will be used instead.
        /// If not set, a built-in fallback style will be applied.
        /// </summary>
        public static TableStyle? DefaultStyle { get; set; }



        /// <summary>
        /// The ANSI escape sequence that represents the console's default background color.
        /// 
        /// This is only required if your application uses ANSI escape codes for color output
        /// (e.g., 256-color or 24-bit RGB modes). It allows the table printer to restore the original
        /// background color after rendering.
        ///        
        /// Example formats:
        /// - 256-color: "\u001b[48;5;234m"
        /// - RGB (true color): "\u001b[48;2;30;30;30m"
        /// </summary>
        /// <remarks>
        /// Do not set this if you're using standard <see cref="Console.BackgroundColor"/>. 
        /// </remarks>
        public static string? ConsoleAnsiBg { get; set; }

        /// <summary>
        /// The ANSI escape sequence that represents the console's default foreground color.
        /// 
        /// This is only required if your application uses ANSI escape codes for color output
        /// (e.g., 256-color or 24-bit RGB modes). It allows the table printer to restore the original
        /// foreground color after rendering.
        ///
        /// Example formats:
        /// - 256-color: "\u001b[38;5;250m"
        /// - RGB (true color): "\u001b[38;2;200;200;200m"
        /// </summary>
        /// <remarks>
        /// Do not set this if you're using standard <see cref="Console.ForegroundColor"/>.
        /// </remarks>
        public static string? ConsoleAnsiFg { get; set; }

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
}