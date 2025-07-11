using System;
using System.Collections.Generic;
using UnityEngine;

public class Healer : SpecialPieceCore
{
    public override void AttackDestroy(RectTransform targetPosition)
    {
        animator.SetTrigger("Attack");
        animator.SetTrigger("FinalIdle");

        Health.Instance.Heal(1, playerValue);

        base.AttackDestroy(targetPosition);
    }

    public override void MoveStart(Action onMoveComplete)
    {

        MoveEnd(onMoveComplete);
    }
}
