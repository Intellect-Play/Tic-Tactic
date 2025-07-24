using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<DeckCard> DeckCards = new List<DeckCard>();
    public SpecialPieceController specialPieceController;
    List<int> levels = new List<int>
{
    1, 7, 13, 18, 24, 30, 36, 41, 47, 53, 59, 64, 70, 76, 82, 87, 93, 99, 105, 110, 116, 122, 128, 133, 139, 145, 150
};

    private void Start()
    {
        GetDeckCards();
        //List<int> levels = new List<int> { 1, 3, 5, 7, 10 };
        SetDeckCardLevels(levels);
    }
    public void GetDeckCards()
    {
        for (int i = 0; i < SaveDataService.UnlockedWeapons.Count; i++)
        {
            DeckCards[i].GetSprite(specialPieceController.specialPieces.Find(x => x.specialPieceType == SaveDataService.UnlockedWeapons[i]).XSprite);

        }
     
    }
    public void SetDeckCardLevels(List<int> levelList)
    {
        for (int i = 0; i < levelList.Count; i++)
        {
            DeckCards[i].GetLevel(levelList[i]);
        }
    }

}
