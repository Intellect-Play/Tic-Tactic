using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PieceMovePlayer : PlayerPiece
{
    //// Start is called before the first frame update
    //public void OnBeginDrag(PointerEventData eventData)
    //{
    //    canvasGroup.blocksRaycasts = false;
    //    transform.parent = canvas.transform;

    //}

    //public void OnDrag(PointerEventData eventData)
    //{
    //    rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    //}

    //public void OnEndDrag(PointerEventData eventData)
    //{
    //    RemoveCell();
    //    canvasGroup.blocksRaycasts = true;

    //    GameObject obj = eventData.pointerEnter;
    //    if (obj != null && obj.TryGetComponent(out Cell cell))
    //    {
    //        if (!cell.HasValue)
    //        {
    //            transform.SetParent(obj.transform);
    //            rectTransform.anchoredPosition = Vector2.zero;
    //            transform.parent = originalParent;
    //            PieceCell = cell; // Cell referansını saxla
    //            cell.SetValue(this);
    //            return;
    //        }
    //    }
    //    //transform.parent = originalParent;
    //    rectTransform.localPosition = originalPos;
    //}
}
