using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PieceMovePlayer : PieceBase, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // Start is called before the first frame update
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (IsPlaced) return;
        targetImage.raycastTarget=false;
        canvasGroup.blocksRaycasts = false;
        transform.parent = canvas.transform;

    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (IsPlaced) return;

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (IsPlaced) return;

        //RemoveCell();
        canvasGroup.blocksRaycasts = true;

        GameObject obj = eventData.pointerEnter;
        if (obj != null && obj.TryGetComponent(out Cell cell))
        {
          
            if (!cell.HasValue ||(cell.HasValue&&cell.cellValue==playerValue&&!cell._PlayerPiece.IsPlaced))
            {
                PlayerController.Instance.GetPiece(this);
                ChangeCell(cell);

                //cell.SetValue(this);
                targetImage.raycastTarget = false;

                return;
            }
        }
        Back();
    }
}
