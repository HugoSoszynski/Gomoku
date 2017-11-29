using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Gomoku.Commands;

namespace Gomoku
{
    public class Serializer {

        public Serializer() {}

        public DataCommand Read()
        {
            while (true)
            {
                var tmp = CommandList.From(Console.ReadLine());
                if (tmp.CommandType == ECommand.UNKNOWN)
                    Console.Out.WriteLine("UNKNOWN");
                else
                    return tmp;
            }
        }

        public void Send(string output)
        {
            Console.Out.WriteLine(output);
        }
    }
}