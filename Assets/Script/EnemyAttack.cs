using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public RectTransform EnnemyAttackPoint;
    public void Attack(List<Cell> cells)
    {
        StartCoroutine(AttackTime(cells));
    }
    IEnumerator AttackTime(List<Cell> cells)
    {
        foreach (Cell cell in cells)
        {
            cell._PlayerPiece.AttackDestroy(EnnemyAttackPoint);
            yield return new WaitForSeconds(.1f); // Delay between attacks
        }
        // Implement attack logic here
        Debug.Log("Enemy attacks!");
    }
}
