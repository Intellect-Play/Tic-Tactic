using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelLineObject : MonoBehaviour
{
    [SerializeField] private Image SelectedImage;
    [SerializeField] private Image CardImage;
    [SerializeField] private GameObject CardEffect;
    [SerializeField] private TextMeshProUGUI LevelText;

    public void GetLevelImageCharacters(int mainNum,int Num, bool Active, Sprite sprite)
    {
        CardEffect.SetActive(Active);
        CardImage.sprite = sprite;
        LevelText.text = (Num+1).ToString();
        SelectedImage.enabled =(mainNum == -1);
        int size = Active ? 450 : 300;
        CardImage.GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
    }
    public void DontActive()
    {
        LevelText.text = "0";

        CardEffect.SetActive(false);
        SelectedImage.gameObject.SetActive(false);
    }
}
