namespace ConsoleTablesPrinter.Test;
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
        string? ansifg = "\u001b[38;2;200;100;200m";
        string? ansibg = "\u001b[48;2;130;30;30m";

        Console.WriteLine(ansifg);  // set the foreground
        Console.Write(ansibg);      // Set background 
        Console.Write("\u001b[2J"); // Apply colors to the whole window by clearing the screen


        var people = new List<Person>
        {
            new() { Id=1, Name = "Alice Johnson", Email="Alice@example.com", DoB = new DateOnly(1962,4,13), City = "Seattle" },
            new() { Id=2, Name = "Bob Smith", Email="bob@example.com", DoB =  new DateOnly(1991,7,27), City = "Portland" },
            new() { Id=3, Name = "Charlie Potato", Email="charlie.potato@example.com", DoB=  new DateOnly(1962,4,13), City = "San Francisco" }
        };


        ConsoleTablePrinter.ConsoleAnsiFg = ansifg;
        ConsoleTablePrinter.ConsoleAnsiBg = ansibg;
        people.PrintAsTable(o => o.BorderStyle = BorderStyles.GoodOldAscii);
        people.PrintAsTable(o => o.PrintMode = TablePrintModes.Markdown);
    }
}