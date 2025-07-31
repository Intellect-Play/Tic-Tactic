using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Flame : SpecialPieceCore
{
    Board board;
    public override void Start()
    {
        base.Start();
        board = GameManager.Instance.board;
        //anim.Play();
    }

    public override void ChangeCell(Cell cell)
    {
        board = GameManager.Instance.board;
        base.ChangeCell(cell);
        board.SetMainColorCells();

        board.SetCellColor(PieceCell.x, PieceCell.y + 1, false);
        board.SetCellColor(PieceCell.x, PieceCell.y - 1, false);
    }
    public override void Back()
    {
        board = GameManager.Instance.board;
        base.Back();
        board.SetCellColor(PieceCell.x, PieceCell.y + 1, true);
        board.SetCellColor(PieceCell.x, PieceCell.y - 1, true);
    
    }
    public override void BackCell()
    {
        board = GameManager.Instance.board;
        base.BackCell();
        board.SetCellColor(PieceCell.x, PieceCell.y + 1, true);
        board.SetCellColor(PieceCell.x, PieceCell.y - 1, true);
 
    }
    public override void MoveStart(Action onMoveComplete)
    {
      
        StartCoroutine(FlameDelay(onMoveComplete));
    }
    IEnumerator FlameDelay(Action onMoveComplete)
    {
        animator.SetTrigger("Attack");
        animator.SetTrigger("FinalIdle");
        yield return new WaitForSeconds(0.5f); // Attack animasiyasının müddəti

        GameManager.Instance.board.DestroyPiece(PieceCell.x, PieceCell.y + 1, playerValue, specialPieceData);
        GameManager.Instance.board.DestroyPiece(PieceCell.x, PieceCell.y - 1, playerValue, specialPieceData);
        SoundManager.Instance.PlaySound(SoundType.Flame);
        yield return new WaitForSeconds(0.3f); // Attack animasiyasının müddəti

        MoveEnd(onMoveComplete);

    }
}
