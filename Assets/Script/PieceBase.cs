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
    public Image pieceImage;
    public Transform originalParent;
    public Cell PieceCell;

    public float moveDuration = 0.5f; 
    public Ease moveEase = Ease.OutBack; 
    public bool IsPlaced;
    public PlayerSpawnButtons playerSpawnButtons;
    RectTransform targetCell;
    public float size;
    public float sizeX;


    public virtual void Start()
    {
        pieceImage = GetComponent<Image>();
        if (playerValue == PieceType.Player) sizeX = GameDatas.Instance.mainGameDatasSO.PlayerPieceSize;
        else sizeX = GameDatas.Instance.mainGameDatasSO.EnemyPieceSize;
        size = GameDatas.Instance.mainGameDatasSO.PieceSize;
        GetComponent<RectTransform>().localScale = new Vector2(size, size);
        moveDuration = GameDatas.Instance.mainGameDatasSO.MoveDuration;
        IsPlaced = false;
        targetImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        originalPos = rectTransform.localPosition;
        //valueText.text = playerValue;
        originalParent = transform.parent; 
        ShowPopupBounce(sizeX);
    }
    public void ShowPopupBounce(float x)
    {
        rectTransform.localScale = new Vector3(size-.4f,size - .4f, size - .4f);

        Sequence popupSequence = DOTween.Sequence();
        popupSequence.Append(rectTransform.DOScale((size+0.2f)*x, 0.2f).SetEase(Ease.OutQuad));  // 0 → 1.2x böyümə
        popupSequence.Append(rectTransform.DOScale(1.5f * x, 0.15f).SetEase(Ease.InOutQuad)); // bir az balacalaşma
        popupSequence.Append(rectTransform.DOScale(size * x, 0.1f).SetEase(Ease.OutBack)); // düz ölçüyə qayıtma
    }
    public virtual void Placed(bool _isPlaced)
    {
        IsPlaced = _isPlaced;
        if (IsPlaced&&playerSpawnButtons!=null) { 
            playerSpawnButtons.pieceBase = null; // Remove reference to the playerSpawnButtons when placed        
            playerSpawnButtons = null;
        }
        
    }
    public virtual void Init(PieceType value)
    {
        targetImage.raycastTarget = false;

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
        rectTransform.DOScale(size, 0.1f).SetEase(Ease.OutBack);
        PieceCell = cell;
        cell.SetValue(this);
        //ShowPopupBounce(1);
        SoundManager.Instance.PlaySound(SoundType.PutPiece);

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
        rectTransform.DOScale(size, 0.1f).SetEase(Ease.OutBack);
        //popupSequence.Append(rectTransform.DOScale(size , 0.1f).SetEase(Ease.OutBack)); // düz ölçüyə qayıtma
        //ShowPopupBounce(1);

        // Əgər ehtiyac varsa loqika üçün bunu yenə saxla:

    }
    public virtual void Back()
    {
        //Debug.Log("Back to original position");
        targetImage.raycastTarget = true;

       
        transform.parent = originalParent;
        //rectTransform.localPosition = originalPos;
        if (PieceCell != null)
            PieceCell.RemoveCell();
        rectTransform.DOLocalMove(originalPos, GameDatas.Instance.mainGameDatasSO.MoveDuration).SetEase(Ease.InOutQuad);

        rectTransform.DOScale(size * sizeX, 0.1f).SetEase(Ease.OutBack);


    }
    public virtual void BackCell()
    {
        //Debug.Log("Back to original position");
        targetImage.raycastTarget = true;
        //rectTransform.DOScale(size * sizeX, 0.1f).SetEase(Ease.OutBack);

        if (PieceCell == null)
        {
            transform.parent = originalParent;
            rectTransform.DOLocalMove(originalPos, GameDatas.Instance.mainGameDatasSO.MoveDuration).SetEase(Ease.InOutQuad);
        }
        else
        {
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
    public virtual void PlayPopFade(float scaleAmount = .3f, float duration = 0.3f)
    {
        RemoveCellBase();

        targetImage.color = new Color(1f, 1f, 1f, 1f); 
        targetImage.transform.localScale = Vector3.one;

        Sequence seq = DOTween.Sequence();

        seq.Join(targetImage.transform.DOScale(scaleAmount+size, duration).SetEase(Ease.OutBack));
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
