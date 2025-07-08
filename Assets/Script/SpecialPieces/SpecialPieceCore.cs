using System;

using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animation))]
public abstract class SpecialPieceCore : PieceMovePlayer
{
    public Animation anim;
    public Sprite sprite;
    public void SetupAnime(Animation animation = null)
    {
        if (anim == null)
        {
            anim = GetComponent<Animation>();
        }
        if (anim == null)
        {
            Debug.LogError("Animation component is missing on " + gameObject.name);
        }
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
