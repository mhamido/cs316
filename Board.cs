using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

public class Board
{
    public int Rows { get; }
    public int Columns { get; }
    public int WinningStreak { get; }
    public Cell[,] Cells { get; }
    public int MovesPlayed;
    public Board parent;

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

    public Board(Board that)
    {
        this.Rows = that.Rows;
        this.MovesPlayed = that.MovesPlayed;
        this.Columns = that.Columns;
        this.WinningStreak = that.WinningStreak;
        this.Cells = that.Cells.Clone() as Cell[,];
    }

    public IEnumerable<(int, Board)> Sucessors(Cell cell)
    {
        (int, Board) placeChild(int col)
        {
            for (int row = Rows - 1; row >= 0; --row)
            {
                if (Cells[row, col] == Cell.Empty)
                {
                    Board child = new Board(this);
                    child.Cells[row, col] = cell;
                    return (col, child);
                }
            }
            // There is no empty row in the column of interest
            // this should not occur under normal circumstances.
            return (0, this);
        }

        int len = Columns - 1;
        int right = Columns / 2;
        int left = Math.Max(right - 1, 0);

        while (left >= 0 || right <= len)
        {
            if (right <= len)
            {
                yield return placeChild(right++);
            }

            if (left >= 0)
            {
                yield return placeChild(left--);
            }
        }
    }
    public void MakeMove(IPlayer player, int column)
    {
        // ^ True = Player won, False = Player did not win using this move.
        // Ensure column is not full
        // Ensure 0 <= column <= Columns
        Debug.Assert(0 <= column && column < Columns);

        ++MovesPlayed;

        // Find the row closest to the bottom.
        int row;
        bool ok = false;
        for (row = Rows - 1; row >= 0; --row)
        {
            if (Cells[row, column] == Cell.Empty)
            {
                // Put the player piece in the cells of the board
                Cells[row, column] = player.GetCell();
                ok = true;
                break;
            }

        }
        if (!ok)
            throw new IndexOutOfRangeException();
        // Check if the player won with the most recent move
        return;
    }

    public Cell HasWon(int mostRecentRow, int mostRecentColumn)
    {

        Cell cell = CheckHorizontal(mostRecentRow, mostRecentColumn);
        if (cell != Cell.Empty)
            return cell;
        cell = CheckVertical(mostRecentRow, mostRecentColumn);
        if (cell != Cell.Empty)
            return cell;
        cell = CheckRightDiagonal(mostRecentRow, mostRecentColumn);
        if (cell != Cell.Empty)
            return cell;
        cell = CheckLeftDiagonal(mostRecentRow, mostRecentColumn);
        if (cell != Cell.Empty)
            return cell;

        return cell;
    }

    public Cell HasWon()
    {
        for (int row = Rows - 1; row >= 0; --row)
        {
            for (int col = 0; col < Columns; ++col)
            {
                return HasWon(row, col);
            }
        }

        return Cell.Empty;
    }

    private Cell CheckHorizontal(int mostRecentRow, int mostRecentColumn)
    {

        int r = mostRecentRow;
        int c = mostRecentColumn;
        if (Cells[r, c] == Cell.HumanPiece)
        {
            int Hstreak = 0;

            for (c = mostRecentColumn; c < Columns; c++)
            {
                if (Cells[r, c] == Cell.HumanPiece)
                {
                    Hstreak++;
                    if (Hstreak == WinningStreak)
                    {
                        return Cell.HumanPiece;
                    }
                }
                else { break; }
            }

            for (c = mostRecentColumn - 1; c >= 0; c--)
            {
                if (Cells[r, c] == Cell.HumanPiece)
                {
                    Hstreak++;
                    if (Hstreak == WinningStreak)
                    {
                        return Cell.HumanPiece;
                    }
                }
                else { break; }
            }


        }
        else if (Cells[r, c] == Cell.AgentPiece)
        {
            int Astreak = 0;
            for (c = mostRecentColumn; c < Columns; c++)
            {
                if (Cells[r, c] == Cell.AgentPiece)
                {
                    Astreak++;
                    if (Astreak == WinningStreak)
                    {
                        return Cell.AgentPiece;
                    }
                }
                else { break; }
            }

            for (c = mostRecentColumn - 1; c >= 0; c--)
            {
                if (Cells[r, c] == Cell.AgentPiece)
                {
                    Astreak++;
                    if (Astreak == WinningStreak)
                    {
                        return Cell.AgentPiece;
                    }
                }
                else { break; }
            }

        }
        return Cell.Empty;
    }

    private Cell CheckVertical(int mostRecentRow, int mostRecentColumn)
    {
        int r = mostRecentRow;
        int c = mostRecentColumn;
        if (Cells[r, c] == Cell.HumanPiece)
        {
            int Hstreak = 0;

            for (r = mostRecentRow; r < Rows; r++)
            {
                if (Cells[r, c] == Cell.HumanPiece)
                {
                    Hstreak++;
                    if (Hstreak == WinningStreak)
                    {
                        return Cell.HumanPiece;
                    }
                }
                else { break; }
            }

            for (r = mostRecentRow - 1; r >= 0; r--)
            {
                if (Cells[r, c] == Cell.HumanPiece)
                {
                    Hstreak++;
                    if (Hstreak == WinningStreak)
                    {
                        return Cell.HumanPiece;
                    }
                }
                else { break; }
            }


        }
        else if (Cells[r, c] == Cell.AgentPiece)
        {
            int Astreak = 0;
            for (r = mostRecentRow; r < Rows; r++)
            {
                if (Cells[r, c] == Cell.AgentPiece)
                {
                    Astreak++;
                    if (Astreak == WinningStreak)
                    {
                        return Cell.AgentPiece;
                    }
                }
                else { break; }
            }

            for (r = mostRecentRow - 1; r >= 0; r--)
            {
                if (Cells[r, c] == Cell.AgentPiece)
                {
                    Astreak++;
                    if (Astreak == WinningStreak)
                    {
                        return Cell.AgentPiece;
                    }
                }
                else { break; }
            }

        }
        return Cell.Empty;

    }

    private Cell CheckRightDiagonal(int mostRecentRow, int mostRecentColumn)
    {
        int r = mostRecentRow;
        int c = mostRecentColumn;
        if (Cells[r, c] == Cell.HumanPiece)
        {
            int Hstreak = 0;

            for (r = mostRecentRow; r < Rows && c >= 0; r++)
            {
                if (Cells[r, c] == Cell.HumanPiece)
                {
                    Hstreak++;
                    if (Hstreak == WinningStreak)
                    {
                        return Cell.HumanPiece;
                    }
                    c--;
                }
                else { break; }
            }
            for (r = mostRecentRow - 1, c = mostRecentColumn + 1; r >= 0 && c < Columns; r--)
            {
                if (Cells[r, c] == Cell.HumanPiece)
                {
                    Hstreak++;
                    if (Hstreak == WinningStreak)
                    {
                        return Cell.HumanPiece;
                    }
                    c++;
                }
                else { break; }

            }
        }
        else if (Cells[r, c] == Cell.AgentPiece)
        {
            int Astreak = 0;
            for (r = mostRecentRow; r < Rows && c >= 0; r++)
            {
                if (Cells[r, c] == Cell.AgentPiece)
                {
                    Astreak++;
                    if (Astreak == WinningStreak)
                    {
                        return Cell.AgentPiece;
                    }
                    c--;
                }
                else { break; }
            }
            for (r = mostRecentRow - 1, c = mostRecentColumn + 1; r >= 0 && c < Columns; r--)
            {
                if (Cells[r, c] == Cell.AgentPiece)
                {
                    Astreak++;
                    if (Astreak == WinningStreak)
                    {
                        return Cell.AgentPiece;
                    }
                    c++;
                }
                else { break; }

            }
        }
        return Cell.Empty;
    }

    private Cell CheckLeftDiagonal(int mostRecentRow, int mostRecentColumn)
    {
        int r = mostRecentRow;
        int c = mostRecentColumn;
        if (Cells[r, c] == Cell.HumanPiece)
        {
            int Hstreak = 0;

            for (r = mostRecentRow; r >= 0 && c > 0; r--)
            {
                if (Cells[r, c] == Cell.HumanPiece)
                {
                    Hstreak++;
                    if (Hstreak == WinningStreak)
                    {
                        return Cell.HumanPiece;
                    }
                    c--;
                }
                else { break; }
            }
            for (r = mostRecentRow + 1, c = mostRecentColumn + 1; r < Rows && c < Columns; r++)
            {
                if (Cells[r, c] == Cell.HumanPiece)
                {
                    Hstreak++;
                    if (Hstreak == WinningStreak)
                    {
                        return Cell.HumanPiece;
                    }
                    c++;
                }
                else { break; }

            }
        }
        else if (Cells[r, c] == Cell.AgentPiece)
        {
            int Astreak = 0;
            for (r = mostRecentRow; r >= 0 && c > 0; r--)
            {
                if (Cells[r, c] == Cell.AgentPiece)
                {
                    Astreak++;
                    if (Astreak == WinningStreak)
                    {
                        return Cell.AgentPiece;
                    }
                    c--;
                }
                else { break; }
            }
            for (r = mostRecentRow + 1, c = mostRecentColumn + 1; r < Rows && c < Columns; r++)
            {
                if (Cells[r, c] == Cell.AgentPiece)
                {
                    Astreak++;
                    if (Astreak == WinningStreak)
                    {
                        return Cell.AgentPiece;
                    }
                    c++;
                }
                else { break; }

            }
        }
        return Cell.Empty;
    }

    public override string ToString()
    {
        string[] lines = new string[Rows];


        for (int row = 0; row < Rows; ++row)
        {
            string line = "";
            for (int col = 0; col < Columns; ++col)
            {
                switch (Cells[row, col])
                {
                    case Cell.Empty: line += '-'; break;
                    case Cell.AgentPiece: line += 'O'; break;
                    case Cell.HumanPiece: line += 'X'; break;
                }
            }
            lines[row] = line;
        }

        return String.Join("\n", lines);
    }
}