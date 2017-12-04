using System;
using System.Text.RegularExpressions;

namespace Gomoku.Commands.Classes
{
    public class Start : ACommand
    {
        public Start()
            :base(ECommand.START)
        {}

        public override DataCommand CreateDataCommand(string input)
        {
            Regex mRegex = new Regex("START (\\d+)");
            Match match = mRegex.Match(input);
            if (match.Success)
                return new DataCommand {CommandType = Type, Data = Convert.ToUInt32(match.Groups[1].Value)};
            return new DataCommand {CommandType = ECommand.ERROR, Data = null};
        }
    }
}