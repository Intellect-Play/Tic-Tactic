using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    private void OnEnable()
    {
        GameActions.Instance.OnEndTurn += EndTurn;
    }

    private void OnDisable()
    {
        GameActions.Instance.OnEndTurn -= EndTurn;
    }

    private void Start()
    {
        GameActions.Instance.InvokeStartGame();
   
    }


    void EndTurn()
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
            enemyAttack.Attack(winResult.winCells);
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
