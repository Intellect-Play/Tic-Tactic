using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoSideGun : SpecialPieceCore
{
    Board board;
    public override void Start()
    {
        base.Start();
        size = 2.4f;
        board = GameManager.Instance.board;
        ShowPopupBounce(sizeX);

        //GetComponent<RectTransform>().localScale = new Vector2(2.4f, 2.4f);
    }
    public override void ChangeCell(Cell cell)
    {
        base.ChangeCell(cell);

        board.SetMainColorCells();
        Debug.Log($"ChangeCell: {PieceCell.x}, {PieceCell.y} for {playerValue}");
        board.SetCellColor(PieceCell.x + 1, PieceCell.y, false);
        board.SetCellColor(PieceCell.x - 1, PieceCell.y, false);

    }
    public override void Back()
    {

        board.SetCellColor(PieceCell.x + 1, PieceCell.y, true);
        board.SetCellColor(PieceCell.x - 1, PieceCell.y, true);
        base.Back();

    }
    public override void BackCell()
    {

        board.SetCellColor(PieceCell.x + 1, PieceCell.y, true);
        board.SetCellColor(PieceCell.x - 1, PieceCell.y, true);
        base.BackCell();

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
        SoundManager.Instance.PlaySound(SoundType.Shoot);

        board.DestroyPiece(PieceCell.x + 1, PieceCell.y, playerValue, specialPieceData);
        board.DestroyPiece(PieceCell.x - 1, PieceCell.y, playerValue, specialPieceData);
        yield return new WaitForSeconds(0.3f); // Attack animasiyasının müddəti
        MoveEnd(onMoveComplete);
    }

}
