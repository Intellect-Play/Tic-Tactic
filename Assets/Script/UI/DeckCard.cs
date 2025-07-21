using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckCard : MonoBehaviour
{
    public SpecialPieceType specialPieceType;
    public TextMeshProUGUI OpenLevel;
    public GameObject Unlock;
    public Image CardImage;

    private void Awake()
    {
        Unlock.SetActive(true);
        CardImage.gameObject.SetActive(false);
        CardImage.sprite = null; // Initialize with no sprite
    }
    public void GetLevel(int level)
    {
        if (OpenLevel != null)
        {
            OpenLevel.text = "Level " + level.ToString();
        }
        else
        {
            Debug.LogWarning("OpenLevel is not assigned in DeckCard.");
        }
    }
    public void GetSprite(Sprite sprite)
    {
        if (CardImage != null)
        {
            Unlock.SetActive(false);
            CardImage.gameObject.SetActive(true);

            CardImage.sprite = sprite;
        }
        else
        {
            Debug.LogWarning("CardImage is not assigned in DeckCard.");
        }
    }
}
