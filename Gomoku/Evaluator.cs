using System;
using System.Collections.Generic;

namespace Gomoku
{
    struct Score
    {
        public bool Five;
        public uint Four;
        public uint Three;
        public uint Two;
        public uint One;
    }
    
    public class Evaluator
    {
        public double Evaluate(Board board)
        {
            Tuple<Score, Score> scores = WinChains(board);
            if (scores.Item1.Five)
                return Double.MaxValue;
            if (scores.Item2.Five)
                return Double.MinValue;
            double opponentScore = scores.Item2.Four * 100 + scores.Item2.Three * 5 + scores.Item2.Two * 1 + scores.Item2.One * 1;
            double myselfScore = scores.Item1.Four * 100 + scores.Item1.Three * 5 + scores.Item1.Two * 2;
            double tmp = myselfScore - opponentScore;
            return tmp;
        }

        // Tuple Item1 is score of myself, Item2 is score of opponent
        private Tuple<Score, Score> WinChains(Board board)
        {
            Score opponentScore = new Score{Five = false, Four = 0, Three = 0, Two = 0, One =  0};
            Score myselfScore = new Score{Five = false, Four = 0, Three = 0, Two = 0, One = 0};
            
            for (int x = 0; x < board.Size; x++)
            {
                for (int y = 0; y < board.Size; y++)
                {
                    if (AroundEmpty(board, x, y))
                        continue;
                    FulfillScore(NewColStates(board, x, y), ref myselfScore, State.Myself);
                    FulfillScore(NewRowStates(board, x, y), ref myselfScore, State.Myself);
                    FulfillScore(NewDiagRightStates(board, x, y), ref myselfScore, State.Myself);
                    FulfillScore(NewDiagLeftStates(board, x, y), ref myselfScore, State.Myself);
                    FulfillScore(NewColStates(board, x, y), ref opponentScore, State.Opponent);
                    FulfillScore(NewRowStates(board, x, y), ref opponentScore, State.Opponent);
                    FulfillScore(NewDiagRightStates(board, x, y), ref opponentScore, State.Opponent);
                    FulfillScore(NewDiagLeftStates(board, x, y), ref opponentScore, State.Opponent);
                    if (myselfScore.Five || opponentScore.Five)
                        return new Tuple<Score, Score>(myselfScore, opponentScore);
                }
            }
            return new Tuple<Score, Score>(myselfScore, opponentScore);
        }

        private bool AroundEmpty(Board board, int x, int y)
        {
            if (x + 1 < board.Size && board.Map[x + 1, y] != State.Empty)
                return false;
            if (x + 1 < board.Size && y - 1 > 0 && board.Map[x + 1, y - 1] != State.Empty)
                return false;
            if (x + 1 < board.Size && y + 1 < board.Size && board.Map[x + 1, y + 1] != State.Empty)
                return false;
            if (x - 1 > 0 && board.Map[x - 1, y] != State.Empty)
                return false;
            if (x - 1 > 0 && y - 1 > 0 && board.Map[x - 1, y - 1] != State.Empty)
                return false;
            if (x - 1 > 0 && y + 1 < board.Size && board.Map[x - 1, y + 1] != State.Empty)
                return false;
            if (y - 1 > 0 &&  y - 1 > 0 && board.Map[x, y - 1] != State.Empty)
                return false;
            if (y - 1 > 0 && y + 1 < board.Size && board.Map[x, y + 1] != State.Empty)
                return false;
            return true;
        }

        private Score FulfillScore(List<State> list, ref Score score, State player)
        {
            var tmp = LongestChain(list, player);
            switch (tmp)
            {
                case 5:
                    score.Five = true;
                    break;
                case 4:
                    ++score.Four;
                    break;
                case 3:
                    ++score.Three;
                    break;
                case 2:
                    ++score.Two;
                    break;
                case 1:
                    ++score.One;
                    break;
            }
            return score;
        }

        private List<State> NewRowStates(Board board, int x, int y)
        {
            var list = new List<State>();
            for (int i = 0; i < 5; i++)
            {
                if (y + i >= board.Size)
                    break;
                list.Add(board.Map[x, y + i]);
            }
            return list;
        }
        
        private List<State> NewColStates(Board board, int x, int y)
        {
            var list = new List<State>();
            for (int i = 0; i < 5; i++)
            {
                if (x + i >= board.Size)
                    break;
                list.Add(board.Map[x + i, y]);
            }
            return list;
        }

        private List<State> NewDiagRightStates(Board board, int x, int y)
        {
            var list = new List<State>();
            for (int i = 0; i < 5; i++)
            {
                if (x + i >= board.Size || y + i >= board.Size)
                    break;
                list.Add(board.Map[x + i, y + i]);
            }
            return list;
        }

        private List<State> NewDiagLeftStates(Board board, int x, int y)
        {
            var list = new List<State>();
            for (int i = 0; i < 5; i++)
            {
                if (x + i >= board.Size || y - i < 0)
                    break;
                list.Add(board.Map[x + i, y - i]);
            }
            return list;
        }

        private int LongestChain(List<State> list, State player)
        {
            if (list == null)
                return 0;
            int count = 0;
            foreach (var tmp in list)
            {
                if (tmp == State.Empty)
                    continue;
                if (tmp == player)
                    ++count;
                else
                    return 0;
            }
            
            return count;
        }
    }
}