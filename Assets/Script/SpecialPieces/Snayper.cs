using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snayper : SpecialPieceCore
{
    Board board;
    public override void Start()
    {
        base.Start();
        board = GameManager.Instance.board;
    }
  
    public override void MoveStart(Action onMoveComplete)
    {
        StartCoroutine(WaitForAttackComplete(onMoveComplete));
    }
    IEnumerator WaitForAttackComplete(Action onMoveComplete)
    {
        animator.SetTrigger("Attack");
        animator.SetTrigger("FinalIdle");
        yield return new WaitForSeconds(0.5f); // Attack animasiyasının müddəti
        PieceType pieceType = playerValue == PieceType.Player ? PieceType.Enemy : PieceType.Player;
        List<Cell> EnemyCells = board.Cells.FindAll(c => c._PlayerPiece != null && c._PlayerPiece.playerValue == pieceType);
        if (EnemyCells.Count != 0)
        {
            SoundManager.Instance.PlaySound(SoundType.Shoot);


            Cell PieceCell = EnemyCells[UnityEngine.Random.Range(0, EnemyCells.Count)];



            board.DestroyPiece(PieceCell.x, PieceCell.y, playerValue, specialPieceData);

        }
        MoveEnd(onMoveComplete);
    }

}
