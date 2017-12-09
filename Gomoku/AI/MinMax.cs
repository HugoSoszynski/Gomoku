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
        private List<Tuple<uint, uint>> PossibleMoves = null;

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
                if (CheckBorderDown(board, eoff, off))
                    break;
            }
            if (eoff < board.Size - 1)
                ++eoff;
            Offset = off;
            Map = new Board(eoff - off + 1, board.MaxSize, off);

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

        private bool CheckBorderDown(Board board, uint eoffset, uint offset)
        {
            if (eoffset == offset)
                return true;
            for (var x = eoffset; x > offset; --x) {
                if (board.Map[x, eoffset] != State.Empty)
                    return true;
            }
            for (var y = eoffset; y > offset; --y) {
                if (board.Map[eoffset, y] != State.Empty)
                    return true;
            }
            return false;
        }

        private Tuple<uint, uint> DecideMove() {
            Tuple<uint, uint> best = null;
            double bestScore = double.MinValue;
            PossibleMoves = Map.PossibleMoves();
            double currentScore = 0;
            var depth = MaxDepth;

            if (PossibleMoves.Count() == 1)
                return PossibleMoves.ElementAt(0);
            RemoveUselessMoves();
            var moves = new List<Tuple<uint, uint>>(PossibleMoves);
            foreach (var move in moves) {
                Map.Play(move.Item1, move.Item2, State.Myself);
                PossibleMoves.Remove(move);
                currentScore = MinimiseMove(depth, double.MinValue, double.MaxValue, move);
                if (currentScore == double.MaxValue) {
                    PossibleMoves.Add(move);
                    Map.Unplay(move.Item1, move.Item2);
                    return move;
                }
                if (best == null || bestScore < currentScore) {
                    best = move;
                    bestScore = currentScore;
                }
                PossibleMoves.Add(move);
                Map.Unplay(move.Item1, move.Item2);
                if (Finished)
                    break;
            }
            return best;
        }

        private void RemoveUselessMoves() {
            var moves = new List<Tuple<uint, uint>>();

            foreach (var move in PossibleMoves) {
                if (CheckAround(move))
                    moves.Add(move);
            }
            PossibleMoves = moves;
        }

        private bool CheckAround(Tuple<uint, uint> move) {
            for (var x = -2; x < 3; ++x) {
                if (x < 0 && Math.Abs(x) > move.Item1)
                    continue;
                if (move.Item1 + x >= Map.Size)
                    continue;
                for (var y = -2; y < 3; ++y) {
                    if (y < 0 && Math.Abs(y) > move.Item2)
                        continue;
                    if (move.Item2 + y >= Map.Size)
                        continue;
                    if (x == 0 && y == 0)
                        continue;
                    if (Map.Map[move.Item1 + x, move.Item2 + y] != State.Empty)
                        return true;
                }
            }
            return false;
        }

        private double MinimiseMove(uint depth, double alpha, double beta, Tuple<uint, uint> madeMove) {
            double res;

            Evaluator eval = new Evaluator();
            res = eval.Evaluate(Map);
            if (depth == 0 || res == double.MaxValue || res == double.MinValue) {
                return res;
            }

            res = double.MaxValue;
            var moves = new List<Tuple<uint, uint>>(PossibleMoves);
            foreach (var move in moves) {
                PossibleMoves.Remove(move);
                Map.Play(move.Item1, move.Item2, State.Opponent);
                double score = MaximiseMove(depth - 1, alpha, beta, move);
                if (score == double.MaxValue) {
                    PossibleMoves.Add(move);
                    Map.Unplay(move.Item1, move.Item2);
                    return score;
                }
                PossibleMoves.Add(move);
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
            var moves = new List<Tuple<uint, uint>>(PossibleMoves);
            foreach (var move in moves) {
                PossibleMoves.Remove(move);
                Map.Play(move.Item1, move.Item2, State.Myself);
                double score = MinimiseMove(depth - 1, alpha, beta, move);
                if (score == double.MaxValue) {
                    PossibleMoves.Add(move);
                    Map.Unplay(move.Item1, move.Item2);
                    return score;
                }
                PossibleMoves.Add(move);
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
