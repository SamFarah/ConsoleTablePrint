using System.Text;

namespace ConsoleTablesPrinter.Helpers;
internal static class ConsoleStateManager
{
    internal static void WithConsoleState(Action action, bool requiresUtf8)
    {
        var originalEncoding = Console.OutputEncoding;
        var originalBg = Console.BackgroundColor;
        var originalFg = Console.ForegroundColor;
        var originalCursor = true;

        try
        {
            if (requiresUtf8)
                Console.OutputEncoding = Encoding.UTF8;

            if (OperatingSystem.IsWindows())
                originalCursor = Console.CursorVisible;

            Console.CursorVisible = false;

            action();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(ex);
        }
        finally
        {
            Console.OutputEncoding = originalEncoding;
            Console.BackgroundColor = originalBg;
            Console.ForegroundColor = originalFg;
            if (OperatingSystem.IsWindows()) Console.CursorVisible = originalCursor;
        }
    }
}
