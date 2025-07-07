using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Unity.Collections;
using DG.Tweening;
using Assets.Script;


public class PieceBase : MonoBehaviour
{
    public Canvas canvas;
    public RectTransform rectTransform;
    public CanvasGroup canvasGroup;
    public Vector3 originalPos;

    [SerializeField] public Image targetImage;

    [SerializeField] public PieceType playerValue;
    [SerializeField] public TextMeshProUGUI valueText;

    public Transform originalParent;
    public Cell PieceCell;

    public float moveDuration = 0.5f; 
    public Ease moveEase = Ease.OutBack; 
    public bool IsPlaced;
    RectTransform targetCell;
    public virtual void Start()
    {
        moveDuration = GameDatas.Instance.mainGameDatasSO.MoveDuration;
        IsPlaced = false;
        targetImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        originalPos = rectTransform.localPosition;
        //valueText.text = playerValue;
        originalParent = transform.parent; 
    }

    public virtual void Placed(bool _isPlaced)
    {
        IsPlaced = _isPlaced;
        
    }
    public virtual void Init(PieceType value)
    {
        playerValue = value;
        //valueText.text = playerValue;
    }
  
    
    public virtual void RemoveCell()
    {
        RemoveCellBase();
        transform.parent = originalParent;
        rectTransform.localPosition = originalPos;
    }
    public void RemoveCellBase()
    {
        if (PieceCell != null)
        {
            targetImage.raycastTarget = true;

            
            PieceCell.RemoveCell();
        }
    }
    public virtual void DestroyPiece()
    {
        //Debug.Log("Destroying piece");
        PlayPopFade();
    }
    public virtual void AttackDestroy(RectTransform targetPosition)
    {
        Health.Instance.Damage(1, playerValue);
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
    public virtual void ChangeCell(Cell cell)
    {
        if (PieceCell != null)
            PieceCell.RemoveCell();
        transform.SetParent(cell.gameObject.transform);
        rectTransform.anchoredPosition = Vector2.zero;
        transform.parent = originalParent;

        PieceCell = cell;
        cell.SetValue(this);
    }
    public virtual void ChangeCellDelay(Cell cell)
    {
       
        targetCell = cell.GetComponent<RectTransform>();
        Vector3 worldTargetPos = targetCell.GetComponent<RectTransform>().position;
        PieceCell = cell;
        cell.SetValue(this);
        // Mövcud rectTransform'u dünya mövqeyinə animasiya ilə apar
        rectTransform.DOMove(worldTargetPos, GameDatas.Instance.mainGameDatasSO.MoveDuration)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() => Debug.Log("Moved without parent change"));

        // Əgər ehtiyac varsa loqika üçün bunu yenə saxla:
      
    }
    public virtual void Back()
    {
        //Debug.Log("Back to original position");
        targetImage.raycastTarget = true;

       
        transform.parent = originalParent;
        rectTransform.DOLocalMove(originalPos, GameDatas.Instance.mainGameDatasSO.MoveDuration).SetEase(Ease.InOutQuad);

        //rectTransform.localPosition = originalPos;
        if(PieceCell != null)
            PieceCell.RemoveCell();

    }
    public virtual void BackCell()
    {
        //Debug.Log("Back to original position");
        targetImage.raycastTarget = true;

        if (PieceCell == null)
        {
            Debug.Log("No target cell, moving to original position");
            transform.parent = originalParent;
            rectTransform.DOLocalMove(originalPos, GameDatas.Instance.mainGameDatasSO.MoveDuration).SetEase(Ease.InOutQuad);
        }
        else
        {
            Debug.Log("Moving to target cell position");
            Vector3 worldTargetPos = PieceCell.GetComponent<RectTransform>().position;
            transform.SetParent(PieceCell.gameObject.transform);
            rectTransform.anchoredPosition = Vector2.zero;
            transform.parent = originalParent;

            //rectTransform.DOLocalMove(worldTargetPos, GameDatas.Instance.mainGameDatasSO.MoveDuration).SetEase(Ease.InOutQuad).OnComplete (() =>
            //{

            //    //Debug.Log("Moved to target cell position");
            //});            ;

            // Mövcud rectTransform'u dünya mövqeyinə animasiya ilə apar

        }


        //rectTransform.localPosition = originalPos;


    }
    public virtual void PlayPopFade(float scaleAmount = 1.5f, float duration = 0.5f)
    {
        RemoveCellBase();

        targetImage.color = new Color(1f, 1f, 1f, 1f); 
        targetImage.transform.localScale = Vector3.one;

        Sequence seq = DOTween.Sequence();

        seq.Join(targetImage.transform.DOScale(scaleAmount, duration).SetEase(Ease.OutBack));
        seq.Join(targetImage.DOFade(0f, duration).SetEase(Ease.InOutSine));

        seq.OnComplete(() =>
        {
            targetImage.gameObject.SetActive(false); 
        });
        
    }
    public virtual void ActivePiece(bool active) {
        if (targetImage != null)
        {
            targetImage.raycastTarget = active;
            canvasGroup.blocksRaycasts = active;
            canvasGroup.interactable = active;
        }
    }

    //public virtual void AddToList()
    //{
    //    throw new System.NotImplementedException();
    //}

    //public virtual void MoveStart()
    //{
    //    throw new System.NotImplementedException();
    //}

    //public virtual void MoveEnd()
    //{
    //    throw new System.NotImplementedException();
    //}

    //public virtual void RemoveFromList()
    //{
    //    throw new System.NotImplementedException();
    //}
}
