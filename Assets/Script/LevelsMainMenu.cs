using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsMainMenu : MonoBehaviour
{
    List<LevelData> levelsData;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[Serializable]
public class LevelData
{
    public int levelNumber;
    public string SpecialPiece;
    public int Coin;
}