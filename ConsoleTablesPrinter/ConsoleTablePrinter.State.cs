using System;
using System.Collections.Generic;

namespace ConsoleTablesPrinter
{
    public static partial class ConsoleTablePrinter
    {
        private class TableHeader
        {
            public string PropertyName { get; set; } = string.Empty;
            public string DisplayName { get; set; } = string.Empty;
            public CellStyle HeaderStyle { get; set; } = new CellStyle();
        }

        private class TableCell
        {
            public string? Value { get; set; }
            public CellStyle CellStyle { get; set; } = new CellStyle();
        }

        private enum HorLineDefs
        {
            TopLine,
            UnderHeaderLine,
            RowSeparator,
            BottomLine
        }

        private enum PiecePos
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


        private static readonly Dictionary<BorderStyles, char[]> _borderLlines = new Dictionary<BorderStyles, char[]>()
        {
            { BorderStyles.SingleLine ,            new char[]{'┌', '┐', '└', '┘', '─', '│', '┬', '┴', '┤', '├', '┼',}},
            { BorderStyles.SingleBoldLine ,        new char[]{'┏', '┓', '┗', '┛', '━', '┃', '┳', '┻', '┫', '┣', '╋',}},
            { BorderStyles.DoubleLine ,            new char[]{'╔', '╗', '╚', '╝', '═', '║', '╦', '╩', '╣', '╠', '╬',}},
            { BorderStyles.DoubleToSingleLine ,    new char[]{'╓', '╖', '╙', '╜', '─', '║', '╥', '╨', '╢', '╟', '╫',}},
            { BorderStyles.SingleToDoubleLine ,    new char[]{'╒', '╕', '╘', '╛', '═', '│', '╤', '╧', '╡', '╞', '╪',}},
            { BorderStyles.SingleDashedLine ,      new char[]{'┌', '┐', '└', '┘', '╌', '╎', '┬', '┴', '┤', '├', '┼',}},
            { BorderStyles.SingleDashedBoldLine ,  new char[]{'┏', '┓', '┗', '┛', '╍', '╏', '┳', '┻', '┫', '┣', '╋',}},
            { BorderStyles.SingleCurvedLine ,      new char[]{'╭', '╮', '╰', '╯', '─', '│', '┬', '┴', '┤', '├', '┼',}},
            { BorderStyles.GoodOldAscii ,          new char[]{'-', '-', '-', '-', '-', '│', '-', '-', '-', '-', '-',}},
            { BorderStyles.ImprovedAscii ,         new char[]{'+', '+', '+', '+', '-', '│', '+', '+', '+', '+', '+',}},
        };

        private static readonly Dictionary<HorLineDefs, PiecePos[]> _lineDefinitions = new Dictionary<HorLineDefs, PiecePos[]>()
        {
                { HorLineDefs.TopLine, new PiecePos[]{PiecePos.TopLeft, PiecePos.Horizontal, PiecePos.TopT, PiecePos.TopRight } },
                { HorLineDefs.UnderHeaderLine, new PiecePos[]{PiecePos.LeftT, PiecePos.Horizontal, PiecePos.Cross, PiecePos.RightT }},
                { HorLineDefs.RowSeparator,new PiecePos[]{PiecePos.LeftT, PiecePos.Horizontal, PiecePos.Cross, PiecePos.RightT }},
                { HorLineDefs.BottomLine,new PiecePos[]{PiecePos.BottomLeft, PiecePos.Horizontal, PiecePos.BottomT, PiecePos.BottomRight }}
        };

        private static readonly List<BorderStyles> _specialEncodingStyles = new List<BorderStyles>
        {
            BorderStyles.SingleBoldLine,
            BorderStyles.SingleDashedBoldLine,
            BorderStyles.SingleCurvedLine,
            BorderStyles.SingleDashedLine
        };

        private static readonly Dictionary<ConsoleColor, string> _foregrounds = new Dictionary<ConsoleColor, string>()
        {
            {ConsoleColor.Black, "\x1b[30m"},
            {ConsoleColor.DarkRed, "\x1b[31m"},
            {ConsoleColor.DarkGreen, "\x1b[32m"},
            {ConsoleColor.DarkYellow, "\x1b[33m"},
            {ConsoleColor.DarkBlue, "\x1b[34m"},
            {ConsoleColor.DarkMagenta, "\x1b[35m"},
            {ConsoleColor.DarkCyan, "\x1b[36m"},
            {ConsoleColor.Gray, "\x1b[37m"},
            {ConsoleColor.DarkGray, "\x1b[90m"},
            {ConsoleColor.Red, "\x1b[91m"},
            {ConsoleColor.Green, "\x1b[92m"},
            {ConsoleColor.Yellow, "\x1b[93m"},
            {ConsoleColor.Blue, "\x1b[94m"},
            {ConsoleColor.Magenta, "\x1b[95m"},
            {ConsoleColor.Cyan, "\x1b[96m"},
            {ConsoleColor.White, "\x1b[97m"},
        };

        private static readonly Dictionary<ConsoleColor, string> _backgrounds = new Dictionary<ConsoleColor, string>()
        {
            {ConsoleColor.Black, "\x1b[40m"},
            {ConsoleColor.DarkRed, "\x1b[41m"},
            {ConsoleColor.DarkGreen, "\x1b[42m"},
            {ConsoleColor.DarkYellow, "\x1b[43m"},
            {ConsoleColor.DarkBlue, "\x1b[44m"},
            {ConsoleColor.DarkMagenta, "\x1b[45m"},
            {ConsoleColor.DarkCyan, "\x1b[46m"},
            {ConsoleColor.Gray, "\x1b[47m"},
            {ConsoleColor.DarkGray, "\x1b[100m"},
            {ConsoleColor.Red, "\x1b[101m"},
            {ConsoleColor.Green, "\x1b[102m"},
            {ConsoleColor.Yellow, "\x1b[103m"},
            {ConsoleColor.Blue, "\x1b[104m"},
            {ConsoleColor.Magenta, "\x1b[105m"},
            {ConsoleColor.Cyan, "\x1b[106m"},
            {ConsoleColor.White, "\x1b[107m"},
        };
    }
}