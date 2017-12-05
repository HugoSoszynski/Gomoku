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
            for (;;)
            {
                var tmp = CommandList.From(Console.ReadLine());
                switch (tmp.CommandType)
                {
                    case ECommand.UNKNOWN:
                    case ECommand.ERROR:
                        Console.Out.WriteLine(tmp.CommandType.ToString());
                        break;
                    default:
                        return tmp;
                }
            }
        }

        public void Send(string output)
        {
            Console.Out.WriteLine(output);
        }
    }
}