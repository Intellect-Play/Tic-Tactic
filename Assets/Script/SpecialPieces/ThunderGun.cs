using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderGun : SpecialPieceCore
{
    public override void MoveStart(Action onMoveComplete)
    {
        //Debug.Log("TwoSideGun MoveStart");
        GameManager.Instance.board.DestroyPiece(PieceCell.x+1, PieceCell.y + 1, playerValue);
        GameManager.Instance.board.DestroyPiece(PieceCell.x + 1, PieceCell.y - 1, playerValue);
        GameManager.Instance.board.DestroyPiece(PieceCell.x - 1, PieceCell.y + 1, playerValue);
        GameManager.Instance.board.DestroyPiece(PieceCell.x - 1, PieceCell.y - 1, playerValue);


        MoveEnd(onMoveComplete);
    }
}
