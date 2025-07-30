using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public PieceType cellValue;
    public PieceBase _PlayerPiece;

    [SerializeField] Sprite mainSprite;
    [SerializeField] Sprite redSprite;

    private Image _Image;

    private void Awake()
    {
        _Image = GetComponent<Image>();
    }
    public bool HasValue => !(cellValue==PieceType.Null);

    public int x, y;

    public void Init(int _x, int _y)
    {
        cellValue = PieceType.Null;
        x = _x;
        y = _y;
    }
    public void SetImage(bool isMain)
    {
        if (isMain)
        {
            _Image.sprite = mainSprite;
        }
        else
        {
            _Image.sprite = redSprite;
        }
    }

    public void SetValue(PieceBase value)
    {
       // if (HasValue) return;
        if(_PlayerPiece!=null) _PlayerPiece.RemoveCell();

        _PlayerPiece = value;
        cellValue = value.playerValue;

    }
    public void RemoveCell()
    {
        cellValue = PieceType.Null;
        if(_PlayerPiece != null)
        {

            _PlayerPiece.PieceCell = null;
            _PlayerPiece = null;
        }
        
    }

    public bool CheckCell()
    {
        if (HasValue)
        {
            return false;
        }
        return true;
    }
 
}
