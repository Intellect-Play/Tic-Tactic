using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;

public class Line : MonoBehaviour
{
    public Image lineImage; // Assign in Inspector

    private void OnEnable()
    {
        lineImage = GetComponent<Image>();
        PlayStrikeEffect();
    }
    public void PlayStrikeEffect(float fadeDuration = 0.3f, float fillDuration = 0.2f, float delayAfterFill = 0.6f)
    {
        // İlk hazırlıq: şəffaf və fill = 0
        Color color = lineImage.color;
        color.a = 0f;
        lineImage.color = color;
        lineImage.fillAmount = 0f;

        // Fade-in → Fill → Fade-out ardıcıl animasiyası
        Sequence seq = DOTween.Sequence();

        seq.Append(lineImage.DOFade(1f, 0)) // Fade in
           .Append(lineImage.DOFillAmount(1f, fillDuration)) // Fill 0 → 1
           .AppendInterval(delayAfterFill) // Gözləmə
           .Append(lineImage.DOFade(0f, fadeDuration)) // Fade out
           .OnComplete(() =>
           {
               // Opsional: animasiya bitəndə obyekt gizlədilsin
               gameObject.SetActive(false);
           });
    }
}
