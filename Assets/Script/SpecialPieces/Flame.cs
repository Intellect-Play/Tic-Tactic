using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Flame : SpecialPieceCore
{
    public override void MoveStart(Action onMoveComplete)
    {
      
        StartCoroutine(FlameDelay(onMoveComplete));
    }
    IEnumerator FlameDelay(Action onMoveComplete)
    {
        animator.SetTrigger("Attack");
        animator.SetTrigger("FinalIdle");
        yield return new WaitForSeconds(0.5f); // Attack animasiyasının müddəti

        GameManager.Instance.board.DestroyPiece(PieceCell.x, PieceCell.y + 1, playerValue, specialPieceData);
        GameManager.Instance.board.DestroyPiece(PieceCell.x, PieceCell.y - 1, playerValue, specialPieceData);
        SoundManager.Instance.PlaySound(SoundType.Flame);
        yield return new WaitForSeconds(0.3f); // Attack animasiyasının müddəti

        MoveEnd(onMoveComplete);

    }
}
