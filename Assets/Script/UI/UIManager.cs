using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public TextMeshProUGUI Level;

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
        LevelText("Level: " + SaveDataService.Current.CurrentLevel.ToString());
    }
    public void LevelText(string x)
    {
        Level.text = x;
    }
}
