using System;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpawnButtons : MonoBehaviour
{
    public PieceBase pieceBase;
    public GameObject BuyPieceImage;
    public Button BuyButton;
    [SerializeField] TextMeshProUGUI CountText;
    [SerializeField] TextMeshProUGUI CoinText;
    // Start is called before the first frame update
    public void BuyPiece(Action<PlayerSpawnButtons> buyEvent,int cost,int num)
    {
        Debug.Log("Buy Piece " + num + " " + cost);
        if (BuyPieceImage.activeSelf) return;
      
        pieceBase = null;
        BuyPieceImage.SetActive(true);
        CountText.text = num.ToString();
        CoinText.text = cost.ToString();
        BuyButton.onClick.AddListener(() =>
        {
            buyEvent.Invoke(this);
            
        });
    }
    public void ResetButton()
    {
        pieceBase = null;
        BuyPieceImage.SetActive(false);
        BuyButton.onClick.RemoveAllListeners();

    }
    public void GetPiece(PieceBase _pieceBase)
    {
        pieceBase = _pieceBase;
        _pieceBase.playerSpawnButtons = this; // Set the reference to this PlayerSpawnButtons
        BuyPieceImage.SetActive(false);
        BuyButton.onClick.RemoveAllListeners();

    }
}

