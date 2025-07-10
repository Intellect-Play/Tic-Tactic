using System;
using System.Collections.Generic;
using UnityEngine;

public class Immortal : SpecialPieceCore
{
    public int KillTime=1;
    public override void DestroyPiece()
    {
        animator.SetTrigger("Attack");

        if (KillTime==0) base.DestroyPiece();
        KillTime--;

    }

    public override void MoveStart(Action onMoveComplete)
    {

        MoveEnd(onMoveComplete);
    }
}
