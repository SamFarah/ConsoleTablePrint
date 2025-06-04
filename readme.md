[![NuGet](https://img.shields.io/nuget/v/ConsoleTablesPrinter.svg?style=flat-square)](https://www.nuget.org/packages/ConsoleTablesPrinter/)
# ConsoleTablesPrinter
A simple, flexible, and customizable table printer for .NET console applications.  
Easily print objects and collections as styled console tables with minimal setup.

---

## Features

- Print single objects or lists as formatted console tables  
- Customizable table styles, borders, colors, padding, and "animations"
- Support for property-level formatting and visibility using attributes  
- Multiple built-in border styles with UTF-8 or ASCII fallback  
- Supports cell text alignment and custom foreground/background colors  
- Easy to use extension methods for quick printing  

---

## Installation

Install via NuGet:

```bash
dotnet add package ConsoleTablePrinter --version 1.0.4
```


Or using the NuGet Package Manager:

```
Install-Package ConsoleTablePrinter -Version 1.0.4
```

---

## Usage


### Print a list of objects

```csharp
using ConsoleTablesPrinter;
class Person
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public DateOnly? DoB { get; set; }  
    public string? City { get; set; } 
}
class Program
{
    static void Main(string[] args)
    {
        var people = new List<Person>
        {
            new() { Id=1, Name = "Alice Johnson", Email="Alice@example.com", DoB = new DateOnly(1962,4,13), City = "Seattle" },
            new() { Id=2, Name = "Bob Smith", Email="bob@example.com", DoB =  new DateOnly(1991,7,27), City = "Portland" },
            new() { Id=3, Name = "Charlie Potato", Email="charlie.potato@example.com", DoB=  new DateOnly(1962,4,13), City = "San Francisco" }
        };
        people.PrintAsTable();
    }
}
```

![screenshot](https://raw.githubusercontent.com/SamFarah/ConsoleTablesPrinter/refs/heads/main/Screenshots/screenshot1.png)

### Print a single object

```csharp
var person = new Person() { Id=1, Name = "Alice Johnson", Email="Alice@eample.com", DoB = new DateOnly(1962,4,13), City = "Seattle" };
person.PrintAsTable();
```

![screenshot](https://raw.githubusercontent.com/SamFarah/ConsoleTablesPrinter/refs/heads/main/Screenshots/screenshot2.1.png)

When printing a single object, the table pivots into a 2-column layout showing property names and values.


## Customize Table Style

You can customize the look and feel of your tables using the `TableStyle` class. Below are the available styling properties:

### Borders

| Property | Description |
|----------|-------------|
| `BorderStyle` | Sets the style of the outer table border. Defaults to `SingleLine` if `null` or unset. the available options are described below. |
| `BorderColor` | The color used for the border lines, it uses the default `System.ConsoleColor`. Defaults to the console’s foreground color if `null` or unset. |

---

### Colors and Cell Styles

| Property | Description |
|----------|-------------|
| `BackgroundColor` | Background color applied to the entire table. |
| `HeaderCellStyle` | Style settings for header cells (colors, alignment). Can be overridden per column using `[TablePrintCol]`. |
| `DataCellStyle` | Style settings for data cells. Can be overridden per column using `[TablePrintCol]`. |

Both `HeaderCellStyle` and `DataCellStyle` are of type `CellStyle`

| Property | Description |
|----------|-------------|
| `BackgroundColor` | Background color applied to the cell. |
| `ForegroundColor` | Text (Foreground) color applied to the cell. |
| `TextAlignment` | Text alignment [Left, Center, Right]. Defaults to left if `null` or unset |

---

### Padding & Spacing

| Property | Description |
|----------|-------------|
| `CellHorizontalPadding` | Number of spaces inside each cell (horizontal). |
| `HorizontalPadding` | Spaces between the table and the left edge of the console. |
| `VerticalPadding` | Blank lines above and below the table. |

---

### Row Layout

| Property | Description |
|----------|-------------|
| `UseRowSeparator` | If set to `true` it will add a line between each row for better readability. |
| `RowSeparatorStyle` | (Optional) Use a different border style for row separators. Style of the row separators. Falls back to `BorderStyle` if not set. Only applies if `UseRowSeparator` is `true`. |

---

### "Animation"

| Property | Description |
|----------|-------------|
| `UseAnimation` | Enables a row-by-row reveal effect. |
| `AnimationDelay` | Delay (in ms) between rows during animation. Clamped between `0` and `200`. |

---

## Supported Border Styles

| Property | Uses the Characters |
|----------|-------------|
| `SingleLine` | ┌ ┐ └ ┘ ─ │ ┬ ┴ ┤ ├ ┼  |
| `SingleBoldLine` | ┏ ┓ ┗ ┛ ━ ┃ ┳ ┻ ┫ ┣ ╋   |
| `DoubleLine` | ╔ ╗ ╚ ╝ ═ ║ ╦ ╩ ╣ ╠ ╬ |
| `DoubleToSingleLine` | ╓ ╖ ╙ ╜ ─ ║ ╥ ╨ ╢ ╟ ╫   |
| `SingleToDoubleLine` | ╒ ╕ ╘ ╛ ═ │ ╤ ╧ ╡ ╞ ╪ |
| `SingleDashedLine`| ┌ ┐ └ ┘ ╌ ╎ ┬ ┴ ┤ ├ ┼   |
| `SingleDashedBoldLine` | ┏ ┓ ┗ ┛ ╍ ╏ ┳ ┻ ┫ ┣ ╋   |
| `SingleCurvedLine` | ╭ ╮ ╰ ╯ ─ │ ┬ ┴ ┤ ├ ┼   |
| `GoodOldAscii` | - │ |
| `ImprovedAscii` | + - │ |

---

### Example Usage

Styles can be applied either inline via a lambda or by passing a pre-configured `TableStyle` object to `PrintAsTable()`.

```csharp
people.PrintAsTable(style =>
{
    style.BorderStyle = BorderStyles.SingleBoldLine;
    style.CellHorizontalPadding = 2;
    style.BackgroundColor = ConsoleColor.DarkBlue;
    style.BorderColor = ConsoleColor.Cyan;
    style.HeaderCellStyle = new CellStyle()
    {
        BackgroundColor = ConsoleColor.Cyan,
        ForegroundColor = ConsoleColor.DarkBlue,
        TextAlignment = TextAlignments.Center
    };
});
```
![screenshot](https://raw.githubusercontent.com/SamFarah/ConsoleTablesPrinter/refs/heads/main/Screenshots/screenshot3.png)

Or

```csharp
people.PrintAsTable(new TableStyle()
{
    BorderStyle = BorderStyles.SingleToDoubleLine,
    UseRowSeparator = true,
    RowSeparatorStyle = BorderStyles.SingleLine,
    BorderColor = ConsoleColor.Red,
    HorizontalPadding = 5,
    VerticalPadding = 1,
           
});
```
![screenshot](https://raw.githubusercontent.com/SamFarah/ConsoleTablesPrinter/refs/heads/main/Screenshots/screenshot4.png)

---

## Attributes for column customization

Use `[TablePrintCol]` attribute on your model properties to control how columns display. These will override other styling defined on the table that affect the property.

```csharp
class Person
{
    public int Id { get; set; }
    [TablePrintCol(DisplayName = "Full Name")]
    public string? Name { get; set; }

    [TablePrintCol(Format = "C", CellTextAlignment = TextAlignments.Right, HeaderTextColor = ConsoleColor.Red, CellBgColor = ConsoleColor.DarkGray)]    
    public double Salary { get; set; }

    [TablePrintCol(Hidden = true)]
    public string? SecretCode { get; set; }
    
    public string? Email { get; set; }

    [TablePrintCol(DisplayName ="Date of Birth",Format = "MMM dd, yyyy",CellTextColor =ConsoleColor.Magenta)]
    public DateOnly? DoB { get; set; }
    public string? City { get; set; }
}
```

When printing a list of `Person`:

```csharp
people.PrintAsTable(new TableStyle()
        {
            BorderStyle = BorderStyles.SingleToDoubleLine,
            UseRowSeparator = true,
            RowSeparatorStyle = BorderStyles.SingleLine,
            BorderColor = ConsoleColor.Red,
            HorizontalPadding = 5,
            VerticalPadding = 1,
            HeaderCellStyle = new CellStyle()
            {
                BackgroundColor = ConsoleColor.Yellow,
                ForegroundColor = ConsoleColor.Black,
            }
        });
```

![screenshot](https://raw.githubusercontent.com/SamFarah/ConsoleTablesPrinter/refs/heads/main/Screenshots/screenshot5.png)

When printing a single `Person` object, styling is preserved and applied to the pivoted layout:

```csharp
ConsoleTablePrinter.DefaultStyle = new TableStyle()
{
    BorderStyle = BorderStyles.SingleToDoubleLine,
    UseRowSeparator = true,
    RowSeparatorStyle = BorderStyles.SingleLine,
    BorderColor = ConsoleColor.Red,
    HorizontalPadding = 5,
    VerticalPadding = 1,
    HeaderCellStyle = new CellStyle()
    {
        BackgroundColor = ConsoleColor.Yellow,
        ForegroundColor = ConsoleColor.Black,
    }
};
var person = new Person() { Id = 1, Name = "Alice Johnson", Email = "Alice@eample.com", DoB = new DateOnly(1962, 4, 13), City = "Seattle" };
people.PrintAsTable();
person.PrintAsTable();
```

![screenshot](https://raw.githubusercontent.com/SamFarah/ConsoleTablesPrinter/refs/heads/main/Screenshots/screenshot6.png)

### Supported attributes:

The following attributes can be used on the model properties

| Attribute | Description |
|-----------|-------------|
| `DisplayName` | The header text to display for the column. If not specified, the property name will be used as the header. |
| `Format` | The format string used to format the column's values. This supports standard .NET format strings, e.g. "C2" for currency with two decimals. |
| `Hidden` | Indicate whether this column should be hidden from the output. |
| `HeaderBgColor` | The background color of the column header. |
| `HeaderTextColor` | The text color of the column header. |
| `HeaderTextAlignment` | The text alignment of the column header. |
| `CellBgColor` | The background color of the cell content in this column. |
| `CellTextColor` | The text color of the cell content in this column. |
| `CellTextAlignment` | The text alignment of the cell content in this column. |

---

## TableStyle default

You can set a default style globally:

```csharp
ConsoleTablePrinter.DefaultStyle = new TableStyle()
{
    BorderStyle = BorderStyles.SingleBoldLine,
    CellHorizontalPadding = 1,
    BackgroundColor = ConsoleColor.Black,
    BorderColor = ConsoleColor.Green,
};
```

If no style is specified in `PrintAsTable()`, this default will be used.

![screenshot](https://raw.githubusercontent.com/SamFarah/ConsoleTablesPrinter/refs/heads/main/Screenshots/screenshot7.png)

---


## Version History

| Version       | Last updated  |
| ------------- |---------------|
| 1.0.0 | 2025-06-02 |
| 1.0.1 | 2025-06-03 |
| 1.0.2 | 2025-06-03 |
| 1.0.3 | 2025-06-03 |
| 1.0.4 | 2025-06-03 |

---
## License

MIT License © Sam Farah

---

## Contact

Feel free to open issues or contribute on GitHub.  
[GitHub Repository](https://github.com/SamFarah/ConsoleTablesPrinter)  

---

Enjoy nicely formatted tables in your console apps!
