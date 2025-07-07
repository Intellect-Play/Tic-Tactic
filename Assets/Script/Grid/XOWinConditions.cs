using System.Collections.Generic;
using UnityEngine;

public static class XOWinConditions
{
    private const int WinCount = 3;

    public static WinResult CheckWin(this List<Cell> board)
    {
        int rowCount = GameDatas.Instance.mainGameDatasSO.BoardSizeY;
        int colCount = GameDatas.Instance.mainGameDatasSO.BoardSizeX;
        if (rowCount * colCount != board.Count)
        {
            Debug.LogError("Invalid board: Dimensions do not match board size.");
            return new WinResult(false, PieceType.Null, null);
        }

        List<Cell> winCells = new List<Cell>();
        HashSet<int> visitedIndices = new HashSet<int>();

        for (int y = 0; y < rowCount; y++)
        {
            for (int x = 0; x < colCount; x++)
            {
                var current = board[y * colCount + x];
                if (current.cellValue == PieceType.Null)
                    continue;

                TryDirection(board, rowCount, colCount, x, y, 1, 0, current.cellValue, winCells, visitedIndices);  // →
                TryDirection(board, rowCount, colCount, x, y, 0, 1, current.cellValue, winCells, visitedIndices);  // ↓
                TryDirection(board, rowCount, colCount, x, y, 1, 1, current.cellValue, winCells, visitedIndices);  // ↘
                TryDirection(board, rowCount, colCount, x, y, -1, 1, current.cellValue, winCells, visitedIndices); // ↙
            }
        }

        if (winCells.Count > 0)
        {
            PieceType winner = winCells[0].cellValue;
            return new WinResult(true, winner, winCells);
        }

        return new WinResult(false, PieceType.Null, null);
    }

    private static void TryDirection(List<Cell> board, int rowCount, int colCount, int startX, int startY, int stepX, int stepY, PieceType pieceType, List<Cell> winCells, HashSet<int> visited)
    {
        List<Cell> temp = new List<Cell>();
        int x = startX;
        int y = startY;

        while (x >= 0 && x < colCount && y >= 0 && y < rowCount)
        {
            int index = y * colCount + x;
            if (board[index].cellValue != pieceType)
                break;

            temp.Add(board[index]);
            x += stepX;
            y += stepY;
        }

        if (temp.Count >= WinCount)
        {
            foreach (var cell in temp)
            {
                int idx = board.IndexOf(cell);
                if (!visited.Contains(idx))
                {
                    visited.Add(idx);
                    winCells.Add(cell);
                }
            }
        }
    }
}


public struct WinResult
{
    public bool hasWon;
    public PieceType winner; 
    public List<Cell> winCells;
    public WinResult(bool hasWon, PieceType winner,List<Cell> cells)
    {
        this.hasWon = hasWon;
        this.winner = winner;
        this.winCells = cells;
    }
}