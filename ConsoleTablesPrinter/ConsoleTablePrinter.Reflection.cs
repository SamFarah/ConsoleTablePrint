using System.Reflection;

namespace ConsoleTablesPrinter;
public static partial class ConsoleTablePrinter
{
    private static List<PropertyInfo> GetVisibleProperties<T>() =>
      typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
           .Where(p => p.GetCustomAttribute<TablePrintColAttribute>()?.Hidden != true)
           .ToList();

    private static string FormatValue(object? val, TablePrintColAttribute? attr)
    {
        if (val == null) return "";
        if (!string.IsNullOrEmpty(attr?.Format) && val is IFormattable fmt) return fmt.ToString(attr.Format, null);
        return val.ToString() ?? "";
    }

    private static CellStyle GetHeaderStyle(TablePrintColAttribute? attr) => new()
    {
        BackgroundColor = (int?)attr?.HeaderBgColor != -1 ? attr?.HeaderBgColor : null,
        ForegroundColor = (int?)attr?.HeaderTextColor != -1 ? attr?.HeaderTextColor : null,
        TextAlignment = (int?)attr?.HeaderTextAlignment != -1 ? attr?.HeaderTextAlignment : null
    };


    private static CellStyle GetCellStyle(TablePrintColAttribute? attr)
    {
        var style = new CellStyle();
        if (attr != null)
        {
            if ((int)attr.CellBgColor != -1) style.BackgroundColor = attr.CellBgColor;
            if ((int)attr.CellTextColor != -1) style.ForegroundColor = attr.CellTextColor;
            if ((int)attr.CellTextAlignment != -1) style.TextAlignment = attr.CellTextAlignment;
        }
        return style;
    }


}