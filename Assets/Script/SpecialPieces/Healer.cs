using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Healer : SpecialPieceCore
{
    public override void AttackDestroy(RectTransform targetPosition)
    {
        Health.Instance.Heal(1, playerValue);

        base.AttackDestroy(targetPosition);
    }

    public override void MoveStart(Action onMoveComplete)
    {

        MoveEnd(onMoveComplete);
    }
}
