using System;
using System.Text.RegularExpressions;

namespace Gomoku.Commands.Classes
{
    public class Turn : ACommand
    {
        public Turn()
            :base(ECommand.TURN)
        {}

        public override DataCommand CreateDataCommand(string input)
        {
            Regex mRegex = new Regex("TURN (\\d{1,2}),(\\d{1,2})");
            Match match = mRegex.Match(input);
            if (match.Success)
                return new DataCommand {CommandType = Type, Data = new Tuple<uint, uint>(Convert.ToUInt32(match.Groups[1].Value), Convert.ToUInt32(match.Groups[2].Value))};
            return new DataCommand {CommandType = ECommand.ERROR, Data = null};
        }
    }
}