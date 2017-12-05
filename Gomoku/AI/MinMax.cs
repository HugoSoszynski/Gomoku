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
        private uint MaxDepth = 1;
        public bool Finished = false;
        private uint Offset = 0;

        public Tuple<uint, uint> MakeMove(Board board) {
            if (board.Empty())
                return new Tuple<uint, uint>(board.Size / 2, board.Size / 2);
            InitBoard(board);
            var res = DecideMove();
            res = new Tuple<uint, uint>(res.Item1 + Offset, res.Item2 + Offset);
            Map = null;
            Offset = 0;
            Finished = false;
            return res;
        }

        public Tuple<uint, uint> MakeMove(Board board, uint depth) {
            MaxDepth = depth;
            var res = MakeMove(board);
            MaxDepth = 2;
            return res;
        }

        private void InitBoard(Board board) {
            uint off;
            uint eoff = 0;

            for (off = 0; off < board.Size; ++off) {
                if (CheckBorderUp(board, off))
                    break;
            }
            if (off > 0)
                --off;
            for (eoff = board.Size - 1; eoff >= 0; --eoff) {
                if (CheckBorderDown(board, eoff))
                    break;
            }
            if (eoff < board.Size - 1)
                ++eoff;
            Offset = off;
            Map = new Board(eoff - off + 1);

            for (int i = 0; i < Map.Size; i++)
                for (int j = 0; j < Map.Size; j++)
                    Map.Map[i, j] = board.Map[Offset + i, Offset + j];
        }

        private bool CheckBorderUp(Board board, uint offset) {
            for (var x = offset; x < board.Size; ++x) {
                if (board.Map[x, offset] != State.Empty)
                    return true;
            }
            for (var y = offset; y < board.Size; ++y) {
                if (board.Map[offset, y] != State.Empty)
                    return true;
            }
            return false;
        }

        private bool CheckBorderDown(Board board, uint offset) {
            for (var x = offset; x > 0; --x) {
                if (board.Map[x, offset] != State.Empty)
                    return true;
            }
            for (var y = offset; y > 0; --y) {
                if (board.Map[offset, y] != State.Empty)
                    return true;
            }
            return false;
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
                if (currentScore == double.MaxValue) {
                    Map.Unplay(move.Item1, move.Item2);
                    return move;
                }
                if (best == null || bestScore < currentScore) {
                    best = move;
                    bestScore = currentScore;
                }
                Map.Unplay(move.Item1, move.Item2);
                if (Finished)
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
                if (score == double.MaxValue) {
                    Map.Unplay(move.Item1, move.Item2);
                    return score;
                }
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
                if (score == double.MaxValue) {
                    Map.Unplay(move.Item1, move.Item2);
                    return score;
                }
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
