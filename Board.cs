using System;

public class Board
{
    public int Rows { get; }
    public int Columns { get; }
    public int WinningStreak { get; }
    public Cell[,] Cells { get; }
    private int movesPlayed;

    public Board(int rows = 6, int columns = 7, int winningStreak = 4)
    {
        // The minimum dimensions required
        // to play Connect-Four is 5x4.
        movesPlayed = 0;
        Rows = Math.Max(rows, 4);
        Columns = Math.Max(columns, 5);
        WinningStreak = Math.Max(winningStreak, 4);
        Cells = new Cell[Rows, Columns];
        
        for (int row = 0; row < Rows; ++row)
        {
            for (int col = 0; col < Columns; ++col)
            {
                Cells[row, col] = Cell.Empty;
            }
        }
    }

    public bool MakeMove(Player player, int column)
    {
        // ^ True = Player won, False = Player did not win using this move.

        // Ensure column is not full.
        // Ensure 0 <= column <= Columns
        ++movesPlayed;

        // Find the row closest to the bottom.
        int row;

        // Put the player piece in the cells of the board.

        // Check if the player won with the most recent move
        return HasWon(player, row, column);
    }

    private bool HasWon(Player player, int mostRecentRow, int mostRecentColumn)
    {
        // Check Rows:
        // (Since rows are filled from bottom to top, it's
        // faster to search for a winning move from the bottom.)

        // Check Diagonals
    }
}
