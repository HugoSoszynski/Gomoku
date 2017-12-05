using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gomoku {
    public class MinMax
        : IAI {
        private Board Map = null;
        private uint MaxDepth = 5;
        public bool finished = false;

        public Tuple<uint, uint> MakeMove(Board board) {
            Map = new Board(board);
            var res = DecideMove();
            Map = null;
            return res;
        }

        public Tuple<uint, uint> MakeMove(Board board, uint depth) {
            Map = new Board(board);
            MaxDepth = depth;
            var res = DecideMove();
            Map = null;
            MaxDepth = 5;
            return res;
        }

        private Tuple<uint, uint> DecideMove() {
            Tuple<uint, uint> best = null;
            double bestScore = double.MinValue;
            var moves = Map.PossibleMoves();
            double currentScore = 0;
            var depth = MaxDepth;

            if (moves.Count() == 1)
                return moves.ElementAt(0);
            foreach (var move in moves) {
                Map.Play(move.Item1, move.Item2, State.Myself);
                currentScore = MinimiseMove(depth, double.MinValue, double.MaxValue, move);
                if (best == null || bestScore < currentScore) {
                    best = move;
                    bestScore = currentScore;
                }
                Map.Unplay(move.Item1, move.Item2);
                if (finished)
                    break;
            }
            return best;
        } 

        private double MinimiseMove(uint depth, double alpha, double beta, Tuple<uint, uint> madeMove) {
            double res;

            Evaluator eval = new Evaluator();
            res = eval.Evaluate(Map);
            if (depth == 0 || res == double.MaxValue || res == double.MinValue) {
                return res;
            }

            res = double.MaxValue;
            var moves = Map.PossibleMoves();
            foreach (var move in moves) {
                Map.Play(move.Item1, move.Item2, State.Opponent);
                double score = MaximiseMove(depth - 1, alpha, beta, move);
                Map.Unplay(move.Item1, move.Item2);
                res = Math.Min(res, score);
                beta = Math.Min(beta, score);
                if (beta <= alpha)
                    break;
            }
            return res;
        }

        private double MaximiseMove(uint depth, double alpha, double beta, Tuple<uint, uint> madeMove) {
            double res;

            Evaluator eval = new Evaluator();
            res = eval.Evaluate(Map);
            if (depth == 0 || res == double.MaxValue || res == double.MinValue) {
                return res;
            }

            res = double.MinValue;
            var moves = Map.PossibleMoves();
            foreach (var move in moves) {
                Map.Play(move.Item1, move.Item2, State.Myself);
                double score = MinimiseMove(depth - 1, alpha, beta, move);
                Map.Unplay(move.Item1, move.Item2);
                res = Math.Max(res, score);
                alpha = Math.Max(alpha, score);
                if (beta <= alpha)
                    break;
            }
            return res;
        }
    }
}
