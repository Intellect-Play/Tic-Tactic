using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelLine : MonoBehaviour
{
    public List<GameUnChangedData> gameUnChangedDatas;
    int currentevel = 0;
    int currenChangedLevel = 0;

    [SerializeField] private List<LevelLineObject> levelLineObjects;

    [SerializeField] private Sprite GoldImage;

    SpecialPieceController specialPieceController;
    void Start()
    {
        currenChangedLevel = currentevel = SaveDataService.CurrentLevel;
        specialPieceController = GameManager.Instance.specialPieceController;
        gameUnChangedDatas = GameManager.Instance.gameUnChangedDatas;

        SetLevelLines();
    }

    void SetLevelLines()
    {

        for (int i = -3; i < levelLineObjects.Count - 3; i++)
        {
            currenChangedLevel = currentevel + i;
            if (currenChangedLevel >= 0)
            {
                if (gameUnChangedDatas[currenChangedLevel].PlayerSpecialUnlock != SpecialPieceType.Null)
                {
                    levelLineObjects[i + 3].GetLevelImageCharacters(i, currenChangedLevel, true, specialPieceController.specialPieces.Find(x => x.specialPieceType == gameUnChangedDatas[currenChangedLevel].PlayerSpecialUnlock).XSprite);
                }
                else
                {
                    levelLineObjects[i + 3].GetLevelImageCharacters(i, currenChangedLevel, false, GoldImage);
                }
            }
            else
            {
                levelLineObjects[i + 3].DontActive();

            }
        }
    }
}
