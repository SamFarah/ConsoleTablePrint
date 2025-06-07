using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace ConsoleTablesPrinter
{
    public static partial class ConsoleTablePrinter
    {

        private static string _styleResetter = string.Empty;
        private static StringBuilder stringBuilder = new StringBuilder();        

        private static void PrintListAsTable<T>(this List<T> items, TableStyle? tableStyle = null)
        {
            tableStyle ??= DefaultStyle;
            var ignoreStyling = tableStyle?.PrintMode == TablePrintModes.Markdown;

            var properties = GetVisibleProperties<T>();
            var propsWithAttrs = GetVisibleProperties<T>()
            .Select(p => new
            {
                Property = p,
                Attribute = p.GetCustomAttribute<TablePrintColAttribute>()
            })
            .ToList();

            var headers = propsWithAttrs.Select(pa => new TableHeader
            {
                PropertyName = pa.Property.Name,
                DisplayName = pa.Attribute?.DisplayName ?? pa.Property.Name,
                HeaderStyle = ignoreStyling ? new CellStyle() : GetHeaderStyle(pa.Attribute)
            }).ToList();


            var rows = items.Select(item =>
               propsWithAttrs.Select(pa =>
               {
                   var val = pa.Property.GetValue(item);
                   return new TableCell
                   {
                       Value = FormatValue(val, pa.Attribute),
                       CellStyle = ignoreStyling ? new CellStyle() : GetCellStyle(pa.Attribute)
                   };
               }).ToList()
           ).ToList();


            var requiresUtf8 = tableStyle != null && (_specialEncodingStyles.Contains(tableStyle.BorderStyle.GetValueOrDefault()) ||
                                                      _specialEncodingStyles.Contains(tableStyle.RowSeparatorStyle.GetValueOrDefault()));

            ConsoleStateManager.WithConsoleState(() => PrintTable(headers, rows, tableStyle), requiresUtf8, ConsoleAnsiBg, ConsoleAnsiFg);
        }

        private static void PrintObjectAsTable<T>(this T item, TableStyle? tableStyle = null)
        {
            tableStyle ??= DefaultStyle;
            var ignoreStyling = tableStyle?.PrintMode == TablePrintModes.Markdown;

            var properties = GetVisibleProperties<T>();

            var headers = new List<TableHeader>
            {
                new TableHeader() { PropertyName = "Property", DisplayName = "Property" },
                new TableHeader() { PropertyName = "Value", DisplayName = "Value" }
            };

            var rows = properties.Select(p =>
            {
                var attr = p.GetCustomAttribute<TablePrintColAttribute>();
                var val = p.GetValue(item);

                return new List<TableCell>
                {
                    new TableCell() { Value = attr?.DisplayName ?? p.Name, CellStyle = ignoreStyling? new CellStyle(): GetHeaderStyle(attr) },
                    new TableCell() { Value = FormatValue(val, attr), CellStyle = ignoreStyling? new CellStyle():GetCellStyle(attr) }
                };
            }).ToList();

            var requiresUtf8 = tableStyle != null && (_specialEncodingStyles.Contains(tableStyle.BorderStyle.GetValueOrDefault()) ||
                                                      _specialEncodingStyles.Contains(tableStyle.RowSeparatorStyle.GetValueOrDefault()));

            ConsoleStateManager.WithConsoleState(() => PrintTable(headers, rows, tableStyle), requiresUtf8, ConsoleAnsiBg, ConsoleAnsiFg);
        }

        private static void PrintTable(List<TableHeader> headers, List<List<TableCell>> rows, TableStyle? tableStyle = null)
        {
            if (headers.Count == 0 || rows.Count == 0) return;
            var isMarkdown = tableStyle?.PrintMode == TablePrintModes.Markdown;

            // if the table does not use any colours. no point in wasting time reseting colours. this makes printing uncoloured tables much faster
            bool hasAnyColors = !isMarkdown &&(
                headers.Any(h => h.HeaderStyle.ForegroundColor.HasValue || h.HeaderStyle.BackgroundColor.HasValue) ||
                rows.Any(row => row.Any(cell => cell.CellStyle.ForegroundColor.HasValue || cell.CellStyle.BackgroundColor.HasValue)) ||
                (tableStyle?.BorderColor.HasValue ?? false) ||
                (tableStyle?.BackgroundColor.HasValue ?? false) ||
                (tableStyle?.HeaderCellStyle?.ForegroundColor.HasValue ?? false) ||
                (tableStyle?.HeaderCellStyle?.BackgroundColor.HasValue ?? false) ||
                (tableStyle?.DataCellStyle?.ForegroundColor.HasValue ?? false) ||
                (tableStyle?.DataCellStyle?.BackgroundColor.HasValue ?? false));

            _styleResetter = hasAnyColors ? _styleResetter : string.Empty;

            tableStyle ??= DefaultStyle;
            stringBuilder = new StringBuilder();

            var borderStyle =  tableStyle?.BorderStyle ?? BorderStyles.SingleLine;
            var currentBorderLines = isMarkdown? _markdownLines :  _borderLlines[borderStyle];
            var animationDelay = tableStyle != null ? Math.Clamp(tableStyle.AnimationDelay, 0, 200) : 0;
            ConsoleColor? borderColor = isMarkdown ? null : tableStyle?.BorderColor;
            ConsoleColor? borderBgColor = isMarkdown ? null : tableStyle?.BackgroundColor;
            var colWidths = Enumerable.Range(0, headers.Count)
                .Select(i => Math.Max(
                    headers[i].DisplayName.Trim().Length, (rows.Count == 0 ? 0 :
                    rows.Max(row => row[i].Value?.Trim().Length ?? 0))
                )).ToList();


            var xCellPadding = isMarkdown ? 1 : Math.Max(tableStyle?.CellHorizontalPadding ?? 1, 0);

            var colCount = headers.Count;
            var rowCount = rows.Count;

            // create horozental Line Template, we need to calcualte this template once, and we can use it for every horozental line
            var hLineTemplate = string.Empty;
            hLineTemplate += "0";
            for (var i = 0; i < colCount; i++)
            {
                var colWidth = colWidths[i];
                hLineTemplate += "".PadLeft(colWidth + (2 * xCellPadding), '1');
                if (i < colCount - 1) hLineTemplate += "2";
            }
            hLineTemplate += "3";
            var hLinePadding = _styleResetter + "".PadLeft(isMarkdown ? 0 : tableStyle?.HorizontalPadding ?? 0, ' ');

            if (!isMarkdown)
            {
                PrintVerticalPadding(tableStyle?.VerticalPadding);
                PrintHorizentalLine(hLineTemplate, hLinePadding, currentBorderLines, HorLineDefs.TopLine, borderColor, borderBgColor);
                PrintHorizontalPadding(tableStyle?.HorizontalPadding);
            }
            // print header
            for (var i = 0; i < headers.Count; i++)
            {
                PrintBorder(currentBorderLines, PiecePos.Vertical, borderColor, borderBgColor);
                if (isMarkdown) PrintCell(headers[i].DisplayName, colWidths[i], xCellPadding);
                else PrintCell(headers[i].DisplayName, colWidths[i], xCellPadding, tableStyle?.HeaderCellStyle, headers[i].HeaderStyle, tableStyle?.BackgroundColor);
            }
            PrintBorder(currentBorderLines, PiecePos.Vertical, borderColor, borderBgColor, appendNewline: true);
            PrintHorizentalLine(hLineTemplate, hLinePadding, currentBorderLines, HorLineDefs.UnderHeaderLine, borderColor, borderBgColor);

            // print rows
            for (var i = 0; i < rowCount; i++)
            {
                if (!isMarkdown) PrintHorizontalPadding(tableStyle?.HorizontalPadding);
                for (var j = 0; j < colCount; j++)
                {
                    PrintBorder(currentBorderLines, PiecePos.Vertical, borderColor, borderBgColor);

                    var cell = rows[i][j];
                    if (isMarkdown) PrintCell(cell.Value?.Trim() ?? string.Empty, colWidths[j], xCellPadding);
                    else PrintCell(cell.Value?.Trim() ?? string.Empty, colWidths[j], xCellPadding, tableStyle?.DataCellStyle, cell.CellStyle, tableStyle?.BackgroundColor);
                }
                PrintBorder(currentBorderLines, PiecePos.Vertical, borderColor, borderBgColor, appendNewline: true);

                if (i < rowCount - 1 && !isMarkdown && (tableStyle?.UseRowSeparator ?? false))
                {
                    var rowSeparatorStyle = tableStyle?.RowSeparatorStyle ?? borderStyle;
                    var rowSeperatorLines = _borderLlines[rowSeparatorStyle];

                    // print row separator 
                    PrintHorizentalLine(hLineTemplate, hLinePadding, rowSeperatorLines, HorLineDefs.RowSeparator, borderColor, borderBgColor);
                }
            }

            if (!isMarkdown)
            {
                // print bottom line
                PrintHorizentalLine(hLineTemplate, hLinePadding, currentBorderLines, HorLineDefs.BottomLine, borderColor, borderBgColor);
                PrintVerticalPadding(Math.Max((tableStyle?.VerticalPadding ?? 0) - 1, 0)); // the -1 to counter the last blank line that will be introduced
            }

            var renderedTable = stringBuilder.ToString();
            if (!isMarkdown && (tableStyle?.UseAnimation ?? false))
            {
                var lines = renderedTable.Split(Environment.NewLine);
                foreach (var line in lines)
                {
                    Thread.Sleep(animationDelay);
                    Console.Out.WriteLine(line);
                }

            }
            else Console.Out.WriteLine(renderedTable);
        }

        private static void PrintVerticalPadding(int? padding) => stringBuilder.Append("".PadRight((padding ?? 0), '\n'));
        private static void PrintHorizontalPadding(int? padding) => stringBuilder.Append("".PadLeft((padding ?? 0), ' '));

        private static void PrintBorder(string borderLines, PiecePos pos, ConsoleColor? color = null, ConsoleColor? bgColor = null, bool appendNewline = false)
        {
            string bg = string.Empty;
            string fg = string.Empty;

            if (color != null) fg = _foregrounds[color.Value];
            if (bgColor != null) bg = _backgrounds[bgColor.Value];
            stringBuilder.Append(fg + bg + borderLines[(int)pos] + _styleResetter + (appendNewline ? "\n" : ""));
        }

        private static void PrintHorizentalLine(string lineTemplate, string linePadding, string borderLines, HorLineDefs lineDef, ConsoleColor? color = null, ConsoleColor? bgColor = null)
        {
            string bg = string.Empty;
            string fg = string.Empty;

            if (color != null) fg = _foregrounds[color.Value];
            if (bgColor != null) bg = _backgrounds[bgColor.Value];

            var linePieces = _lineDefinitions[lineDef];
            var line = lineTemplate;
            for (int i = 0; i < linePieces.Length; i++) line = line.Replace((char)(i + '0'), borderLines[(int)linePieces[i]]);
            // line = Regex.Replace(line, @"[0-3]", match => _lines[style][(int)linePieces[match.Value[0] - '0']].ToString()); // if anything it does worse performance

            stringBuilder.AppendLine(linePadding + fg + bg + line + _styleResetter);
        }

        private static void PrintCell(string cell, int colWidth, int xPadding = 0, CellStyle? tableCellStyle = null, CellStyle? overrideStyle = null, ConsoleColor? borderBgColor = null)
        {
            var cellWidth = colWidth + (2 * xPadding);

            int leftPadding = xPadding;
            int rightPadding = xPadding;

            switch (overrideStyle?.TextAlignment ?? tableCellStyle?.TextAlignment ?? TextAlignments.Left)
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

            string bg = string.Empty;
            string fg = string.Empty;

            if (overrideStyle?.BackgroundColor != null) bg = _backgrounds[overrideStyle.BackgroundColor.Value];
            else if (tableCellStyle?.BackgroundColor != null) bg = _backgrounds[tableCellStyle.BackgroundColor.Value];
            else if (borderBgColor != null) bg = _backgrounds[borderBgColor.Value];

            if (overrideStyle?.ForegroundColor != null) fg = _foregrounds[overrideStyle.ForegroundColor.Value];
            else if (tableCellStyle?.ForegroundColor != null) fg = _foregrounds[tableCellStyle.ForegroundColor.Value];

            stringBuilder.Append(fg + bg + "".PadLeft(leftPadding, ' '));
            stringBuilder.Append(cell);
            stringBuilder.Append("".PadRight(rightPadding, ' ') + _styleResetter);
        }
    }
}
