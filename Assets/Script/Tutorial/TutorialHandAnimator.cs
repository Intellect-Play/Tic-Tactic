using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialHandAnimator : MonoBehaviour
{
    public RectTransform handImage;
    public float moveDistance = 100f;
    public float duration = 1f;

    public Canvas canvas;
    private Tweener currentTween;
    public bool TweenPlay;

    [Header("Optional Directional Arrows")]
    public RectTransform fourArrow;


    void Awake()
    {
        TweenPlay = false;
        canvas = handImage.GetComponentInParent<Canvas>();
        handImage.gameObject.SetActive(false); // başlanğıcda gizli olsun
    }

    public void ShowMoveHandAnimationUI(RectTransform uiTarget, Vector3 offset)
    {
        if (uiTarget == null)
        {
            HideHand();
            return;
        }
        handImage.gameObject.SetActive(true);

        Debug.Log("ShowMoveHandAnimationUI called with target: " + uiTarget.anchoredPosition);

        TweenPlay = true;

        // Dünya mövqeyini al
        Vector3[] corners = new Vector3[4];
        uiTarget.GetWorldCorners(corners);
        Vector3 centerWorldPos = (corners[0] + corners[2]) * 0.5f + offset;

        // Hand-in world mövqeyini təyin et
        handImage.position = centerWorldPos;
        gameObject.SetActive(true);

        currentTween?.Kill();
        handImage.localScale = Vector3.one;

        // ✳️ World space-də yuxarı-aşağı animasiya
        Vector3 upOffset = new Vector3(1f, 3f, 0f); // yuxarı-aşağı animasiya miqdarı

        currentTween = handImage
            .DOMove(centerWorldPos + upOffset, duration * 0.7f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }


    public void ShowTapAnimationWorldUI(RectTransform uiTarget, Vector3 offset)
    {
        if (uiTarget == null)
        {
            HideHand();
            return;
        }
        Debug.Log("ShowTapAnimationWorldUI called with target: " + uiTarget.anchoredPosition);

        handImage.gameObject.SetActive(true);
        gameObject.SetActive(true);
        TweenPlay = true;

        bool isWorldCanvas = canvas.renderMode == RenderMode.WorldSpace;
        bool isSameCanvas = handImage.GetComponentInParent<Canvas>() == uiTarget.GetComponentInParent<Canvas>();

        if (isWorldCanvas && isSameCanvas)
        {
            // Hər ikisi World Space canvas-dadırsa, sadəcə pozisiyanı kopyala
            handImage.position = uiTarget.position + offset;
        }
        else
        {
            // Ekran mövqeyi ilə konvertasiya
            Vector3 screenPos = Camera.main.WorldToScreenPoint(uiTarget.position + offset);

            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                handImage.parent as RectTransform,
                screenPos,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main,
                out localPoint
            );

            handImage.localPosition = localPoint;
        }


        currentTween?.Kill();
        handImage.localScale = Vector3.one;

        currentTween = handImage
            .DOScale(1.3f, duration * 0.5f)
            .SetEase(Ease.OutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
    public void ShowTapAnimationUI(RectTransform uiTarget, Vector3 offset)
    {
        if (uiTarget == null)
        {
            HideHand();
            return;
        }

        TweenPlay = true;

        // 1. Dünyadakı (world space) mövqeni tap
        Vector3 worldPos = uiTarget.position + offset;

        // 2. Ekran koordinatını al
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main,
            worldPos
        );

        // 3. UI parent içində lokal mövqe tap
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            handImage.parent as RectTransform,
            screenPos,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main,
            out localPoint
        );

        // 4. handImage yerləşdir
        handImage.localPosition = localPoint;
        handImage.gameObject.SetActive(true);
        gameObject.SetActive(true);

        // 5. Animasiya təmizlənir və yenidən başlayır
        currentTween?.Kill();
        handImage.localScale = Vector3.one;

        currentTween = handImage
            .DOScale(1.3f, duration * 0.5f)
            .SetEase(Ease.OutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void ShowTapAnimationUIEndTurn(RectTransform uiTarget, Vector2 offset)
    {
        if (uiTarget == null)
        {
            HideHand();
            return;
        }

        TweenPlay = true;

        // 1. Dünyadakı (world space) mövqeni tap
        Vector3 worldPos = uiTarget.anchoredPosition;

        // 2. Ekran koordinatını al
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main,
            worldPos
        );

        // 3. UI parent içində lokal mövqe tap
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            handImage.parent as RectTransform,
            screenPos,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main,
            out localPoint
        );

        // 4. handImage yerləşdir
        handImage.localPosition = localPoint;
        handImage.gameObject.SetActive(true);
        gameObject.SetActive(true);

        // 5. Animasiya təmizlənir və yenidən başlayır
        currentTween?.Kill();
        handImage.localScale = Vector3.one;

        currentTween = handImage
            .DOScale(1.3f, duration * 0.5f)
            .SetEase(Ease.OutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void HideHandTouch()
    {
        TweenPlay = false;
        currentTween?.Kill();
        currentTween = null;
        handImage.gameObject.SetActive(false);
        gameObject.SetActive(false);
        
    }

    public void HideHand()
    {
        if (!TweenPlay)
        {
            currentTween?.Kill();
            currentTween = null;
            handImage.gameObject.SetActive(false);
            gameObject.SetActive(false);
           
        }
    }
  
  

  
}
