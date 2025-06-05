using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    [SerializeField]private MainGameDatas mainGameDatas;
    [SerializeField]private GameObject cellPrefab;

    public List<Cell> Cells = new List<Cell>();

    private void OnEnable()
    {
        GameActions.Instance.OnStartGame += Generate;
    }

    private void OnDisable()
    {
        GameActions.Instance.OnStartGame -= Generate;
    }
 

    public void Generate()
    {
        for (int i = 0; i < mainGameDatas.BoardSize; i++)
        {
            for (int j = 0; j < mainGameDatas.BoardSize; j++)
            {
                GameObject cellObject = Instantiate(cellPrefab, transform);
                Cell cell = cellObject.GetComponent<Cell>();
                cell.cellValue = PieceType.Null;
                Cells.Add(cell);
            }
        }
    }


    public bool IsFull()
    {
        foreach (var cell in Cells)
            if (!cell.HasValue) return false;

        return true;
    }
}
