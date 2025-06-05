using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
  
    [SerializeField]private Board board;
    [SerializeField]private EnemyAttack enemyAttack;

    public string currentPlayer = "X";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;           
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        GameActions.Instance.OnEndTurn += EndTurn;
        StartCoroutine(StartTime());
    }

    private void OnDisable()
    {
        GameActions.Instance.OnEndTurn -= EndTurn;
    }

    IEnumerator StartTime()
    {
        yield return new WaitForSeconds(1f);
        GameActions.Instance.InvokeStartGame();
   
    }


    public void EndTurn()
    {
        CheckResult();
        SwitchTurn();
    }
    public void SwitchTurn()
    {
        currentPlayer = currentPlayer == "X" ? "O" : "X";
    }

    public void CheckResult()
    {
        WinResult winResult = board.Cells.CheckWin();
        if (winResult.hasWon)
        {
            enemyAttack.AttackEnemy(winResult.winCells);
            ShowResult($"{winResult.winner} Wins!");
        }
        else if (board.IsFull())
        {
            ShowResult("Draw!");
        }
    }

    private void ShowResult(string message)
    {
      Debug.Log(message);
    }
}
