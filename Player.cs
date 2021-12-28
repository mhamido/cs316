using System;
using System.Collections.Generic;

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

    private IEnumerable<Board> GetSucessors(Board board)
    {
        throw new NotImplementedException();
    }

    public int Deliberate(Board board)
    {
        int minimax(Board currentBoard, int depth, Player player)
        {
            if (depth == 0)
                return Evaluate(currentBoard);

            switch (player)
            {
                case Player.Agent:
                    int bestOption = Int32.MinValue;
                    foreach (var Child in currentBoard)
                    {

                    }
                    break;
                case Player.Human:
                    break;
            }
        }

        return minimax(board, depthLimit, Player.Agent);
    }

    private int Evaluate(Board board)
    {
        throw new NotImplementedException();
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