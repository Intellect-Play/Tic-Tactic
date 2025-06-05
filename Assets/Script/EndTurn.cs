using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurn : MonoBehaviour
{
    // Start is called before the first frame update
    Button button;
    private void Start()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnEndTurnButtonClicked);
        }
    }
    public void OnEndTurnButtonClicked()
    {
        //Debug.Log("End Turn button clicked");
        GameActions.Instance.InvokeEndTurn();
    }

}
