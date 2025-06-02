using static TablePrinter.ConsoleTable;

namespace TablePrinter;

class Person
{
    public int Id { get; set; }

    //[ConsoleTable(DisplayName = "Name",CellTextAlignment = TextAlignment.Right)]
    public string? Name { get; set; }

    [ConsoleTable(CellFgColour = ConsoleColor.Magenta,CellTextAlignment = TextAlignment.Center)]
    public string? Email { get; set; }

    [ConsoleTable(Format = "MMM dd, yyyy",
    DisplayName = "Date Of Birth")]
    public DateOnly? DoB { get; set; }

    [ConsoleTable(DisplayName ="Annual Salary",Format ="C", CellFgColour = ConsoleColor.DarkCyan)]
    public double Salary { get; set; }
}


class Program
{
    static void Main(string[] args)
    {
        var rand = new Random((int)DateTime.Now.Ticks);
        List<Person> people =
        [
            new (){ Id = 1, Name = "Alice", Email = "alice@example.com", DoB=new(1993,9,12),  Salary =27386.21},
            new (){ Id = 2, Name = "Bob", Email = "bob@example.com" ,  Salary = 9076.556},
            new (){ Id = 2, Name = "Bob sdflksdjflk sjdf lkjs", Email = "bob@example.com" ,  Salary = 32667.803 },
            new (){ Id = 1, Name = "Alice", Email = "alice@example.com", DoB=new(1993,9,12),  Salary = 13209.741  },
            new (){ Id = 2, Name = "Bob", Email = "bob@example.com",  Salary = 31483.062  },
            new (){ Id = 2, Name = "Bob sdflksdjflk sjdf lkjs", Email = "bob@example.com",  Salary = 44228.16  },
            new (){ Id = 1, Name = "Alice", Email = "alice@example.com", DoB=new(1993,9,12) ,  Salary =5862.314},
       
        ];
        // Console.BackgroundColor = ConsoleColor.White;
        //Console.ForegroundColor = ConsoleColor.Red;
        //Console.ResetColor();
        //Console.Clear();
       // people.PrintAsTable(o=> { o.TableXPadding = 3;o.UseRowSeperator = true; });
       // people.PrintAsTable(o => o.TableXPadding = 3);

        people.PrintAsTable(new TableStyle()
        {
            BorderStyle = BorderStyles.SingleToDoubleLine,
            BorderColor = ConsoleColor.Cyan,
            BackgroundColor = ConsoleColor.Yellow,
            CellPadding = 1,
            UseRowSeperator = false,
            RowSeperatorStyle = BorderStyles.SingleLine,
            HeaderCellStyle = new()
            {
                BackgroundColor = ConsoleColor.DarkMagenta,
                ForegroundColor = ConsoleColor.DarkBlue,
                TextAlignment = TextAlignment.Right,
            },
            DataCellStyle = new()
            {
                TextAlignment = TextAlignment.Right,
                BackgroundColor = ConsoleColor.Red,
            },
            TableXPadding = 14,
            TableYPadding = 2,
          //  UseAnimation = true,
          //  AnimationDelay = 10

        });

        //people.PrintAsTable(style =>
        //{
        //    style.BorderStyle = BorderStyles.DoubleLine;
        //    style.BorderColor = ConsoleColor.Cyan;
        //    style.CellPadding = 2;
        //    style.UseRowSeperator = true;
        //    style.RowSeperatorStyle = BorderStyles.DoubleToSingleLine;
        //    style.HeaderCellStyle = new()
        //    {

        //        BackgroundColor = ConsoleColor.Cyan,
        //        ForegroundColor = ConsoleColor.Black,
        //        TextAlignment = TextAlignment.Center,
        //    };
        //    style.DataCellStyle = new()
        //    {
                
        //    };
        //    style.TableXPadding = 14;
        //    style.TableYPadding = 2;      
            
        //});

       // _ = Console.ReadKey();
    }
}

