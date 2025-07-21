using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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
    [SerializeField] public Button EndTurnButton;

    [SerializeField] public GameObject GameFinishWin;
    [SerializeField] public GameObject GameFinishLose;
    [SerializeField] public GameObject GetNewPiece;
    [SerializeField] public Image GetNewPieceImage;

    [SerializeField] public TextMeshProUGUI FinishText;
    [SerializeField] public SpecialPieceController specialPieceController;

    public PieceType currentPlayer;
    public GameUnChangedData currenGameUnChangedData;
    public List<GameUnChangedData> gameUnChangedDatas;
    public bool EndTurnButtonPressed = false;
    public bool IsGameFinished = false;
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
        EndTurnButtonPressed = true;
    }

    private void Start()
    {
        currentPlayer = PieceType.Player;
        gameUnChangedDatas = GameDatas.Instance.Data.gameUnChangedDatas;
        currenGameUnChangedData = GameDatas.Instance.Data.gameUnChangedDatas[SaveDataService.CurrentLevel-1];
        GameActions.Instance.OnEndTurn += EndTurn;
        GameFinishLose.SetActive(false);
        GameFinishWin.SetActive(false);
        GetNewPiece.SetActive(false);

        //StartCoroutine(StartTime());
    }
    public void StartGame()
    {
        GameActions.Instance.InvokeStartGame();
        cellController.cells = board.Cells;
        Debug.Log("Game Started");
        //StartCoroutine(StartTime());
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
        if(IsGameFinished) return;
        cellController.IsPlacedCellPieces();
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
        EndTurnButton.interactable = false;        
        playerController.ChangeColorsActive(currentPlayer == PieceType.Player);
    }

    IEnumerator CheckResult()
    {
        yield return new WaitForSeconds(.1f);

        WinResult winResult = board.Cells.CheckWin();
        if (winResult.hasWon)
        {
            CameraShakeVibration.Instance.Shake();
            if (winResult.winner == PieceType.Player)
            {
                SaveDataService.Coins += (GameDatas.Instance.mainGameDatasSO.CoinGet* winResult.winCells.Count);
                UIManager.Instance.UpdateCoinText(SaveDataService.Coins);
                enemyAttack.AttackPlayer(winResult.winCells);
            }
            else if (winResult.winner == PieceType.Enemy)
            {
               yield return new WaitForSeconds(.5f);
                enemyAttack.AttackEnemy(winResult.winCells);

            }
            yield return new WaitForSeconds(1.3f);

            //ShowResult($"{winResult.winner} Wins!");
        }
        else if (board.IsFull())
        {
            board.CleanBoard();
            yield return new WaitForSeconds(2f);
        }
        else {
            SwitchTurn();
        }
        yield return new WaitForSeconds(.5f);
        aiController.MakeMove(currentPlayer);
        playerController.MakeMove(currentPlayer);
        //EndTurnButtonPressed = true;

    }

    private void ShowResult(string message)
    {
      Debug.Log(message);
    }

    public void DiedCase(PieceType pieceType)
    {
        if (IsGameFinished) return;
        Debug.Log("Game Over: " + pieceType);
        if (pieceType == PieceType.Enemy)
        {
            SaveDataService.CurrentLevel++;
            if (currenGameUnChangedData.PlayerSpecialUnlock != SpecialPieceType.Null)
            {
                Debug.Log("Player Special Unlock: " + currenGameUnChangedData.PlayerSpecialUnlock);
                var list = SaveDataService.UnlockedWeapons;
                list.Add(currenGameUnChangedData.PlayerSpecialUnlock);
                SaveDataService.UnlockedWeapons = list;
                GetNewPiece.SetActive(true);

                GetNewPieceImage.sprite = specialPieceController.specialPieces.Find(x => x.specialPieceType == currenGameUnChangedData.PlayerSpecialUnlock).XSprite;
                SaveDataService.Save();

                //SaveDataService.UnlockedWeapons.Add(currenGameUnChangedData.PlayerSpecialUnlock);
            }
            else
            {

                GameWin();
            }
        }
        else GameLose();





    }

    public void GameWin()
    {
        Debug.Log("Game Win");
        if (IsGameFinished) return;
        IsGameFinished = true;
        GameFinishWin.SetActive(true);

    }
    public void GameLose()
    {Debug.Log("Game Lose");
        if (IsGameFinished) return;
        IsGameFinished = true;
          StartCoroutine(FinishTime(false));
    }
    IEnumerator FinishTime(bool Win)
    {


        yield return new WaitForSeconds(2);
        if(Win)
        {
            GameFinishWin.SetActive(true);
            //FinishText.text = "You Win!";
        }
        else
        {
            GameFinishLose.SetActive(true);
            //FinishText.text = "You Lose!";
        }
        //SceneManager.LoadScene(0);

    }
}
public enum PieceType
{
    Null,
    Player,
    Enemy
}
