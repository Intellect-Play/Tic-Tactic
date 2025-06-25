using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb3Turn : SpecialPieceCore
{
    int turnCount = 0;
    public override void Start()
    {
        base.Start();
        
    }

    private void OnDisable()
    {
        GameActions.Instance.OnEndTurn -= Turn;
    }
    public void Turn() {
        Debug.Log("Bomb3Turn");

        turnCount++;
        if (turnCount >= 4)
        {
            Debug.Log("Bomb3Turn Turn");
            AddToList();
            turnCount = 0;
        }
    }
    public override void Placed(bool _isPlaced)
    {

        if (!IsPlaced && _isPlaced)
        {
            GameActions.Instance.OnEndTurn += Turn;

        }
        IsPlaced = _isPlaced;
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
