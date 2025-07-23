using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class EndTurn : MonoBehaviour
{
    // Start is called before the first frame update
    Button button;
    RectTransform rectTransform;
    float size = 1.0f; // Default size, can be adjusted in the inspector
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnEndTurnButtonClicked);
        }
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
        GameManager.Instance.EndTurnButtonPressed = false;
        //Debug.Log("End Turn button clicked");
        GameActions.Instance.InvokeEndTurn();
    }

}
