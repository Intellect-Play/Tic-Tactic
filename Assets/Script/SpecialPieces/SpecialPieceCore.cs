using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class SpecialPieceCore : PieceMovePlayer
{
    public Animator animator;
    public AnimatorOverrideController overrideController;
    public Sprite sprite;
    public SpecialPieceData specialPieceData;   
    public void SetupSpecial(SpecialPieceData _specialPieceData)
    {
        GetComponent<Image>().sprite = _specialPieceData.XSprite;        
        specialPieceData = _specialPieceData;
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        if (animator == null)
        {
            Debug.LogError("Animation component is missing on " + gameObject.name);
        }
        overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = overrideController;
        if (playerValue == PieceType.Enemy)
        {
            overrideController["IdlePlaceholder"] = _specialPieceData.EnemyAnimeIdle;
            overrideController["Attack"] = _specialPieceData.EnemyAnimeAttack;
        }
        else
        {
            overrideController["IdlePlaceholder"] = _specialPieceData.PlayerAnimeIdle;
            overrideController["Attack"] = _specialPieceData.PlayerAnimeAttack;
        }
        animator.Rebind();
    }
    public void AddToList()
    {
        AllSpecialPiecesMove.Instance.AddPiece(this);
    }

    public virtual void MoveEnd(Action onMoveComplete)
    {
        onMoveComplete?.Invoke();
    }


    public abstract void MoveStart(Action onMoveComplete);

    public void RemoveFromList()
    {
        AllSpecialPiecesMove.Instance.RemovePiece(this);
    }
    public override void ChangeCell(Cell cell)
    {
        base.ChangeCell(cell);
        //AddToList();
    }
    public override void Back()
    {
        RemoveFromList();
        base.Back();
    }
    public override void DestroyPiece()
    {
        RemoveFromList();
        base.DestroyPiece();
    }
    public override void Placed(bool _isPlaced)
    {
        if (!IsPlaced&&_isPlaced)
        {

            AddToList();
        }
        base.Placed(_isPlaced);

    }


    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (playerValue != PieceType.Player) return;
        base.OnBeginDrag(eventData);

    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (playerValue != PieceType.Player) return;
        base.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (playerValue != PieceType.Player) return;
        base.OnEndDrag(eventData);
    }
}
