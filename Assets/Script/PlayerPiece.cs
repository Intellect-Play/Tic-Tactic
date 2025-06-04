using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Unity.Collections;
using DG.Tweening;


public class PlayerPiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 originalPos;

    [SerializeField] public string playerValue;
    [SerializeField] private TextMeshProUGUI valueText;

    private Transform originalParent;
    public Cell PieceCell;

    public float moveDuration = 0.5f; // Neçə saniyəyə getsin
    public Ease moveEase = Ease.OutBack; // Hərəkət effekti
    public bool IsPlaced => PieceCell != null && PieceCell.HasValue;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        originalPos = rectTransform.localPosition;
        valueText.text = playerValue;
        originalParent = transform.parent; 
    }

    public void Init(string value)
    {
        playerValue = value;
        valueText.text = playerValue;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        RemoveCell();
        canvasGroup.blocksRaycasts = true;

        GameObject obj = eventData.pointerEnter;
        if (obj != null && obj.TryGetComponent(out Cell cell))
        {
            if (!cell.HasValue)
            {
                transform.SetParent(obj.transform); 
                rectTransform.anchoredPosition = Vector2.zero;             
                transform.parent = originalParent;
                PieceCell = cell; // Cell referansını saxla
                cell.SetValue(this);
                return;
            }
        }
        
        rectTransform.localPosition = originalPos;
    }
    
    public void RemoveCell()
    {
        if (PieceCell != null)
        {
            PieceCell.RemoveCell();
        }
    }

    public void AttackDestroy(RectTransform targetPosition)
    {
        rectTransform.DOMove(targetPosition.position, moveDuration)
           .SetEase(moveEase)
           .OnComplete(() =>
           {
               Debug.Log("Hədəfə çatdı! Attack tamamlandı.");
               // Burada zərbə animasiyası və ya digər effekti əlavə edə bilərsən
               Destroy(gameObject);
           });
    }
}
