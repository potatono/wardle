using System;
using System.Drawing;

namespace Wardle
{
    class Program
    {
        static void Main(string[] args)
        {
            Map map = new Map();
            map.Open("sample.map");

            ConsoleUI ui = new ConsoleUI(map);
            ui.Run();
        }
    }
}
