using System.Reflection;
using TablePrinter.Helpers;

namespace TablePrinter;
public static partial class ConsoleTablePrinter
{
    private static void PrintListAsTable<T>(this List<T> items, TableStyle? tableStyle = null)
    {
        tableStyle ??= DefaultStyle;

        var properties = GetVisibleProperties<T>();

        var headers = properties.Select(p =>
        {
            var attr = p.GetCustomAttribute<TablePrintColAttribute>();
            return new TableHeader
            {
                PropertyName = p.Name,
                DisplayName = attr?.DisplayName ?? p.Name,
                HeaderStyle = GetHeaderStyle(attr)
            };
        }).ToList();

        var rows = items.Select(item =>
            properties.Select(p =>
            {
                var attr = p.GetCustomAttribute<TablePrintColAttribute>();
                var val = p.GetValue(item);

                return new TableCell
                {
                    Value = FormatValue(val, attr),
                    CellStyle = GetCellStyle(attr)
                };
            }).ToList()
        ).ToList();

        var requiresUtf8 = tableStyle != null && (_specialEncodingStyles.Contains(tableStyle.BorderStyle.GetValueOrDefault()) ||
                                                  _specialEncodingStyles.Contains(tableStyle.RowSeperatorStyle.GetValueOrDefault()));

        ConsoleStateManager.WithConsoleState(() => PrintTable(headers, rows, tableStyle), requiresUtf8);
    }

    private static void PrintObjectAsTable<T>(this T item, TableStyle? tableStyle = null)
    {
        tableStyle ??= DefaultStyle;

        var properties = GetVisibleProperties<T>();

        var headers = new List<TableHeader>
        {
            new() { PropertyName = "Property", DisplayName = "Property" },
            new() { PropertyName = "Value", DisplayName = "Value" }
        };

        var rows = properties.Select(p =>
        {
            var attr = p.GetCustomAttribute<TablePrintColAttribute>();
            var val = p.GetValue(item);

            return new List<TableCell>
            {
                new() { Value = attr?.DisplayName ?? p.Name, CellStyle = GetHeaderStyle(attr) },
                new() { Value = FormatValue(val, attr), CellStyle = GetCellStyle(attr) }
            };
        }).ToList();

        var requiresUtf8 = tableStyle != null && (_specialEncodingStyles.Contains(tableStyle.BorderStyle.GetValueOrDefault()) ||
                                                  _specialEncodingStyles.Contains(tableStyle.RowSeperatorStyle.GetValueOrDefault()));

        ConsoleStateManager.WithConsoleState(() => PrintTable(headers, rows, tableStyle), requiresUtf8);
    }

    private static void PrintTable(List<TableHeader> headers, List<List<TableCell>> rows, TableStyle? tableStyle = null)
    {
        if (headers.Count == 0) return;
        if (rows.Count == 0) return;
        tableStyle ??= DefaultStyle;

        var borderStyle = tableStyle?.BorderStyle ?? BorderStyles.SingleLine;
        var animationDelay = tableStyle != null ? Math.Clamp(tableStyle.AnimationDelay, 0, 200) : 0;
        ConsoleColor? borderColor = tableStyle?.BorderColor;
        ConsoleColor? borderBgColor = tableStyle?.BackgroundColor;
        var colWidths = Enumerable.Range(0, headers.Count)
            .Select(i => Math.Max(
                headers[i].DisplayName.Trim().Length, (rows.Count == 0 ? 0 :
                rows.Max(row => row[i].Value?.Trim().Length ?? 0))
            )).ToList();

        var xCellPadding = Math.Max(tableStyle?.CellHorizontalPadding ?? 1, 0);

        var colCount = headers.Count;
        var rowCount = rows.Count;

        // create horozental Line Template, we need to calcualte this template once, and we can use it for every horozental line
        var hLineTemplate = "".PadLeft(tableStyle?.HorizontalPadding ?? 0, ' ');
        hLineTemplate += "0";
        for (var i = 0; i < colCount; i++)
        {
            var colWidth = colWidths[i];
            hLineTemplate += "".PadLeft(colWidth + (2 * xCellPadding), '1');
            if (i < colCount - 1) hLineTemplate += "2";
        }
        hLineTemplate += "3";

        PrintVerticalPadding(tableStyle?.VerticalPadding);
        PrintHorizentalLine(hLineTemplate, borderStyle, HorLineDefs.TopLine, borderColor, borderBgColor);

        // print header
        PrintHorizontalPadding(tableStyle?.HorizontalPadding);
        for (var i = 0; i < headers.Count; i++)
        {
            PrintBorder(borderStyle, PiecePos.Vertical, borderColor, borderBgColor);
            PrintCell(headers[i].DisplayName, colWidths[i], xCellPadding, tableStyle?.HeaderCellStyle, headers[i].HeaderStyle, tableStyle?.BackgroundColor);
        }
        PrintBorder(borderStyle, PiecePos.Vertical, borderColor, borderBgColor, appendNewline: true);
        PrintHorizentalLine(hLineTemplate, borderStyle, HorLineDefs.UnderHeaderLine, borderColor, borderBgColor);

        // print rows
        for (var i = 0; i < rowCount; i++)
        {
            if (tableStyle?.UseAnimation ?? false) Thread.Sleep(animationDelay);
            PrintHorizontalPadding(tableStyle?.HorizontalPadding);
            for (var j = 0; j < colCount; j++)
            {
                if (tableStyle?.UseAnimation ?? false) Thread.Sleep(animationDelay);
                PrintBorder(borderStyle, PiecePos.Vertical, borderColor, borderBgColor);

                var cell = rows[i][j];
                PrintCell(cell.Value?.Trim() ?? string.Empty, colWidths[j], xCellPadding, tableStyle?.DataCellStyle, cell.CellStyle, tableStyle?.BackgroundColor);
            }
            PrintBorder(borderStyle, PiecePos.Vertical, borderColor, borderBgColor, appendNewline: true);

            if (i < rowCount - 1 && (tableStyle?.UseRowSeperator ?? false))
            {
                var rowSeperatorStyle = tableStyle?.RowSeperatorStyle ?? borderStyle;

                // print row seperator 
                PrintHorizentalLine(hLineTemplate, rowSeperatorStyle, HorLineDefs.RowSeperator, borderColor, borderBgColor);
            }
        }

        // print bottom line
        PrintHorizentalLine(hLineTemplate, borderStyle, HorLineDefs.BottomLine, borderColor, borderBgColor);
        PrintVerticalPadding(tableStyle?.VerticalPadding);
    }

    private static void PrintVerticalPadding(int? padding) => Console.Write("".PadRight((padding ?? 0), '\n'));
    private static void PrintHorizontalPadding(int? padding) => Console.Write("".PadLeft((padding ?? 0), ' '));

    private static void PrintBorder(BorderStyles style, PiecePos pos, ConsoleColor? color = null, ConsoleColor? bgColor = null, bool appendNewline = false)
    {
        var originalBg = Console.BackgroundColor;
        var originalFg = Console.ForegroundColor;

        if (color != null) Console.ForegroundColor = color.Value;
        if (bgColor != null) Console.BackgroundColor = bgColor.Value;
        Console.Write(_lines[style][(int)pos] + (appendNewline ? "\n" : ""));

        Console.BackgroundColor = originalBg;
        Console.ForegroundColor = originalFg;
    }

    private static void PrintHorizentalLine(string lineTemplate, BorderStyles style, HorLineDefs lineDef, ConsoleColor? color = null, ConsoleColor? bgColor = null)
    {
        var originalBg = Console.BackgroundColor;
        var originalFg = Console.ForegroundColor;

        if (color != null) Console.ForegroundColor = color.Value;
        if (bgColor != null) Console.BackgroundColor = bgColor.Value;

        var linePieces = _lineDefinitions[lineDef];
        var line = lineTemplate;
        for (int i = 0; i < linePieces.Length; i++)
        {
            line = line.Replace((char)(i + '0'), _lines[style][(int)linePieces[i]]);
        }
        Console.WriteLine(line);

        Console.BackgroundColor = originalBg;
        Console.ForegroundColor = originalFg;
    }

    private static void PrintCell(string cell, int colWidth, int xPadding, CellStyle? tableCellStyle, CellStyle overrideStyle, ConsoleColor? borderBgColor)
    {
        var originalBg = Console.BackgroundColor;
        var originalFg = Console.ForegroundColor;
        var cellWidth = colWidth + (2 * xPadding);


        int leftPadding = xPadding;
        int rightPadding = xPadding;

        switch (overrideStyle.TextAlignment ?? tableCellStyle?.TextAlignment ?? TextAlignments.Left)
        {
            case TextAlignments.Left:
                rightPadding = cellWidth - cell.Length - xPadding;
                break;
            case TextAlignments.Center:
                leftPadding = (int)Math.Ceiling((cellWidth - cell.Length) / 2.0);
                rightPadding = cellWidth - cell.Length - leftPadding;
                break;
            case TextAlignments.Right:
                leftPadding = cellWidth - cell.Length - xPadding;
                break;
        }
        if (overrideStyle.BackgroundColor != null) Console.BackgroundColor = overrideStyle.BackgroundColor.Value;
        else if (tableCellStyle?.BackgroundColor != null) Console.BackgroundColor = tableCellStyle.BackgroundColor.Value;
        else if (borderBgColor != null) Console.BackgroundColor = borderBgColor.Value;

        if (overrideStyle.ForegroundColor != null) Console.ForegroundColor = overrideStyle.ForegroundColor.Value;
        else if (tableCellStyle?.ForegroundColor != null) Console.ForegroundColor = tableCellStyle.ForegroundColor.Value;


        Console.Write("".PadLeft(leftPadding, ' '));
        Console.Write(cell);
        Console.Write("".PadRight(rightPadding, ' '));

        Console.BackgroundColor = originalBg;
        Console.ForegroundColor = originalFg;
    }
}
