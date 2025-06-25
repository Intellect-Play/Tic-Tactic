using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellController : MonoBehaviour
{
    public List<Cell> cells;

    //private void Start()
    //{
    //    GameActions.Instance.OnEndTurn += IsPlacedCellPieces;
    //}
    //private void OnDisable()
    //{
    //    GameActions.Instance.OnEndTurn -= IsPlacedCellPieces;
    //}
    public void IsPlacedCellPieces()
    {
        foreach (var cell in cells)
        {
            if (cell._PlayerPiece != null)
            {
                cell._PlayerPiece.Placed(this);
            }
        }
    }
}
