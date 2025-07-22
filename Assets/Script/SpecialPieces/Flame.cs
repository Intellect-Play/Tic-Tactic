using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Flame : SpecialPieceCore
{
    public override void MoveStart(Action onMoveComplete)
    {
        animator.SetTrigger("Attack");
        animator.SetTrigger("FinalIdle");

        GameManager.Instance.board.DestroyPiece(PieceCell.x , PieceCell.y+1, playerValue, specialPieceData);
        GameManager.Instance.board.DestroyPiece(PieceCell.x , PieceCell.y-1, playerValue, specialPieceData);
        SoundManager.Instance.PlaySound(SoundType.Flame);


        MoveEnd(onMoveComplete);
    }
}
