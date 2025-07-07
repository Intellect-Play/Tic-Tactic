using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI Level;

    private void Start()
    {
        Level.text = "Level: " + SaveDataService.Current.CurrentLevel.ToString();
    }
}
