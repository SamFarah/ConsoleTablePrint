using System.Reflection;
using System.Text;

namespace TablePrinter;
public static class ConsoleTable
{

    [AttributeUsage(AttributeTargets.Property)]
    public class ConsoleTableAttribute : Attribute
    {
        public string? DisplayName { get; set; }
        public string? Format { get; set; }
        public bool Hidden { get; set; }

        public CellStyle? HeaderStyle { get; set; }

        public ConsoleColor HeaderBgColour { get; set; } = (ConsoleColor)(-1);
        public ConsoleColor HeaderFgColour { get; set; } = (ConsoleColor)(-1);
        public TextAlignment HeaderTextAlignment { get; set; } = (TextAlignment)(-1);

        public ConsoleColor CellBgColour { get; set; } = (ConsoleColor)(-1);
        public ConsoleColor CellFgColour { get; set; } = (ConsoleColor)(-1);
        public TextAlignment CellTextAlignment { get; set; } = (TextAlignment)(-1);

        public ConsoleTableAttribute(string displayName)
        {
            DisplayName = displayName;

        }
        public ConsoleTableAttribute() { }
    }

    public class TableStyle
    {
        public BorderStyles? BorderStyle { get; set; }
        public ConsoleColor? BorderColor { get; set; }
        public ConsoleColor? BackgroundColor { get; set; }
        public int? CellPadding { get; set; }
        public int TableXPadding { get; set; }
        public int TableYPadding { get; set; }
        public CellStyle? HeaderCellStyle { get; set; }
        public CellStyle? DataCellStyle { get; set; }
        public bool UseRowSeperator { get; set; }
        public BorderStyles? RowSeperatorStyle { get; set; }
        public bool UseAnimation { get; set; }
        public int AnimationDelay { get; set; } = 100;
    }

    public class CellStyle
    {
        public ConsoleColor? ForegroundColor { get; set; }
        public ConsoleColor? BackgroundColor { get; set; }
        public TextAlignment? TextAlignment { get; set; }

    }

    private class TableHeader
    {
        public string PropertyName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public CellStyle HeaderStyle { get; set; } = new();
    }

    private class TableCell
    {
        public string? Value { get; set; }
        public CellStyle CellStyle { get; set; } = new();
        // You can add more metadata here if you want
    }

    public enum BorderStyles
    {
        SingleLine,
        SingleBoldLine,
        DoubleLine,
        DoubleToSingleLine,
        SingleToDoubleLine,
        SingleDashedLine,
        SingleDashedBoldLine,
        SingleCurvedLine,
        GoodOldAscii,
        ImprovedAscii
    }
    public enum PiecePos
    {
        TopLeft = 0,
        TopRight = 1,
        BottomLeft = 2,
        BottomRight = 3,
        Horizontal = 4,
        Vertical = 5,
        TopT = 6,
        BottomT = 7,
        RightT = 8,
        LeftT = 9,
        Cross = 10,

    }

    public enum TextAlignment
    {
        Left,
        Center,
        Right
    }

    private static readonly Dictionary<BorderStyles, char[]> _lines = new()
    {
        { BorderStyles.SingleLine ,           ['┌', '┐', '└', '┘', '─', '│', '┬', '┴', '┤', '├', '┼',]},
        { BorderStyles.SingleBoldLine ,       ['┏', '┓', '┗', '┛', '━', '┃', '┳', '┻', '┫', '┣', '╋',]},
        { BorderStyles.DoubleLine ,           ['╔', '╗', '╚', '╝', '═', '║', '╦', '╩', '╣', '╠', '╬',]},
        { BorderStyles.DoubleToSingleLine ,   ['╓', '╖', '╙', '╜', '─', '║', '╥', '╨', '╢', '╟', '╫',]},
        { BorderStyles.SingleToDoubleLine ,   ['╒', '╕', '╘', '╛', '═', '│', '╤', '╧', '╡', '╞', '╪',]},
        { BorderStyles.SingleDashedLine ,     ['┌', '┐', '└', '┘', '╌', '╎', '┬', '┴', '┤', '├', '┼',]},
        { BorderStyles.SingleDashedBoldLine , ['┏', '┓', '┗', '┛', '╍', '╏', '┳', '┻', '┫', '┣', '╋',]},
        { BorderStyles.SingleCurvedLine ,     ['╭', '╮', '╰', '╯', '─', '│', '┬', '┴', '┤', '├', '┼',]},
        { BorderStyles.GoodOldAscii ,         ['-', '-', '-', '-', '-', '│', '-', '-', '-', '-', '-',]},
        { BorderStyles.ImprovedAscii ,        ['+', '+', '+', '+', '-', '│', '+', '+', '+', '+', '+',]},
    };

    public static void PrintAsTable<T>(this T item, Action<TableStyle> tableConfig) => PrintAsTable([item], tableConfig);

    public static void PrintAsTable<T>(this List<T> items, Action<TableStyle> tableConfig)
    {
        var style = new TableStyle();
        tableConfig?.Invoke(style);

        PrintAsTable(items, style);
    }

    public static void PrintAsTable<T>(this T item, TableStyle? tableStyle = null) => PrintAsTable([item], tableStyle);

    public static void PrintAsTable<T>(this List<T> items, TableStyle? tableStyle = null)
    {
        var originalEncoding = Console.OutputEncoding;
        var originalBg = Console.BackgroundColor;
        var originalFg = Console.ForegroundColor;
        try
        {

            List<BorderStyles> specialEncodingStyles =
            [
                BorderStyles.SingleBoldLine,
                BorderStyles.SingleDashedBoldLine,
                BorderStyles.SingleCurvedLine,
                BorderStyles.SingleDashedLine
            ];

            var type = typeof(T);
            var allProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var properties = allProperties.Where(p => p.GetCustomAttribute<ConsoleTableAttribute>()?.Hidden != true).ToList();


            var headers = properties.Select(p =>
            {
                var attr = p.GetCustomAttribute<ConsoleTableAttribute>();
                return new TableHeader
                {
                    PropertyName = p.Name,
                    DisplayName = attr?.DisplayName ?? p.Name,
                    HeaderStyle = new CellStyle
                    {
                        BackgroundColor = (int?)attr?.HeaderBgColour != -1 ? attr?.HeaderBgColour : null,
                        ForegroundColor = (int?)attr?.HeaderFgColour != -1 ? attr?.HeaderFgColour : null,
                        TextAlignment = (int?)attr?.HeaderTextAlignment != -1 ? attr?.HeaderTextAlignment : null
                    }

                };
            }).ToList();

            var rows = items.Select(item =>
                properties.Select(p =>
                {
                    var attr = p.GetCustomAttribute<ConsoleTableAttribute>();
                    var val = p.GetValue(item);
                    string strValue;

                    if (val == null) strValue = "";
                    else if (!string.IsNullOrEmpty(attr?.Format) && val is IFormattable formattable) strValue = formattable.ToString(attr.Format, null);
                    else strValue = val?.ToString() ?? string.Empty;

                    CellStyle cellStyle = new();
                    if (attr != null)
                    {
                        if ((int)attr.CellBgColour != -1) cellStyle.BackgroundColor = attr.CellBgColour;
                        if ((int)attr.CellFgColour != -1) cellStyle.ForegroundColor = attr.CellFgColour;
                        if ((int)attr.CellTextAlignment != -1) cellStyle.TextAlignment = attr.CellTextAlignment;
                    }

                    return new TableCell
                    {
                        Value = strValue,
                        CellStyle = cellStyle
                    };
                }).ToList()
            ).ToList();

            //var type = typeof(T);
            //var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            //var headers = properties.Select(p => p.Name).ToList();

            //var rows = items.Select(item =>
            //    properties.Select(prop => (prop.GetValue(item)?.ToString()) ?? "").ToList()
            //).ToList();

            if ((tableStyle?.BorderStyle != null && specialEncodingStyles.Contains(tableStyle.BorderStyle.Value)) ||
                (tableStyle?.RowSeperatorStyle != null && specialEncodingStyles.Contains(tableStyle.RowSeperatorStyle.Value)))
            {
                Console.OutputEncoding = Encoding.UTF8;
            }

            var originalCursor = true;
            if (OperatingSystem.IsWindows()) originalCursor = Console.CursorVisible;

            Console.CursorVisible = false;
            PrintTable(headers, rows, tableStyle);
            Console.CursorVisible = originalCursor;


        }
        catch (Exception ex)
        {

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(ex.ToString());

        }
        finally
        {
            Console.OutputEncoding = originalEncoding;
            Console.BackgroundColor = originalBg;
            Console.ForegroundColor = originalFg;
        }
    }

    private static void PrintTable(List<TableHeader> headers, List<List<TableCell>> rows, TableStyle? tableStyle = null)
    {


        var borderStyle = tableStyle?.BorderStyle ?? BorderStyles.SingleLine;
        ConsoleColor? borderColor = tableStyle?.BorderColor;
        ConsoleColor? borderBgColor = tableStyle?.BackgroundColor;
        var colWidths = Enumerable.Range(0, headers.Count)
            .Select(i => Math.Max(
                headers[i].DisplayName.Trim().Length,
                rows.Max(row => row[i].Value?.Trim().Length ?? 0)
            )).ToList();


        var xCellPadding = Math.Max(tableStyle?.CellPadding ?? 1, 0);

        var colCount = headers.Count;
        var rowCount = rows.Count;


        PrintYPadding(tableStyle?.TableYPadding);
        PrintXPadding(tableStyle?.TableXPadding);

        // draw top border
        PrintBorder(borderStyle, PiecePos.TopLeft, borderColor, borderBgColor);
        for (var i = 0; i < colCount; i++)
        {
            var col = headers[i];
            var colWidth = colWidths[i];
            for (var j = 0; j < colWidth + (2 * xCellPadding); j++)
            {
                PrintBorder(borderStyle, PiecePos.Horizontal, borderColor, borderBgColor);
            }
            if (i < colCount - 1) PrintBorder(borderStyle, PiecePos.TopT, borderColor, borderBgColor);

        }
        PrintBorder(borderStyle, PiecePos.TopRight, borderColor, borderBgColor);
        Console.WriteLine();

        // print header
        PrintXPadding(tableStyle?.TableXPadding);
        for (var i = 0; i < headers.Count; i++)
        {
            PrintBorder(borderStyle, PiecePos.Vertical, borderColor, borderBgColor);
            PrintCell(headers[i].DisplayName, colWidths[i], xCellPadding, tableStyle?.HeaderCellStyle, headers[i].HeaderStyle, tableStyle?.BackgroundColor);
        }
        PrintBorder(borderStyle, PiecePos.Vertical, borderColor, borderBgColor);
        Console.WriteLine();

        // print border below header
        PrintXPadding(tableStyle?.TableXPadding);
        PrintBorder(borderStyle, PiecePos.LeftT, borderColor, borderBgColor);
        for (var i = 0; i < colCount; i++)
        {
            var col = headers[i].DisplayName.Trim();
            var colWidth = colWidths[i];
            for (var j = 0; j < colWidth + (2 * xCellPadding); j++)
            {
                PrintBorder(borderStyle, PiecePos.Horizontal, borderColor, borderBgColor);
            }
            if (i < colCount - 1) PrintBorder(borderStyle, PiecePos.Cross, borderColor, borderBgColor);
        }
        PrintBorder(borderStyle, PiecePos.RightT, borderColor, borderBgColor);
        Console.WriteLine();

        var animationDelay = tableStyle != null ? Math.Clamp(tableStyle.AnimationDelay, 0, 200) : 0;
        // print rows
        for (var i = 0; i < rowCount; i++)
        {
            if (tableStyle?.UseAnimation ?? false) Thread.Sleep(animationDelay);
            PrintXPadding(tableStyle?.TableXPadding);
            for (var j = 0; j < colCount; j++)
            {
                if (tableStyle?.UseAnimation ?? false) Thread.Sleep(animationDelay);
                PrintBorder(borderStyle, PiecePos.Vertical, borderColor, borderBgColor);

                var cell = rows[i][j];
                PrintCell(cell.Value?.Trim() ?? string.Empty, colWidths[j], xCellPadding, tableStyle?.DataCellStyle, cell.CellStyle,tableStyle?.BackgroundColor);
            }
            PrintBorder(borderStyle, PiecePos.Vertical, borderColor, borderBgColor);
            Console.WriteLine();

            if (i < rowCount - 1 && (tableStyle?.UseRowSeperator ?? false))
            {
                var rowSeperatorStyle = tableStyle?.RowSeperatorStyle ?? borderStyle;
                PrintXPadding(tableStyle?.TableXPadding);
                PrintBorder(rowSeperatorStyle, PiecePos.LeftT, borderColor, borderBgColor);
                for (var j = 0; j < colCount; j++)
                {
                    var col = headers[j];
                    var colWidth = colWidths[j];
                    for (var j1 = 0; j1 < colWidth + (2 * xCellPadding); j1++)
                    {
                        PrintBorder(rowSeperatorStyle, PiecePos.Horizontal, borderColor, borderBgColor);
                    }
                    if (j < colCount - 1) PrintBorder(rowSeperatorStyle, PiecePos.Cross, borderColor, borderBgColor);


                }
                PrintBorder(rowSeperatorStyle, PiecePos.RightT, borderColor, borderBgColor);
                Console.WriteLine();

            }

        }

        PrintXPadding(tableStyle?.TableXPadding);
        PrintBorder(borderStyle, PiecePos.BottomLeft, borderColor, borderBgColor);
        for (var i = 0; i < colCount; i++)
        {
            var col = headers[i];
            var colWidth = colWidths[i];
            for (var j = 0; j < colWidth + (2 * xCellPadding); j++)
            {
                PrintBorder(borderStyle, PiecePos.Horizontal, borderColor, borderBgColor);
            }
            if (i < colCount - 1) PrintBorder(borderStyle, PiecePos.BottomT, borderColor, borderBgColor);

        }
        PrintBorder(borderStyle, PiecePos.BottomRight, borderColor, borderBgColor);
        Console.WriteLine();
        PrintYPadding(tableStyle?.TableYPadding);


    }

    private static void PrintYPadding(int? padding)
    {

        for (int yPadding = 0; yPadding < (padding ?? 0); yPadding++) Console.WriteLine("");
    }

    private static void PrintXPadding(int? padding)
    {
        for (int xPadding = 0; xPadding < (padding ?? 0); xPadding++) Console.Write(" ");
    }


    private static void PrintBorder(BorderStyles style, PiecePos pos, ConsoleColor? color = null, ConsoleColor? bgColor = null)
    {

        var originalBg = Console.BackgroundColor;
        var originalFg = Console.ForegroundColor;

        if (color != null) Console.ForegroundColor = color.Value;
        if (bgColor != null) Console.BackgroundColor = bgColor.Value;
        Console.Write(_lines[style][(int)pos]);
        //Console.ResetColor();
        Console.BackgroundColor = originalBg;
        Console.ForegroundColor = originalFg;
    }

    private static void PrintCell(string cell, int colWidth, int xPadding, CellStyle? tableCellStyle, CellStyle overrideStyle,ConsoleColor? borderBgColor)
    {
        var originalBg = Console.BackgroundColor;
        var originalFg = Console.ForegroundColor;
        var cellWidth = colWidth + (2 * xPadding);


        int leftPadding = xPadding;
        int rightPadding = xPadding;

        switch (overrideStyle.TextAlignment ?? tableCellStyle?.TextAlignment ?? TextAlignment.Left)
        {
            case TextAlignment.Left:
                rightPadding = cellWidth - cell.Length - xPadding;
                break;
            case TextAlignment.Center:
                leftPadding = (int)Math.Ceiling((cellWidth - cell.Length) / 2.0);
                rightPadding = cellWidth - cell.Length - leftPadding;
                break;
            case TextAlignment.Right:
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
        //Console.ResetColor();
        Console.BackgroundColor = originalBg;
        Console.ForegroundColor = originalFg;
    }




}
