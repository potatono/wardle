using System;
using System.Drawing;

namespace Wardle
{
    class Program
    {
        static void Main(string[] args)
        {
            Map.OpenAsCurrent("sample.map");

            ConsoleUI ui = new ConsoleUI();
            ui.Run();
        }
    }
}
