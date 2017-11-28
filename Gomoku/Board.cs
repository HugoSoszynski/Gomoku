using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku
{
    public class IllegalMoveException : Exception {
        public IllegalMoveException() 
            : base("Illegal Move") {
        }

        public IllegalMoveException(string message)
            : base("Illegal Move: " + message) {
        }

        public IllegalMoveException(string message, Exception inner)
            : base("Illegal Move: " + message, inner) {
        }
    }

    public enum State : byte {
        Empty = 0,
        Myself,
        Opponent
    }

    public struct BoardInit {
        public uint X;
        public uint Y;
        public State State;
    }

    public class Board {

        public uint Size { get; private set; }
        private State[,] Map = null;

        public Board(uint size) {
            Size = size;
            Map = new State[size, size];
            for (var x = 0; x < size; ++x) {
                for (var y = 0; y < size; ++y) {
                    Map[x, y] = State.Empty;
                }
            }
        }

        public void BoardInit(List<BoardInit> init) {
            foreach (var i in init) {
                Map[i.X, i.Y] = i.State;
            }
        }

        public void Play(uint x, uint y, State state) {
            if (x >= Size || y >= Size || x < 0 || y < 0)
                throw new IllegalMoveException("[" + x + "," + y + "]: Out of the board");
            if (Map[x, y] != State.Empty)
                throw new IllegalMoveException("[" + x + "," + y + "]: Not Empty");
            Map[x, y] = state;
        }
    }
}
