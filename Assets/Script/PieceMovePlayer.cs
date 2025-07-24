using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PieceMovePlayer : PieceBase, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    bool BeginBool = false;
    // Start is called before the first frame update
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (IsPlaced||GameManager.Instance.IsGameFinished||playerValue!=GameManager.Instance.currentPlayer) return;
        targetImage.raycastTarget=false;
        canvasGroup.blocksRaycasts = false;
        transform.parent = canvas.transform;
        BeginBool = true;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (IsPlaced||!BeginBool) return;

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (IsPlaced || GameManager.Instance.IsGameFinished || playerValue != GameManager.Instance.currentPlayer) return;
        BeginBool = false;
        //RemoveCell();
        canvasGroup.blocksRaycasts = true;

        GameObject obj = eventData.pointerEnter;
        if (obj != null && obj.TryGetComponent(out Cell cell))
        {
          
            if (!cell.HasValue ||(cell.HasValue&&cell.cellValue==playerValue&&!cell._PlayerPiece.IsPlaced))
            {
                PlayerController.Instance.GetPiece(this);
                ChangeCell(cell);
                targetImage.raycastTarget = true;

                //cell.SetValue(this);

                return;
            }
        }else if(obj != null && obj.TryGetComponent(out PieceBase pieceBase))
        {
            if (pieceBase.playerValue == playerValue && !pieceBase.IsPlaced)
            {
                ChangeCell(pieceBase.PieceCell);
                PlayerController.Instance.GetPiece(this);
                targetImage.raycastTarget = true;
                //pieceBase.BackCell();
                return;
            }
        }
            BackCell();
    }
}
