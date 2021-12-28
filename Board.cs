using System;
using System.Diagnostics;

public class Board
{
    public int Rows { get; }
    public int Columns { get; }
    public int WinningStreak { get; }
    public Cell[,] Cells { get; }
    public int MovesPlayed;

    public Board(int rows = 6, int columns = 7, int winningStreak = 4)
    {
        // The minimum dimensions required
        // to play Connect-Four is 5x4.
        MovesPlayed = 0;
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

    public bool MakeMove(IPlayer player, int column)
    {
        // ^ True = Player won, False = Player did not win using this move.
        // Ensure column is not full
        // Ensure 0 <= column <= Columns
        Debug.Assert(0 <= column && column < Columns);

        ++MovesPlayed;

        // Find the row closest to the bottom.
        int row;
        for (row = Rows - 1; row >= 0; --row)
        {
            if (Cells[row, column] == Cell.Empty)
            {
                // Put the player piece in the cells of the board
                Cells[row, column] = player.GetCell();
                break;
            }
        }

        // Check if the player won with the most recent move
        return HasWon(player, row, column);
    }

    private bool HasWon(IPlayer player, int mostRecentRow, int mostRecentColumn)
    {

        return CheckHorizontal(player, mostRecentRow, mostRecentColumn)
            || CheckVertical(player, mostRecentRow, mostRecentColumn)
            || CheckRightDiagonal(player, mostRecentRow, mostRecentColumn)
            || CheckLeftDiagonal(player, mostRecentRow, mostRecentColumn);
    }
    private bool CheckHorizontal(IPlayer player, int mostRecentRow, int mostRecentColumn)
    {
        return false;
    }

    private bool CheckVertical(IPlayer player, int mostRecentRow, int mostRecentColumn)
    {
        return false;
    }

    private bool CheckRightDiagonal(IPlayer player, int mostRecentRow, int mostRecentColumn)
    {
        return false;
    }

    private bool CheckLeftDiagonal(IPlayer player, int mostRecentRow, int mostRecentColumn)
    {
        return false;
    }
}