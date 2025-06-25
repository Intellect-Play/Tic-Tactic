using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Snayper : SpecialPieceCore
{
    Board board;
    public override void Start()
    {
        base.Start();
        board = GameManager.Instance.board;
    }
    public override void MoveStart(Action onMoveComplete)
    {
        PieceType pieceType = playerValue == PieceType.Player ? PieceType.Enemy : PieceType.Player;
        List<Cell> EnemyCells = board.Cells.FindAll(c => c._PlayerPiece != null&& c._PlayerPiece.playerValue== pieceType);
        UnityEngine.Debug.Log("Snayper MoveStart: " + EnemyCells.Count + " cells found for piece type: " + pieceType);
        if (EnemyCells.Count != 0)
        {
            Cell PieceCell = EnemyCells[UnityEngine.Random.Range(0,EnemyCells.Count)];
            board.DestroyPiece(PieceCell.x, PieceCell.y, playerValue);

            UnityEngine.Debug.Log("Snayper MoveStart: " + PieceCell.x + " " + PieceCell.y);
        }

        MoveEnd(onMoveComplete);
    }
}
