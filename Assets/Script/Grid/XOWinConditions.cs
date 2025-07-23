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
            return new WinResult(false, PieceType.Null, null, Vector3.zero, null);
        }

        List<Cell> winCells = null;
        Vector3 strikeRotation = Vector3.zero;

        for (int y = 0; y < rowCount; y++)
        {
            for (int x = 0; x < colCount; x++)
            {
                var current = board[y * colCount + x];
                if (current.cellValue == PieceType.Null)
                    continue;

                // Check all 4 directions
                var res = TryDirectionWithRotation(board, rowCount, colCount, x, y, 1, 0, current.cellValue);  // →
                if (res.found)
                {
                    winCells = res.cells;
                    strikeRotation = res.rotation;
                    goto FoundWin; // Qələbə tapıldı, dayanırıq
                }

                res = TryDirectionWithRotation(board, rowCount, colCount, x, y, 0, 1, current.cellValue);  // ↓
                if (res.found)
                {
                    winCells = res.cells;
                    strikeRotation = res.rotation;
                    goto FoundWin;
                }

                res = TryDirectionWithRotation(board, rowCount, colCount, x, y, 1, 1, current.cellValue);  // ↘
                if (res.found)
                {
                    winCells = res.cells;
                    strikeRotation = res.rotation;
                    goto FoundWin;
                }

                res = TryDirectionWithRotation(board, rowCount, colCount, x, y, -1, 1, current.cellValue); // ↙
                if (res.found)
                {
                    winCells = res.cells;
                    strikeRotation = res.rotation;
                    goto FoundWin;
                }
            }
        }

    FoundWin:
        if (winCells != null && winCells.Count > 0)
        {
            PieceType winner = winCells[0].cellValue;
            // Orta hüceyrəni tap
            Cell middleCell = GetMiddleCell(winCells);

            return new WinResult(true, winner, winCells, strikeRotation, middleCell);
        }

        return new WinResult(false, PieceType.Null, null, Vector3.zero, null);
    }

    // Yeni versiya: yönləndir və rotation qaytarır
    private static (bool found, List<Cell> cells, Vector3 rotation) TryDirectionWithRotation(
        List<Cell> board, int rowCount, int colCount, int startX, int startY,
        int stepX, int stepY, PieceType pieceType)
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
            // Rotation hesabla (2D üçün)
            float angle = Mathf.Atan2(stepY, stepX) * Mathf.Rad2Deg;
            Vector3 rotation = new Vector3(0, 0, angle);
            return (true, temp, rotation);
        }
        return (false, null, Vector3.zero);
    }

    // Hüceyrələr siyahısının ortadakını tapmaq
    private static Cell GetMiddleCell(List<Cell> cells)
    {
        int count = cells.Count;
        if (count == 0) return null;

        // Middle index
        int midIndex = count / 2;
        return cells[midIndex];
    }

}


public struct WinResult
{
    public bool hasWon;
    public PieceType winner;
    public List<Cell> winCells;
    public Vector3 strikeRotation;
    public Cell middleCell;

    public WinResult(bool hasWon, PieceType winner, List<Cell> cells, Vector3 rotation, Cell middleCell)
    {
        this.hasWon = hasWon;
        this.winner = winner;
        this.winCells = cells;
        this.strikeRotation = rotation;
        this.middleCell = middleCell;
    }
}
