using System.Collections.Generic;
using UnityEngine;

public static class XOWinConditions
{
    public static WinResult CheckWin(this List<Cell> board)
    {
        PieceType winner;
        List<Cell> winCells = new List<Cell>();
        bool verticalBool = CheckRows(board, out winner, ref winCells);
        bool horizontalBool = CheckColumns(board, out winner, ref winCells);
        bool diagonalBool = CheckDiagonals(board, out winner, ref winCells);

        if (verticalBool || horizontalBool || diagonalBool)
        {
            return new WinResult(true, winner, winCells);
        }
        return new WinResult(false, PieceType.Null, null);
    }

    private static bool CheckRows(List<Cell> board, out PieceType winner, ref List<Cell> winCells)
    {
        for (int i = 0; i < 3; i++)
        {
            int start = i * 3;
            if (CheckValues(board[start], board[start + 1], board[start + 2], out winner))
            {
                winCells.AddRange(new[] { board[start], board[start + 1], board[start + 2] });
                return true;
            }
        }
        winner = PieceType.Null;
        return false;
    }

    private static bool CheckColumns(List<Cell> board, out PieceType winner,ref List<Cell> winCells)
    {
        for (int i = 0; i < 3; i++)
        {
            if (CheckValues(board[i], board[i + 3], board[i + 6], out winner))
            {
                winCells.AddRange(new[] { board[i], board[i + 3], board[i + 6] });
                return true;
            }
                
        }
        winner = PieceType.Null;
        return false;
    }

    private static bool CheckDiagonals(List<Cell> board, out PieceType winner, ref List<Cell> winCells)
    {
        if (CheckValues(board[0], board[4], board[8], out winner))
        {
            winCells.AddRange(new[] { board[0], board[4], board[8] });
            return true;
        }
        ;
        if (CheckValues(board[2], board[4], board[6], out winner))
        {
            winCells.AddRange(new[] { board[2], board[4], board[6] });
            return true;
        }

        winner = PieceType.Null;
        return false;
    }

    private static bool CheckValues(Cell a, Cell b, Cell c, out PieceType winner)
    {
        if (a.cellValue == PieceType.Null || b.cellValue == PieceType.Null || c.cellValue == PieceType.Null)
        {
            winner = PieceType.Null;
            return false;
        }

        if (a.cellValue == b.cellValue && a.cellValue == c.cellValue)
        {

            winner = a.cellValue;
            return true;
        }

        winner = PieceType.Null;
        return false;
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