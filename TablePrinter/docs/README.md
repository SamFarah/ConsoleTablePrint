
# TablePrinter

A simple, flexible, and customizable console table printer for .NET applications.  
Easily print objects and collections as styled tables with minimal setup.

---

## Features

- Print single objects or lists as formatted console tables  
- Customizable table styles, borders, colors, paddings, and animations  
- Support for property-level formatting and visibility using attributes  
- Multiple built-in border styles with UTF-8 or ASCII fallback  
- Supports cell text alignment and custom foreground/background colors  
- Easy to use extension methods for quick printing  

---

## Installation

Install via NuGet:


dotnet add package TablePrinter --version 1.0.0


Or using the NuGet Package Manager:


Install-Package TablePrinter -Version 1.0.0


---

## Usage

### Print a single object


var person = new Person { Name = "Alice", Age = 30, City = "Seattle" };
person.PrintAsTable();


### Print a list of objects


var people = new List<Person>
{
    new() { Name = "Alice", Age = 30, City = "Seattle" },
    new() { Name = "Bob", Age = 25, City = "Portland" },
    new() { Name = "Charlie", Age = 35, City = "San Francisco" }
};
people.PrintAsTable();


### Customize table style


people.PrintAsTable(style =>
{
    style.BorderStyle = BorderStyles.DoubleLine;
    style.CellPadding = 2;
    style.UseAnimation = true;
    style.AnimationDelay = 50;
    style.BackgroundColor = ConsoleColor.DarkBlue;
    style.BorderColor = ConsoleColor.Cyan;
});


---

## Attributes for column customization

Use `[TablePrintCol]` attribute on your model properties to control how columns display:


public class Person
{
    [TablePrintCol(DisplayName = "Full Name", HeaderTextColor = ConsoleColor.Yellow)]
    public string Name { get; set; }

    [TablePrintCol(Format = "N0", CellTextAlignment = TextAlignment.Right)]
    public int Age { get; set; }

    [TablePrintCol(Hidden = true)]
    public string SecretCode { get; set; }
}


- `DisplayName`: Custom column header text  
- `Format`: String format for the value  
- `Hidden`: Hide this property in the table  
- `HeaderTextColor`, `CellTextAlignment`, etc. for styling  

---

## TableStyle defaults

You can set a default style globally:


ConsoleTablePrinter.DefaultStyle = new TableStyle
{
    BorderStyle = BorderStyles.SingleBoldLine,
    CellHorizontalPadding = 1,
    BackgroundColor = ConsoleColor.Black,
    BorderColor = ConsoleColor.Green,
};


If no style is specified in `PrintAsTable()`, this default will be used.

---

## Supported Border Styles

- `SingleLine`  
- `SingleBoldLine`  
- `DoubleLine`  
- `DoubleToSingleLine`  
- `SingleToDoubleLine`  
- `SingleDashedLine`  
- `SingleDashedBoldLine`  
- `SingleCurvedLine`  
- `GoodOldAscii`  
- `ImprovedAscii`  

---

## License

MIT License © Sam Farah

---

## Contact

Feel free to open issues or contribute on GitHub.  
[GitHub Repository](https://github.com/SamFarah/ConsoleTablePrint)  

---

Enjoy nicely formatted tables in your console apps!
