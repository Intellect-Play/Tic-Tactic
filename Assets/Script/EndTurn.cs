using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class EndTurn : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Button button;
    RectTransform rectTransform;
    float size = 1.0f; // Default size, can be adjusted in the inspector

    [SerializeField] private RectTransform imageToScale;
    [SerializeField] private Button toggleButton;
    public float scaleCount = 1.1f;

    private Vector3 bigScale = new Vector3(1.1f, 1.1f, 1f);
    [SerializeField] private float scaleDuration = 0.5f;

    private Vector3 originalScale = new Vector3(1,1,1);
    private bool isActive = false;
    private Tween scaleTween;
    private void Start()
    {
        if (button != null)
        {
            button.onClick.AddListener(OnEndTurnButtonClicked);
        }

        if (imageToScale == null) imageToScale = GetComponent<RectTransform>();

      
    }

    public void GetIncretible(bool Active)
    {
        button.interactable = Active;
        ToggleScaleLoop(Active);
        if (Active) OnEndTurnButtonClicked();
    }
    public void ShowPopupBounce(float x)
    {

        Sequence popupSequence = DOTween.Sequence();
        popupSequence.Append(rectTransform.DOScale((x + 0.1f), 0.1f).SetEase(Ease.OutQuad));  // 0 → 1.2x böyümə
        popupSequence.Append(rectTransform.DOScale(x-.1f, 0.1f).SetEase(Ease.InOutQuad)); // bir az balacalaşma
        popupSequence.Append(rectTransform.DOScale(x, 0.1f).SetEase(Ease.OutBack)); // düz ölçüyə qayıtma
    }
    public void OnEndTurnButtonClicked()
    {
        if(!GameManager.Instance.EndTurnButtonPressed) return;
        SoundManager.Instance.PlaySound(SoundType.Click);

        ShowPopupBounce(size);
       GetIncretible(false);

        GameManager.Instance.EndTurnButtonPressed = false;
        //Debug.Log("End Turn button clicked");
        GameActions.Instance.InvokeEndTurn();
    }

    private void ToggleScaleLoop(bool active)
    {
        if(isActive == active) return; // Əgər artıq bu vəziyyətdədirsə, heç nə etmirik
        isActive = active;

        if (isActive)
        {
            StartLoop();
        }
        else
        {
            StopLoop();
        }
    }

    private void StartLoop()
    {
        // Əvvəlki tween varsa dayandır
        scaleTween?.Kill();

        // Sonsuz loop ilə böyüyüb-kiçilmə animasiyası
        scaleTween = imageToScale
            .DOScale(bigScale, scaleDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    private void StopLoop()
    {
        // Loop-u dayandır və orijinal ölçüyə qayıt
        scaleTween?.Kill();
        imageToScale.DOScale(originalScale, scaleDuration).SetEase(Ease.OutBack);
    }

}
