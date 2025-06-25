using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plus2 : SpecialPieceCore
{
    public override void AttackDestroy(RectTransform targetPosition)
    {
        Health.Instance.Damage(2, playerValue);
        DestroyPiece();
        //rectTransform.DOMove(targetPosition.position, moveDuration)
        //   .SetEase(moveEase)
        //   .OnComplete(() =>
        //   {
        //       Debug.Log("Hədəfə çatdı! Attack tamamlandı.");
        //       // Burada zərbə animasiyası və ya digər effekti əlavə edə bilərsən
        //       Destroy(gameObject);
        //   });
    }

    public override void MoveStart(Action onMoveComplete)
    {
        MoveEnd(onMoveComplete);
    }
}
