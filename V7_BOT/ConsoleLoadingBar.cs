using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V7_BOT
{
    public class ConsoleLoadingBar
    {
        public static void LoadBar(int i, int max, int barCount = 50)
        {
            var loadedCount = (int)Math.Round(barCount * (double)(i + 1) / (double)max);
            int loadbgCount = barCount - loadedCount;
            var loaded = String.Join("█", Enumerable.Repeat<string>("", loadedCount));
            var loadedbg = String.Join("░", Enumerable.Repeat<string>("", loadbgCount));
            Console.WriteLine($"{loaded}{loadedbg}");
        }
    }
}
