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

    public class Board {

        public uint Size { get; private set; }
        public State[,] Map = null;

        public Board(uint size) {
            Size = size;
            Map = new State[size, size];
            this.Zero();
        }

        public Board(Board other) {
            this.Size = other.Size;
            this.Map = new State[Size, Size];
            for (var x = 0; x < Size; ++x) {
                for (var y = 0; y < Size; ++y) {
                    this.Map[x, y] = other.Map[x, y];
                }
            }
        }

        public void Init(List<Tuple<uint, uint, State>> init) {
            this.Zero();
            foreach (var i in init) {
                Map[i.Item1, i.Item2] = i.Item3;
            }
        }

        public void Zero() {
            for (var x = 0; x < Size; ++x) {
                for (var y = 0; y < Size; ++y) {
                    Map[x, y] = State.Empty;
                }
            }
        }

        public void Play(uint x, uint y, State state) {
            if (x >= Size || y >= Size || x < 0 || y < 0)
                throw new IllegalMoveException("[" + x + "," + y + "]: Out of the board");
            if (Map[x, y] != State.Empty)
                throw new IllegalMoveException("[" + x + "," + y + "]: Not Empty");
            Map[x, y] = state;
        }

        public void Unplay(uint x, uint y) {
            if (x >= Size || y >= Size || x < 0 || y < 0)
                throw new IllegalMoveException("[" + x + "," + y + "]: Out of the board");
            Map[x, y] = State.Empty;
        }

        public List<Tuple<uint, uint>> PossibleMoves() {
            List<Tuple<uint, uint>> moves = new List<Tuple<uint, uint>>();

            for (uint x = 0; x < Size; ++x) {
                for (uint y = 0; y < Size; ++y) {
                    if (Map[x, y] == State.Empty)
                        moves.Add(new Tuple<uint, uint>(x, y));
                }
            }
            return moves;
        }

        public bool Empty() {
            foreach (var c in Map) {
                if (c != State.Empty)
                    return false;
            }
            return true;
        }
    }
}