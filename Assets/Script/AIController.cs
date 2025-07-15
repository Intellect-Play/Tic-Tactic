using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public static AIController Instance;

    public List<EnemiesUnChangedData> gameUnChangedDatas;

    public PieceType currentPlayer = PieceType.Enemy;

    public List<PieceBase> aiPieces = new List<PieceBase>();
    public List<CharacterBase> aiCharacters = new List<CharacterBase>();

    public List<RectTransform> aiCharactersSpawnPoints = new List<RectTransform>();
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
        GameActions.Instance.OnStartGame += EnemyStart;
    }
    private void OnDisable()
    {
        GameActions.Instance.OnStartGame -= EnemyStart;
    }
    public void EnemyStart() {

        gameUnChangedDatas = GameManager.Instance.currenGameUnChangedData.Enemies;

        for (int i = 0; i < gameUnChangedDatas.Count; i++)
        {
            Debug.Log("Enemy Start: " + i);
            CharacterBase enemyCharacterBase = CharacterDatas.Instance.SetupSpecial(Characters.EnemyYork);
            aiCharacters.Add(enemyCharacterBase);
            enemyCharacterBase.GetPosition(aiCharactersSpawnPoints[i],1-(0.1f*i));
        }

    }

    public void GetAiPiece(PieceBase piece)
    {
        aiPieces.Add(piece);
    }
    public void RemoveAiPiece(PieceBase piece)
    {
        if (aiPieces.Contains(piece))
        {
            aiPieces.Remove(piece);
        }
    }

    public bool CheckSizePieces()
    {
        if (aiPieces.Count == 0)
        {
            GameManager.Instance.pieceSpawner.SpawnEnemyPieces(GameDatas.Instance.mainGameDatasSO.SpawnCount);
            return true;
        }return false;
    }
    public void MakeMove(PieceType pieceType)
    {
        //CheckSizePieces();
        if (pieceType == currentPlayer)
        {
            aiPieces[0].ChangeCellDelay(GameManager.Instance.board.EmptyCell());
            aiPieces.RemoveAt(0);
            GameActions.Instance?.InvokeEndTurn();

            if (!CheckSizePieces())
            {
               
            }
          
        }
    }

}
