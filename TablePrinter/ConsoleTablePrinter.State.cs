namespace TablePrinter;
public static partial class ConsoleTablePrinter
{
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

    private enum HorLineDefs
    {
        TopLine,
        UnderHeaderLine,
        RowSeperator,
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


    private static readonly Dictionary<BorderStyles, char[]> _lines = new()
    {
        { BorderStyles.SingleLine ,           new char[]{'┌', '┐', '└', '┘', '─', '│', '┬', '┴', '┤', '├', '┼', } },
        { BorderStyles.SingleBoldLine ,        new char[]{'┏', '┓', '┗', '┛', '━', '┃', '┳', '┻', '┫', '┣', '╋',}},
        { BorderStyles.DoubleLine ,            new char[]{'╔', '╗', '╚', '╝', '═', '║', '╦', '╩', '╣', '╠', '╬',}},
        { BorderStyles.DoubleToSingleLine ,    new char[]{'╓', '╖', '╙', '╜', '─', '║', '╥', '╨', '╢', '╟', '╫',}},
        { BorderStyles.SingleToDoubleLine ,    new char[]{'╒', '╕', '╘', '╛', '═', '│', '╤', '╧', '╡', '╞', '╪',}},
        { BorderStyles.SingleDashedLine ,      new char[]{'┌', '┐', '└', '┘', '╌', '╎', '┬', '┴', '┤', '├', '┼',}},
        { BorderStyles.SingleDashedBoldLine ,  new char[]{'┏', '┓', '┗', '┛', '╍', '╏', '┳', '┻', '┫', '┣', '╋',}},
        { BorderStyles.SingleCurvedLine ,      new char[]{'╭', '╮', '╰', '╯', '─', '│', '┬', '┴', '┤', '├', '┼',}},
        { BorderStyles.GoodOldAscii ,          new char[]{'-', '-', '-', '-', '-', '│', '-', '-', '-', '-', '-',}},
        { BorderStyles.ImprovedAscii ,         new char[]{'+', '+', '+', '+', '-', '│', '+', '+', '+', '+', '+', } },
    };

    private static readonly Dictionary<HorLineDefs, PiecePos[]> _lineDefinitions = new()
    {
            { HorLineDefs.TopLine, new PiecePos[]{PiecePos.TopLeft, PiecePos.Horizontal, PiecePos.TopT, PiecePos.TopRight } },
            { HorLineDefs.UnderHeaderLine, new PiecePos[]{PiecePos.LeftT, PiecePos.Horizontal, PiecePos.Cross, PiecePos.RightT }},
            { HorLineDefs.RowSeperator,new PiecePos[]{PiecePos.LeftT, PiecePos.Horizontal, PiecePos.Cross, PiecePos.RightT }},
            { HorLineDefs.BottomLine,new PiecePos[]{PiecePos.BottomLeft, PiecePos.Horizontal, PiecePos.BottomT, PiecePos.BottomRight }}
    };

    private static readonly List<BorderStyles> _specialEncodingStyles =new List<BorderStyles>
    {
        BorderStyles.SingleBoldLine,
        BorderStyles.SingleDashedBoldLine,
        BorderStyles.SingleCurvedLine,
        BorderStyles.SingleDashedLine
    };
}
