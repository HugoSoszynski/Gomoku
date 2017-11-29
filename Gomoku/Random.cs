using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku {
    public class RandomAI
        : IAI {
        public Tuple<uint, uint> MakeMove(Board board) {
            Random r = new Random();
            var moves = board.PossibleMoves();
            var move = moves[r.Next(moves.Count())];
            return move;
        }
    }
}
