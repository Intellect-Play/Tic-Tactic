using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        Debug.Log("Bomb3 1");

        if (GameManager.Instance.currentPlayer != playerValue&& turnCount!=0) return;
        turnCount++;
        Debug.Log("Bomb3 2");
        if (turnCount >= 3)
        {
            AddToList();

        }
        else animator.SetTrigger("Bomb" + turnCount);

    }
    IEnumerator BombDelay()
    {
        animator.SetTrigger("Attack");
        animator.SetTrigger("FinalIdle");
        Image image = GetComponent<Image>();
        animator.enabled = false;
        AddToList();
        turnCount = 0;
        Debug.Log($"Bomb3Turn: {PieceCell.x}, {PieceCell.y} for {playerValue} - Turn Count: {turnCount}");
        yield return new WaitForSeconds(1f); // Bomb animasiyasının müddəti
        Debug.Log("Bomb3 turnCount  " + turnCount);
        image.sprite = playerValue == PieceType.Player ? specialPieceData.XSprite : specialPieceData.OSprite;


    }
    private void OnDestroy()
    {
        GameActions.Instance.OnEndTurn -= Turn;
        StopCoroutine(BombDelay());
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
        StartCoroutine(WaitForAttackComplete(onMoveComplete));
    }
    IEnumerator WaitForAttackComplete(Action onMoveComplete)
    {
      
        SoundManager.Instance.PlaySound(SoundType.Bomb);

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                // Ortadakı hüceyrəni keç (özünü silməsin)
                if (dx == 0 && dy == 0)
                    continue;

                int targetX = PieceCell.x + dx;
                int targetY = PieceCell.y + dy;
                if (targetX < 0 || targetX >= GameManager.Instance.board.boardSizeX || targetY < 0 || targetY >= GameManager.Instance.board.boardSizeY)
                    continue;

                GameManager.Instance.board.DestroyPiece(targetX, targetY, playerValue, specialPieceData);
            }
        }
        animator.SetTrigger("Attack");
        animator.SetTrigger("FinalIdle");
        Image image = GetComponent<Image>();
        animator.enabled = false;
        Debug.Log("Bomb3Turn: " + PieceCell.x + ", " + PieceCell.y + " for " + playerValue);
        turnCount = 0;
        Debug.Log($"Bomb3Turn: {PieceCell.x}, {PieceCell.y} for {playerValue} - Turn Count: {turnCount}");
      //  yield return new WaitForSeconds(1f); // Bomb animasiyasının müddəti
        Debug.Log("Bomb3 turnCount  " + turnCount);


        yield return new WaitForSeconds(0.5f); // Attack animasiyasının müddəti
        image.sprite = playerValue == PieceType.Player ? specialPieceData.XSprite : specialPieceData.OSprite;

        MoveEnd(onMoveComplete);
    }
}
