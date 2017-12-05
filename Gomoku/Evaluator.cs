using System;

namespace Gomoku
{
    struct Score
    {
        public bool Five;
        public uint Four;
        public uint Three;
        public uint Two;
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
            double opponentScore = scores.Item2.Four * 80 + scores.Item2.Three * 3 + scores.Item2.Two * 0;
            double myselfScore = scores.Item1.Four * 100 + scores.Item1.Three * 5 + scores.Item1.Two * 1;
            double tmp = myselfScore - opponentScore;
            return tmp;
        }

        // Tuple Item1 is score of myself, Item2 is score of opponent
        private Tuple<Score, Score> WinChains(Board board)
        {
            Score opponentScore = new Score{Five = false, Four = 0, Three = 0, Two = 0};
            Score myselfScore = new Score{Five = false, Four = 0, Three = 0, Two = 0};
            
            for (int x = 0; x < board.Size; x++)
            {
                for (int y = 0; y < board.Size; y++)
                {
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

        private Score FulfillScore(State[] array, ref Score score, State player)
        {
            var tmp = LongestChain(array, player);
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
            }
            return score;
        }

        private State[] NewRowStates(Board board, int x, int y)
        {
            if (y + 4 >= board.Size) 
                return null;
            return new []
            {
                board.Map[x, y],
                board.Map[x, y + 1],
                board.Map[x, y + 2],
                board.Map[x, y + 3],
                board.Map[x, y + 4]
            };
        }
        
        private State[] NewColStates(Board board, int x, int y)
        {
            if (x + 4 >= board.Size) 
                return null;
            return new []
            {
                board.Map[x, y],
                board.Map[x + 1, y],
                board.Map[x + 2, y],
                board.Map[x + 3, y],
                board.Map[x + 4, y]
            };
        }

        private State[] NewDiagRightStates(Board board, int x, int y)
        {
            if (y + 4 >= board.Size || x + 4 >= board.Size) 
                return null;
            return new []
            {
                board.Map[x, y],
                board.Map[x + 1, y + 1],
                board.Map[x + 2, y + 2],
                board.Map[x + 3, y + 3],
                board.Map[x + 4, y + 4]
            };
        }

        private State[] NewDiagLeftStates(Board board, int x, int y)
        {
            if (y - 4 < 0 || x + 4 >= board.Size) 
                return null;
            return new []
            {
                board.Map[x, y],
                board.Map[x + 1, y - 1],
                board.Map[x + 2, y - 2],
                board.Map[x + 3, y - 3],
                board.Map[x + 4, y - 4]
            };
        }

        private int LongestChain(State[] array, State player)
        {
            if (array == null)
                return 0;
            int count = 0;
            foreach (var tmp in array)
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