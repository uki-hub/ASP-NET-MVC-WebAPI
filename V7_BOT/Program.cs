using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V7_BOT
{
    class Program
    {
        static void Main(string[] args)
        {
            new GenerateHemTableClasses("V7_API_BASE.Models", false).generateHer();
        }
    }
}
