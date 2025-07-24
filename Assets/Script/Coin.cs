using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{
    public static Coin Instance;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] RectTransform targetUI;
    [SerializeField] RectTransform ParentCanvas;
    float randomNum = 0.8f;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void GetCoin(int coin)
    {
        SaveDataService.Coins += coin;
        if(SaveDataService.Coins < 0)
        {
            SaveDataService.Coins = 0;
        }
        UIManager.Instance.UpdateCoinText(SaveDataService.Coins);
        
    }

    public void PlayCoinEffect(RectTransform startParent, float duration = 1)
    {
        for (int i = 0; i < 3; i++)
        {
            // Coin Image yaradılır (UI obyekti)
            GameObject coin = Instantiate(coinPrefab, startParent.position, Quaternion.identity, ParentCanvas);
            RectTransform coinRect = coin.GetComponent<RectTransform>();

            // Başlanğıc mövqeyi
            coinRect.position = startParent.position;
            // coinRect.localScale = Vector3.zero;

            // Random yayılma mövqeyi (ilk mərhələ)
            Vector3 randomSpread = startParent.position + new Vector3(
                Random.Range(-randomNum, randomNum),
                Random.Range(-randomNum, randomNum),
                0f
            );
            Debug.Log("Random Spread Position: " + randomSpread);
            // DOTween Sequence
            Sequence seq = DOTween.Sequence();

            // 1. Kiçik effekt: scale artımı
            seq.Append(coinRect.DOScale(1f, 0.2f).SetEase(Ease.OutBack));

            // 2. Random yayılma (ilk partlayış)
            seq.Append(coinRect.DOMove(randomSpread, duration*.5f ).SetEase(Ease.OutQuad));

            // 3. Target UI-a doğru uçuş
            seq.Append(coinRect.DOMove(targetUI.position, duration * .5f).SetEase(Ease.InQuad));

            // 4. Fade out effekti
            Image img = coin.GetComponent<Image>();
            if (img != null)
            {
                seq.Join(img.DOFade(0f, duration ).SetDelay(duration));
            }

            // 5. Bitəndə obyekt silinsin
            seq.OnComplete(() => Destroy(coin));
        }
    }


}
