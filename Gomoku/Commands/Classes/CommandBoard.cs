using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Gomoku.Commands.Classes
{
    public class CommandBoard : ACommand
    {
        public CommandBoard()
            :base(ECommand.BOARD)
        {}

        public override DataCommand CreateDataCommand(string input)
        {
            Regex mRegex = new Regex("(BOARD|\\d{1,2},\\d{1,2},[0-2]|DONE)");
            MatchCollection matches = mRegex.Matches(input);
            if (matches.Count > 1 &&
                matches[matches.Count - 1].Value.Equals("DONE") &&
                matches[0].Value.Equals("BOARD"))
            {
                var list = new List<Tuple<uint, uint, State>>();
                for (int i = 1; i < matches.Count - 1; i++)
                    try
                    {
                        var nbrSplit = matches[i].Value.Split(',');
                        list.Add(new Tuple<uint, uint, State>(Convert.ToUInt32(nbrSplit[0]), Convert.ToUInt32(nbrSplit[1]), (State)Convert.ToInt16(nbrSplit[2])));
                    }
                    catch (Exception e)
                    {
                        return new DataCommand {CommandType = ECommand.ERROR, Data = null};
                    }
                return new DataCommand {CommandType = Type, Data = list};
            }
            return new DataCommand {CommandType = ECommand.ERROR, Data = null};
        }
    }
}