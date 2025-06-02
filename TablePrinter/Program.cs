namespace TablePrinter;

class Program
{
    public enum LineStyles
    {
        SingleLine,
        SingleBoldLine,
        DoubleLine,
        DoubleToSingleLine,
        SingleToDoubleLine,
        SingleDashedLine,
        SingleDashedBoldLine,
        SingleCurvedLine
    }
    public enum Pos
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
    public static readonly Dictionary<LineStyles, char[]> lines = new()
    {
        { LineStyles.SingleLine ,           ['┌', '┐', '└', '┘', '─', '│', '┬', '┴', '┤', '├', '┼',]},
        { LineStyles.SingleBoldLine ,       ['┏', '┓', '┗', '┛', '━', '┃', '┳', '┻', '┫', '┣', '╋',]},
        { LineStyles.DoubleLine ,           ['╔', '╗', '╚', '╝', '═', '║', '╦', '╩', '╣', '╠', '╬',]},
        { LineStyles.DoubleToSingleLine ,   ['╓', '╖', '╙', '╜', '─', '║', '╥', '╨', '╢', '╟', '╫',]},
        { LineStyles.SingleToDoubleLine ,   ['╒', '╕', '╘', '╛', '═', '│', '╤', '╧', '╡', '╞', '╪',]},
        { LineStyles.SingleDashedLine ,     ['┌', '┐', '└', '┘', '╌', '╎', '┬', '┴', '┤', '├', '┼',]},
        { LineStyles.SingleDashedBoldLine , ['┏', '┓', '┗', '┛', '╍', '╏', '┳', '┻', '┫', '┣', '╋',]},
        { LineStyles.SingleCurvedLine ,     ['╭', '╮', '╰', '╯', '─', '│', '┬', '┴', '┤', '├', '┼',] }
    };
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;


        //var style = LineStyles.SingleLine;

        foreach (LineStyles style in Enum.GetValues(typeof(LineStyles)))
        {
            Console.WriteLine("\n\n\n---------------");
            Console.WriteLine(style.ToString());
            Console.WriteLine("---------------\n");

            List<string> columns = ["col 1", "another colmn", "third co", "total"];

            var xPadding = 2;

            var colCount = columns.Count;
            var rowCount = 4;
            //var width = columns.Sum(x => x.Length + (2 * xPadding)) + columns.Count - 1;


            PrintBorder(style, Pos.TopLeft);
            for (var i = 0; i < colCount; i++)
            {
                var col = columns[i];
                for (var j = 0; j < col.Length + (2 * xPadding); j++)
                {
                    PrintBorder(style, Pos.Horizontal);
                }
                if (i < colCount - 1) PrintBorder(style, Pos.TopT);

            }
            PrintBorder(style, Pos.TopRight);
            Console.WriteLine();
            foreach (var col in columns)
            {
                PrintBorder(style, Pos.Vertical);
                Console.Write("".PadLeft(xPadding, ' '));
                Console.Write(col);
                Console.Write("".PadRight(xPadding, ' '));
            }
            PrintBorder(style, Pos.Vertical);
            Console.WriteLine();

            PrintBorder(style, Pos.LeftT);
            for (var i = 0; i < colCount; i++)
            {
                var col = columns[i];
                for (var j = 0; j < col.Length + (2 * xPadding); j++)
                {
                    PrintBorder(style, Pos.Horizontal);
                }
                if (i < colCount - 1) PrintBorder(style, Pos.Cross);
            }
            PrintBorder(style, Pos.RightT);
            Console.WriteLine();

            for (var i = 0; i < rowCount; i++)
            {


                for (var j = 0; j < colCount; j++)
                {
                    PrintBorder(style, Pos.Vertical);
                    Console.Write("".PadLeft(xPadding, ' '));
                    Console.Write("".PadLeft(columns[j].Length, ' '));
                    Console.Write("".PadRight(xPadding, ' '));
                }
                PrintBorder(style, Pos.Vertical);
                Console.WriteLine();
            }
            PrintBorder(style, Pos.BottomLeft);
            for (var i = 0; i < colCount; i++)
            {
                var col = columns[i];
                for (var j = 0; j < col.Length + (2 * xPadding); j++)
                {
                    PrintBorder(style, Pos.Horizontal);
                }
                if (i < colCount - 1) PrintBorder(style, Pos.BottomT);

            }
            PrintBorder(style, Pos.BottomRight);

        }
    }

    public static void PrintBorder(LineStyles style, Pos pos) => Console.Write(lines[style][(int)pos]);

}
