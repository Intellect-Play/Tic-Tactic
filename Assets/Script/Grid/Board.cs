
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{

    [SerializeField]private int boardSizeX;
    [SerializeField] private int boardSizeY;

    [SerializeField]private GameObject cellPrefab;
    GridLayoutGroup gridLayoutGroup;
    public List<Cell> Cells = new List<Cell>();
    public Cell[,] CellArray;
    private List<Cell> emptyCells = new List<Cell>();

    private void Start()
    {
       
    }
    private void OnEnable()
    {
        boardSizeX = GameDatas.Instance.mainGameDatasSO.BoardSizeX;
        boardSizeY = GameDatas.Instance.mainGameDatasSO.BoardSizeY;

        CellArray = new Cell[boardSizeX, boardSizeY];
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
        gridLayoutGroup.constraintCount = boardSizeX;
        GameActions.Instance.OnStartGame += Generate;

    }
    private void OnDisable()
    {
        GameActions.Instance.OnStartGame -= Generate;
    }
 

    public void Generate()
    {
        for (int i = 0; i < boardSizeY; i++)
        {
            for (int j = 0; j < boardSizeX; j++)
            {
                GameObject cellObject = Instantiate(cellPrefab, transform);
                Cell cell = cellObject.GetComponent<Cell>();             
                cell.Init(j, i);
                Cells.Add(cell);
                CellArray[j,i] = cell;
            }
        }
    }
    public void DestroyPiece(int x, int y,PieceType pieceType)
    {
        Cell cell = GetCell(x, y);
        if (cell == null)
        {
            return;
        }
        if (cell != null && cell._PlayerPiece != null&&cell._PlayerPiece.playerValue!=pieceType)
        {
            cell._PlayerPiece.DestroyPiece();
        }
    }
    public void DestroyAllPiece(PieceType pieceType)
    {
        foreach (var cell in Cells)
        {
            if (cell != null && cell._PlayerPiece != null && cell._PlayerPiece.playerValue != pieceType)
            {
                //Debug.Log($"Destroying piece at coordinates ({x}, {y})");
                cell._PlayerPiece.DestroyPiece();
            }
        }
      
    }
    Cell GetCell(int x, int y)
    {
        //Debug.Log($"Getting cell at coordinates ({x}, {y})cdsc"+boardSize);
        if (x < 0 || x >= boardSizeX || y < 0 || y >= boardSizeY)
        {
            return null;
        }
        //Debug.Log($"2  Getting cell at coordinates ({x}, {y})cdsc" + boardSize);

        return CellArray[x,y];
    }
    public Cell EmptyCell()
    {
        emptyCells.Clear();
        foreach (var cell in Cells)
        {
            if (!cell.HasValue)
            {
                emptyCells.Add(cell);
            }
        }
        if (emptyCells.Count>0)
        {
            return emptyCells[UnityEngine.Random.Range(0, emptyCells.Count)];
        }
        else return null;
    }
    public void CleanBoard() {

        foreach (var cell in Cells)
        {
            if(cell._PlayerPiece!=null)
            cell._PlayerPiece.DestroyPiece();
        }
    }
    public bool IsFull()
    {
        foreach (var cell in Cells)
            if (!cell.HasValue) return false;

        return true;
    }
}
