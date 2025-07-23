using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelLine : MonoBehaviour
{
    public List<GameUnChangedData> gameUnChangedDatas;
    int currentevel = 0;
    [SerializeField] private GameObject[] levelLines;
    [SerializeField] private TextMeshProUGUI[] levelLineTexts;
    void Start()
    {
        gameUnChangedDatas = GameManager.Instance.gameUnChangedDatas;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
