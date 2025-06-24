using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
  
    [SerializeField] public Board board;
    [SerializeField] private CellController cellController;
    [SerializeField] private EnemyAttack enemyAttack;
    [SerializeField] public PlayerController playerController;
    [SerializeField] public AIController aiController;
    [SerializeField] public AllSpecialPiecesMove allSpecialPiecesMove;
    [SerializeField] public PieceSpawner pieceSpawner;

    [SerializeField] public GameObject GameFinish;
    [SerializeField] public TextMeshProUGUI FinishText;
    public PieceType currentPlayer;

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
        cellController.cells = board.Cells;

    }


    public void EndTurn()
    {
        allSpecialPiecesMove.MoveSpecialPieces(() =>
        {
            EndTurnFunc();



        });
        //StartCoroutine(EndTurnTime());
    }
    public void EndTurnFunc() {
        StartCoroutine(CheckResult());
    }
    //IEnumerator EndTurnTime()
    //{
    //   // yield return StartCoroutine(CheckResult());
    //    Debug.Log("End Turn called, checking result...");
    //    //CheckResult();
    //    allSpecialPiecesMove.MoveSequentially(() =>
    //    {
    //        StartCoroutine(CheckResult());
    //        Debug.Log("All special pieces have moved.");
           
    //    });
    //}
    public void SwitchTurn()
    {
        currentPlayer = currentPlayer == PieceType.Player ? PieceType.Enemy : PieceType.Player;
        Debug.Log("Switching turn to: " + currentPlayer);
        
    }

    IEnumerator CheckResult()
    {
        yield return new WaitForSeconds(.1f);

        WinResult winResult = board.Cells.CheckWin();
        if (winResult.hasWon)
        {
            if(winResult.winner == PieceType.Player)
            {
                Debug.Log("Player wins!");
                enemyAttack.AttackPlayer(winResult.winCells);
            }
            else if (winResult.winner == PieceType.Enemy)
            {
               yield return new WaitForSeconds(.5f);
               Debug.Log("Enemy wins!");
                enemyAttack.AttackEnemy(winResult.winCells);

            }
            Debug.Log($"{winResult.winner} Wins!");
            yield return new WaitForSeconds(1.3f);
            //ShowResult($"{winResult.winner} Wins!");
        }
        else if (board.IsFull())
        {
            board.CleanBoard();
            yield return new WaitForSeconds(2f);
            ShowResult("Draw!");
        }
        else {
            SwitchTurn();
        }
        yield return new WaitForSeconds(.1f);
        aiController.MakeMove(currentPlayer);
        playerController.MakeMove(currentPlayer);
    }

    private void ShowResult(string message)
    {
      Debug.Log(message);
    }
    public void DiedCase(PieceType pieceType)
    {

        StartCoroutine(FinishTime(pieceType.ToString() + " id Died"));
    }
    IEnumerator FinishTime(string WinPlayer)
    {
        GameFinish.SetActive(true);
        FinishText.text = WinPlayer;

        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(0);

    }
}
public enum PieceType
{
    Null,
    Player,
    Enemy
}
