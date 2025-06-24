using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public RectTransform EnnemyAttackPoint;

    public RectTransform PlayerAttackPoint;

    public void Attack(List<Cell> cells, PieceType playerType)
    {
        if (playerType == PieceType.Enemy)
        {
            AttackEnemy(cells);
        }
        else if (playerType == PieceType.Player)
        {
            AttackPlayer(cells);
        }
        else
        {
            Debug.LogError("Invalid player type for attack.");
        }
    }
    public void AttackEnemy(List<Cell> cells)
    {
        StartCoroutine(AttackTime(cells, EnnemyAttackPoint,0));
    }
    public void AttackPlayer(List<Cell> cells)
    {
        StartCoroutine(AttackTime(cells, PlayerAttackPoint,1));
    }
    IEnumerator AttackTime(List<Cell> cells, RectTransform rectTransform,float time)
    {

        foreach (Cell cell in cells)
        {
            cell._PlayerPiece.AttackDestroy(EnnemyAttackPoint);
            yield return new WaitForSeconds(.1f); 
        }
        // Implement attack logic here
        Debug.Log("Enemy attacks!");
    }
}
