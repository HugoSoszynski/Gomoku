using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Gomoku {
    public class Core {

        protected static Serializer _serializer = new Serializer();
        protected static Board _board;
        protected static IAI _ia = new MinMax();
        protected static bool isRunning = true;

        private static readonly IDictionary<Commands.ECommand, Action<Object>> functionMappings = new Dictionary<Commands.ECommand, Action<Object>>
        {
            { Commands.ECommand.START, Start },
            { Commands.ECommand.TURN, Turn },
            { Commands.ECommand.BEGIN, Begin },
            { Commands.ECommand.BOARD, Board },
            { Commands.ECommand.INFO, Info },
            { Commands.ECommand.END, End },
            { Commands.ECommand.ABOUT, About }
        };


        public Core() {}

        public void Execute() {
            Read();
        }

        public static void Read() {
            while (isRunning) {
                Commands.DataCommand command = _serializer.Read();
                functionMappings[command.CommandType](command.Data);
            }
        }

        public static void Start(Object _object) {
            _board = new Board((uint)_object);
            _serializer.Send("OK");
        }

        public static void Turn(Object _object) {
            Tuple<uint, uint> tuple;

            try {
                _board.Play(((Tuple<uint, uint>)(_object)).Item1, ((Tuple<uint, uint>)(_object)).Item2, State.Opponent);
            }
            catch (IllegalMoveException e) {
                _serializer.Send("ERROR " + e.Message);
                return;
            }
            tuple = _ia.MakeMove(_board);
            _board.Play(tuple.Item1, tuple.Item2, State.Myself);
            _serializer.Send(tuple.Item1 + "," + tuple.Item2);
        }

        public static void Begin(Object _object) {
            Tuple<uint, uint> tuple;

            tuple = _ia.MakeMove(_board);
            _board.Play(tuple.Item1, tuple.Item2, State.Myself);
            _serializer.Send(tuple.Item1 + "," + tuple.Item2);
        }

        public static void Board(Object _object) {
            Tuple<uint, uint> tuple;
            
            _board.Init((List<Tuple <uint, uint, State>>)(_object));
            tuple = _ia.MakeMove(_board);
            _board.Play(tuple.Item1, tuple.Item2, State.Myself);
            _serializer.Send(tuple.Item1 + "," + tuple.Item2);
        }

        public static void Info(Object _object) {
        }

        public static void End(Object _object) {
            isRunning = false;
        }
        
        public static void About(Object _object) {
            _serializer.Send("name=\"SaltTeam\", version\"1.0\", author=\"SaltTeam\", country=\"France\"");
        }
    }
}