using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku {
    public class Random
        : IAI {
        public Tuple<uint, uint> MakeMove(Board board) {
            return board.PossibleMoves().First();
        }
    }
}
