using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class XOWinConditionsAnySize 
{
    //public static WinResult CheckWin(this Cell[] board)
    //{
    //    int size = Mathf.RoundToInt(Mathf.Sqrt(board.Length));
    //    if (size * size != board.Length)
    //    {
    //        Debug.LogError("Board is not a perfect square.");
    //        return new WinResult(false, null);
    //    }

    //    string winner;

    //    if (CheckRows(board, size, out winner) ||
    //        CheckColumns(board, size, out winner) ||
    //        CheckDiagonals(board, size, out winner))
    //    {
    //        return new WinResult(true, winner);
    //    }

    //    return new WinResult(false, null);
    //}

    //private static bool CheckRows(Cell[] board, int size, out string winner)
    //{
    //    for (int row = 0; row < size; row++)
    //    {
    //        bool allSame = true;
    //        string first = board[row * size].cellValue;
    //        if (string.IsNullOrEmpty(first))
    //        {
    //            continue;
    //        }

    //        for (int col = 1; col < size; col++)
    //        {
    //            if (board[row * size + col].cellValue != first)
    //            {
    //                allSame = false;
    //                break;
    //            }
    //        }

    //        if (allSame)
    //        {
    //            winner = first;
    //            return true;
    //        }
    //    }

    //    winner = null;
    //    return false;
    //}

    //private static bool CheckColumns(Cell[] board, int size, out string winner)
    //{
    //    for (int col = 0; col < size; col++)
    //    {
    //        bool allSame = true;
    //        string first = board[col].cellValue;
    //        if (string.IsNullOrEmpty(first))
    //        {
    //            continue;
    //        }

    //        for (int row = 1; row < size; row++)
    //        {
    //            if (board[row * size + col].cellValue != first)
    //            {
    //                allSame = false;
    //                break;
    //            }
    //        }

    //        if (allSame)
    //        {
    //            winner = first;
    //            return true;
    //        }
    //    }

    //    winner = null;
    //    return false;
    //}

    //private static bool CheckDiagonals(Cell[] board, int size, out string winner)
    //{
    //    string firstMain = board[0].cellValue;
    //    bool mainDiagonal = !string.IsNullOrEmpty(firstMain);
    //    for (int i = 1; i < size; i++)
    //    {
    //        if (board[i * size + i].cellValue != firstMain)
    //        {
    //            mainDiagonal = false;
    //            break;
    //        }
    //    }

    //    if (mainDiagonal)
    //    {
    //        winner = firstMain;
    //        return true;
    //    }

    //    string firstAnti = board[size - 1].cellValue;
    //    bool antiDiagonal = !string.IsNullOrEmpty(firstAnti);
    //    for (int i = 1; i < size; i++)
    //    {
    //        if (board[i * size + (size - 1 - i)].cellValue != firstAnti)
    //        {
    //            antiDiagonal = false;
    //            break;
    //        }
    //    }

    //    if (antiDiagonal)
    //    {
    //        winner = firstAnti;
    //        return true;
    //    }

    //    winner = null;
    //    return false;
    //}
}
