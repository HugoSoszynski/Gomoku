using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku
{
    class Program
    {
        static void Main(string[] args)
        {
            Core core = new Core();

            try {
                core.Execute();
            } catch (Exception e) {
                Console.Out.WriteLine(e);
                Console.ReadLine();
            }
        }
    }
}
