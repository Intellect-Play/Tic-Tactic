using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderGun : SpecialPieceCore
{
    Board board;
    public override void Start()
    {
        base.Start();
        board = GameManager.Instance.board;
    }
    public override void MoveStart(Action onMoveComplete)
    {
        Debug.Log("TwoSideGun MoveStart");
        board.DestroyPiece(PieceCell.x + 1, PieceCell.y + 1, playerValue);
        board.DestroyPiece(PieceCell.x + 1, PieceCell.y - 1, playerValue);
        board.DestroyPiece(PieceCell.x - 1, PieceCell.y + 1, playerValue);
        board.DestroyPiece(PieceCell.x - 1, PieceCell.y - 1, playerValue);


        MoveEnd(onMoveComplete);
    }
}
