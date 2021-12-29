using System;
using System.Collections.Generic;
using System.Windows.Forms;

public enum Cell : byte
{
    Empty,
    AgentPiece,
    HumanPiece,
}

public enum Player : byte
{
    Agent,
    Human
}

public static class PlayerExtensions
{
    public static Player Not(this Player player)
    {
        if (player == Player.Agent)
            return Player.Human;
        return Player.Agent;
    }
}

public interface IPlayer
{
    Cell GetCell();
    Player GetPiece();

    int Deliberate(Board board);
}

sealed class Agent : IPlayer
{
    private int depthLimit = 4;

    public Agent()
    {

    }

    public int Deliberate(Board board)
    {
        int maxMove = 0;
        int maxEvaluation = int.MinValue;
        Board maxChild = null;
        
        foreach (var (move, child) in board.Sucessors(Cell.AgentPiece))
        {
            int evaluation = Evaluate(child);
            if (evaluation > maxEvaluation)
            {
                maxEvaluation = evaluation;
                maxChild = child;
                maxMove = move;
            }
        }

        MessageBox.Show($"Placing on {maxMove}\n" + maxChild?.ToString());
        return maxMove;
    }

    private int Evaluate(Board board)
    {
        Cell won = board.HasWon();
        int sign(Cell cell) => (cell == Cell.HumanPiece) ? -1 : 1;
        if (won == Cell.AgentPiece)
            return int.MaxValue;

        if (won == Cell.HumanPiece)
            return int.MinValue;

        // Iterate through the cells of the board
        // to find near-winning streaks.
        // Scores for higher winning streaks are highly preferable (or
        // undesireable if they belong to the human), so their
        // scores are multiplied.

        int score = 0;

        for (int row = board.Rows - 1; row >= 0; --row)
        {
            for (int col = 0; col < board.Columns; ++col)
            {
                int streak = 0;
                int nextRow, nextCol;
                Cell origin = board.Cells[row, col];
                

                if (origin == Cell.Empty) 
                    continue;

                // Check for horizontal streaks.
                for (nextCol = col; nextCol < board.Columns; ++nextCol)
                {
                    if (board.Cells[row, nextCol] == origin)
                        streak += 1;
                    else break;
                }

                score += sign(origin) * (int)Math.Pow(10, streak);

                // Check vertical streaks.
                streak = 0;
                for (nextRow = row; nextRow >= 0; --nextRow)
                {
                    if (board.Cells[nextRow, col] == origin)
                        streak += 1;
                    else break;
                }

                score += sign(origin) * (int)Math.Pow(10, streak);

                // Check (left) diagonal streaks.
                streak = 0;

                nextRow = row;
                nextCol = col;

                while (nextRow >= 0 && nextCol >= 0)
                {
                    if (board.Cells[nextRow, nextCol] == origin)
                        streak += 1;
                    nextRow -= 1;
                    nextCol -= 1;
                }

                score += sign(origin) * (int)Math.Pow(10, streak);
                
                // Check (right) diagonal streaks.
                streak = 0;
                nextRow = row;
                nextCol = col;

                while (nextRow >= 0 && nextCol < board.Columns)
                {
                    if (board.Cells[nextRow, nextCol] == origin)
                        streak += 1;
                    nextRow -= 1;
                    nextCol += 1;
                }

                score += sign(origin) * (int)Math.Pow(10, streak);
            }
        }

        return score;
    }

    public Cell GetCell() => Cell.AgentPiece;
    public Player GetPiece() => Player.Agent;
}

sealed class Human : IPlayer
{
    public int Deliberate(Board board)
    {
        throw new NotImplementedException();
    }

    public Cell GetCell() => Cell.HumanPiece;
    public Player GetPiece() => Player.Human;
}

