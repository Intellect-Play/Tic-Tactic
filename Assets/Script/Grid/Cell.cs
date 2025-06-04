using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour
{
    public string cellValue;
    public PlayerPiece _PlayerPiece;
 
    public bool HasValue => !string.IsNullOrEmpty(cellValue);

    private int x, y;

    public void Init(int _x, int _y)
    {
        x = _x;
        y = _y;
    }

    public void SetValue(PlayerPiece value)
    {
        if (HasValue) return;

        _PlayerPiece = value;
        cellValue = value.playerValue;

    }
    public void RemoveCell()
    {
        cellValue = string.Empty;
        if(_PlayerPiece != null)
        {
            Debug.Log($"Removing cell at ({x}, {y}) with value: {cellValue}");

            _PlayerPiece.PieceCell = null;
            _PlayerPiece = null;
        }
        
    }

    public bool CheckCell()
    {
        if (HasValue)
        {
            Debug.Log($"Cell at ({x}, {y}) already has a value: {cellValue}");
            return false;
        }
        Debug.Log($"Cell at ({x}, {y}) is empty, can set value.");
        return true;
    }
 
}
