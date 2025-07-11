using System;
using UnityEngine;

public class RandomPiece : SpecialPieceCore
{
    public override void Start()
    {
        base.Start();
        GetComponent<RectTransform>().localScale = new Vector2(1.8f, 1.8f);
    }
    public override void AttackDestroy(RectTransform targetPosition)
    {
        animator.SetTrigger("Attack");

        Health.Instance.Damage(UnityEngine.Random.Range(1,6), playerValue);
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
