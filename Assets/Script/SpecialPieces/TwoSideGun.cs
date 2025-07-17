using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoSideGun : SpecialPieceCore
{

    public override void Start()
    {
        base.Start();
        size = 2.4f;
        ShowPopupBounce(sizeX);

        //GetComponent<RectTransform>().localScale = new Vector2(2.4f, 2.4f);
    }

    public override void MoveStart(Action onMoveComplete)
    {
        StartCoroutine(WaitForAttackComplete(onMoveComplete));
    }
    IEnumerator WaitForAttackComplete(Action onMoveComplete)
    {
        animator.SetTrigger("Attack");
        animator.SetTrigger("FinalIdle");
        yield return new WaitForSeconds(0.5f); // Attack animasiyasının müddəti
        GameManager.Instance.board.DestroyPiece(PieceCell.x + 1, PieceCell.y, playerValue, specialPieceData);
        GameManager.Instance.board.DestroyPiece(PieceCell.x - 1, PieceCell.y, playerValue, specialPieceData);
        MoveEnd(onMoveComplete);
    }

}
