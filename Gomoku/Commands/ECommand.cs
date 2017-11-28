using System;
using System.Collections.Generic;
using System.Linq;
using Gomoku.Commands.Classes;

namespace Gomoku.Commands
{
    public enum ECommand : byte
    {
        UNKNOWN,
        ABOUT,
        BEGIN,
        BOARD,
        END,
        INFO,
        START,
        TURN
    }
    
    public static class CommandDictionary
    {
        public static readonly List<ACommand> CommandMap = new List<ACommand>();
        
        private static readonly ACommand About = new About();
        private static readonly ACommand Begin = new Begin();
        private static readonly ACommand Board = new CommandBoard();
        private static readonly ACommand End = new End();
        private static readonly ACommand Info = new Info();
        private static readonly ACommand Start = new Start();
        private static readonly ACommand Turn = new Turn();

        private static bool Compare(string input, ECommand type)
        {
            switch (type)
            {
                case ECommand.START:
                case ECommand.TURN:
                case ECommand.INFO:
                    return input.StartsWith(type + " ");
                case ECommand.BOARD:
                case ECommand.END:
                case ECommand.BEGIN:
                case ECommand.ABOUT:
                    return input.StartsWith(type + "\r\n");
                default:
                    return false;
            }
        }
        
        public static DataCommand From(string input)
        { return (from command in CommandMap where Compare(input, command.Type) select command.CreateDataCommand(input)).FirstOrDefault(); }
    }
}