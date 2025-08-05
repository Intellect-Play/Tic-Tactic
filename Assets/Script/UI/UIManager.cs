using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public TextMeshProUGUI Level;
    public TextMeshProUGUI MainLevel;

    public TextMeshProUGUI Coin;
    public TextMeshProUGUI CoinGame;

    [Header("Buttons")]
    public RectTransform Buttons;
    public Button playButton;
    public Button deckButton;
    public Button myHeroButton;
    public Button exitButton;

    [Header("Deck Panel")]
    public RectTransform deckPanel;

    public Animation StartButtonAnime;
    public List<GameObject> mainUIObjects = new List<GameObject>();
    private Vector2 originalPosition;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //deckPanel.SetActive(false); // Başda bağlı olsun
        originalPosition = Buttons.anchoredPosition;
        playButton.onClick.AddListener(OnPlayClicked);
        deckButton.onClick.AddListener(DeckPanelClicked);
        exitButton.onClick.AddListener(ExitClicked);

        LevelText(SaveDataService.CurrentLevel.ToString());
        StartButtonAnime.gameObject.SetActive(false);
        MoveDown(deckPanel, Screen.height+500); // 100f qədər aşağı hərəkət et
        Debug.Log(Screen.height+200);
        UpdateCoinText(SaveDataService.Coins);
    }

    public void OnPlayClicked()
    {
        SoundManager.Instance.PlaySound(SoundType.Click);
        StartCoroutine(StartGameDelay(1)); 
    }
    IEnumerator StartGameDelay(float delay)
    {
        MoveDown(Buttons, 700f); // 100f qədər aşağı hərəkət et
        StartButtonAnime.gameObject.SetActive(true);
        StartButtonAnime.Play();
        GameManager.Instance.StartGame();
        yield return new WaitForSeconds(.5f);

        SoundManager.Instance.PlaySound(SoundType.Swoop);

        yield return new WaitForSeconds(.5f);

        foreach (var obj in mainUIObjects)
        {
            obj.SetActive(false);
        }
        yield return new WaitForSeconds(2);
        StartButtonAnime.gameObject.SetActive(false);
    }
    public void MoveDown(RectTransform rectTransform,float amount)
    {
        rectTransform.DOAnchorPosY(originalPosition.y - amount, 0.4f)
                     .SetEase(Ease.InOutSine);
    }
    public void DeckPanelClicked()
    {
        SoundManager.Instance.PlaySound(SoundType.Click);

        StartCoroutine(DeckDelay(.5f)); // 0.5 saniyə gecikmə ilə paneli aç
    }

    IEnumerator DeckDelay(float delay)
    {
        MoveDown(Buttons,700f); // 100f qədər aşağı hərəkət et

        yield return new WaitForSeconds(delay);
        MoveDown(deckPanel, 0); // 100f qədər aşağı hərəkət et

        //MoveDown(deckPanel, 0); // 100f qədər aşağı hərəkət et

    }
    public void ExitClicked()
    {
        SoundManager.Instance.PlaySound(SoundType.Click);

        StartCoroutine(ExitDelay(3.5f)); // 0.5 saniyə gecikmə ilə paneli aç
    }
    public void Claim() { 
    
        GameManager.Instance.GetNewPiece.SetActive(false);
        GameManager.Instance.GameWin();
    }
    IEnumerator ExitDelay(float delay)
    {
        MoveDown(deckPanel, Screen.height + 500); // 100f qədər aşağı hərəkət et

        MoveDown(Buttons, 0); // 100f qədər aşağı hərəkət et
        yield return new WaitForSeconds(delay);
        MoveDown(Buttons, 0); // 100f qədər aşağı hərəkət et

    }
    public void NextLevel()
    {
        SceneManager.LoadScene(0);
    }

    public void LevelText(string x)
    {
        Level.text = x;
        MainLevel.text = "Level " + x;
    }
    public void UpdateCoinText(int coins)
    {
        Coin.text = coins.ToString();
        CoinGame.text = coins.ToString();
    }
}
