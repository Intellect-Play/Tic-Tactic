using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderGun : SpecialPieceCore
{
    Board board;
    public override void Start()
    {
        base.Start();
        board = GameManager.Instance.board;
        //anim.Play();
    }


    //public override void ChangeCell(Cell cell)
    //{
    //    board = GameManager.Instance.board;
    //    base.ChangeCell(cell);
    //    board.SetMainColorCells();
    //    if (PieceCell == null) return;

    //    board.SetCellColor(PieceCell.x + 1, PieceCell.y + 1, false);
    //    board.SetCellColor(PieceCell.x + 1, PieceCell.y - 1, false);
    //    board.SetCellColor(PieceCell.x - 1, PieceCell.y + 1, false);
    //    board.SetCellColor(PieceCell.x - 1, PieceCell.y - 1, false);
    //}
    //public override void Back()
    //{
    //    board = GameManager.Instance.board;
    //    base.Back();
    //    if (PieceCell != null)
    //    {
    //        board.SetCellColor(PieceCell.x + 1, PieceCell.y + 1, true);
    //        board.SetCellColor(PieceCell.x + 1, PieceCell.y - 1, true);
    //        board.SetCellColor(PieceCell.x - 1, PieceCell.y + 1, true);
    //        board.SetCellColor(PieceCell.x - 1, PieceCell.y - 1, true);
    //    }
    
    //}
    //public override void BackCell()
    //{
    //    board = GameManager.Instance.board;
    //    base.BackCell();
    //    if (PieceCell != null)
    //    {
    //        board.SetCellColor(PieceCell.x + 1, PieceCell.y + 1, true);
    //        board.SetCellColor(PieceCell.x + 1, PieceCell.y - 1, true);
    //        board.SetCellColor(PieceCell.x - 1, PieceCell.y + 1, true);
    //        board.SetCellColor(PieceCell.x - 1, PieceCell.y - 1, true);
    //    }
    //}
    public override void MoveStart(Action onMoveComplete)
    {
        StartCoroutine(WaitForAttackComplete(onMoveComplete));
    }
    IEnumerator WaitForAttackComplete(Action onMoveComplete)
    {
        animator.SetTrigger("Attack");
        animator.SetTrigger("FinalIdle");
        yield return new WaitForSeconds(0.5f); // Attack animasiyasının müddəti
        SoundManager.Instance.PlaySound(SoundType.Thunder);

        board.DestroyPiece(PieceCell.x + 1, PieceCell.y + 1, playerValue, specialPieceData);
        board.DestroyPiece(PieceCell.x + 1, PieceCell.y - 1, playerValue, specialPieceData);
        board.DestroyPiece(PieceCell.x - 1, PieceCell.y + 1, playerValue, specialPieceData);
        board.DestroyPiece(PieceCell.x - 1, PieceCell.y - 1, playerValue, specialPieceData);
        yield return new WaitForSeconds(0.3f); // Attack animasiyasının müddəti

        MoveEnd(onMoveComplete);
    }
}
