using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public TextMeshProUGUI Level;
    public TextMeshProUGUI Coin;
    [Header("Buttons")]
    public Button playButton;
    public Button deckButtonRight;
    public Button deckButtonLeft;
    public Button exitButton;

    [Header("Deck Panel")]
    public GameObject deckPanel;
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
        deckPanel.SetActive(false); // Başda bağlı olsun

        playButton.onClick.AddListener(OnPlayClicked);
        deckButtonRight.onClick.AddListener(ToggleDeckPanel);
        LevelText("Level: " + SaveDataService.CurrentLevel.ToString());

    }

    private void OnPlayClicked()
    {
        Debug.Log("Play clicked!");
        // Burada oyuna keçid və ya səhnə yükləmə kodu ola bilər
    }

    private void ToggleDeckPanel()
    {
        deckPanel.SetActive(!deckPanel.activeSelf);
    }


 
    public void LevelText(string x)
    {
        Level.text = x;
    }
    public void UpdateCoinText(int coins)
    {
        Coin.text = coins.ToString();
    }
}
