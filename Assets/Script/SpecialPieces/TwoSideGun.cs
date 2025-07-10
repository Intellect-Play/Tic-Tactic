using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoSideGun : SpecialPieceCore
{
    public override void Start()
    {
        base.Start();
        GetComponent<RectTransform>().localScale = new Vector2(2.4f, 2.4f);
    }

    public override void MoveStart(Action onMoveComplete)
    {
        animator.SetTrigger("Attack");
        Debug.Log("TwoSideGun MoveStart");
        GameManager.Instance.board.DestroyPiece(PieceCell.x+1,PieceCell.y, playerValue);
        GameManager.Instance.board.DestroyPiece(PieceCell.x-1 , PieceCell.y, playerValue);

        MoveEnd(onMoveComplete);
    }


}
