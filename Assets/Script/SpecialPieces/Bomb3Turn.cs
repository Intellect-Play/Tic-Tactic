using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb3Turn : SpecialPieceCore
{
    int turnCount = 0;
    private void Start()
    {
        GameActions.Instance.OnEndTurn += Turn;
    }

    private void OnDisable()
    {
        GameActions.Instance.OnEndTurn -= Turn;
    }
    public void Turn() { 
    
        turnCount++;
        if (turnCount >= 2)
        {
            Debug.Log("Bomb3Turn Turn");
            AddToList();
            turnCount = 0;
        }
    }
    public override void ChangeCell(Cell cell)
    {
        base.ChangeCell(cell);
        //AddToList();
    }
    public override void MoveStart(Action onMoveComplete)
    {
        Debug.Log("Bomb3Turn MoveStart");
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                // Ortadakı hüceyrəni keç (özünü silməsin)
                if (dx == 0 && dy == 0)
                    continue;

                int targetX = PieceCell.x + dx;
                int targetY = PieceCell.y + dy;

                GameManager.Instance.board.DestroyPiece(targetX, targetY, playerValue);
            }
        }

        MoveEnd(onMoveComplete);
    }
}
