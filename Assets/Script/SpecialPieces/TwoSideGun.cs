using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoSideGun : SpecialPieceCore
{
    public Board board;
    private void OnEnable()
    {
        board = GameManager.Instance.board;

    }
    public override void Start()
    {
        base.Start();
        size = 2.4f;
        ShowPopupBounce(sizeX);

        //GetComponent<RectTransform>().localScale = new Vector2(2.4f, 2.4f);
    }
    //public override void ChangeCell(Cell cell)
    //{
    //    base.ChangeCell(cell);
    //    Board.Instance.SetMainColorCells();
    //    if (PieceCell == null) return;

    //    Board.Instance.SetCellColor(PieceCell.x + 1, PieceCell.y, false);
    //    Board.Instance.SetCellColor(PieceCell.x - 1, PieceCell.y, false);

    //}
    //public override void Back()
    //{
    //    if(PieceCell != null)
    //    {
    //        Board.Instance.SetCellColor(PieceCell.x + 1, PieceCell.y, true);
    //        Board.Instance.SetCellColor(PieceCell.x - 1, PieceCell.y, true);
    //    }
     
    //    base.Back();

    //}
    //public override void BackCell()
    //{
    //    if (PieceCell != null)
    //    {
    //        Board.Instance.SetCellColor(PieceCell.x + 1, PieceCell.y, true);
    //        Board.Instance.SetCellColor(PieceCell.x - 1, PieceCell.y, true);
    //    }
    //    base.BackCell();

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
        SoundManager.Instance.PlaySound(SoundType.Shoot);

        board.DestroyPiece(PieceCell.x + 1, PieceCell.y, playerValue, specialPieceData);
        board.DestroyPiece(PieceCell.x - 1, PieceCell.y, playerValue, specialPieceData);
        yield return new WaitForSeconds(0.3f); // Attack animasiyasının müddəti
        MoveEnd(onMoveComplete);
    }

}
