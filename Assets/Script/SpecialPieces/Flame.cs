using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : SpecialPieceCore
{
    public override void MoveStart(Action onMoveComplete)
    {
        GameManager.Instance.board.DestroyPiece(PieceCell.x , PieceCell.y+1, playerValue);
        GameManager.Instance.board.DestroyPiece(PieceCell.x , PieceCell.y-1, playerValue);

        MoveEnd(onMoveComplete);
    }
}
