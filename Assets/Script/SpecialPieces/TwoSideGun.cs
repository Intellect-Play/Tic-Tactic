using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoSideGun : SpecialPieceCore
{


    public override void MoveStart(Action onMoveComplete)
    {
        //Debug.Log("TwoSideGun MoveStart");
        GameManager.Instance.board.DestroyPiece(PieceCell.x,PieceCell.y+1);
        GameManager.Instance.board.DestroyPiece(PieceCell.x , PieceCell.y-1);

        MoveEnd(onMoveComplete);
    }


}
