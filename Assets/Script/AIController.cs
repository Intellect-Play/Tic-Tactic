
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AIController : MonoBehaviour
{
    public static AIController Instance;

    public List<EnemiesUnChangedData> gameUnChangedDatas;

    public PieceType currentPlayer = PieceType.Enemy;

    public List<PieceBase> aiPieces = new List<PieceBase>();
    public List<CharacterBase> aiCharacters = new List<CharacterBase>();

    public RectTransform aiPieceParent;
    public List<RectTransform> aiCharactersSpawnPoints = new List<RectTransform>();

    [SerializeField] TextMeshProUGUI enemyCountText;
    bool DiedAIBool;
    int enemyMaxCount;
    int enemyCount;

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
        DiedAIBool = false;
    }
    private void Start()
    {
        EnemyStart();
        //GameActions.Instance.OnStartGame += EnemyStart;
    }
    private void OnDisable()
    {
        //GameActions.Instance.OnStartGame -= EnemyStart;
    }
    public void EnemyStart()
    {
        gameUnChangedDatas = new List<EnemiesUnChangedData>();
        enemyMaxCount = GameManager.Instance.currenGameUnChangedData.Enemies.Count;
        enemyCount = 1;
        foreach (var enemy in GameManager.Instance.currenGameUnChangedData.Enemies)
        {
            var enemyCopy = new EnemiesUnChangedData
            {
                EnemyHP = enemy.EnemyHP,
                EnemySpecials = new List<SpecialPieceType>(enemy.EnemySpecials),
                WinEnemy = enemy.WinEnemy
            };

            gameUnChangedDatas.Add(enemyCopy);
        }

        // Düşmən rotasiyası üçün dövr edən sıranı yarat
        Characters[] characterCycle = new Characters[]
        {
        Characters.Pyramid,
        Characters.Vodo,
        Characters.EnemyOrk,
        Characters.EnemyYork // Əgər 4-cü düşmənin yoxdursa, dəyişdir və ya sil
        };

      

        for (int i = 0; i < gameUnChangedDatas.Count; i++)
        {
            Characters selectedCharacter = characterCycle[Random.Range(0, characterCycle.Length)];

            CharacterBase enemyCharacterBase = CharacterDatas.Instance.SetupSpecial(selectedCharacter, gameUnChangedDatas[i].EnemyHP);
            aiCharacters.Add(enemyCharacterBase);

            enemyCharacterBase.GetPosition(aiCharactersSpawnPoints[i], 1 - (0.3f * i));
        }
        GetEnemyCountText();
    }
    private void GetEnemyCountText()
    {
        if (enemyCount > enemyMaxCount) return;
        enemyCountText.text = "Enemy: "+ enemyCount.ToString()+"/"+ enemyMaxCount;
        enemyCount++;
    }

    public void Damage(float health)
    {
        gameUnChangedDatas[0].EnemyHP = (int)health;
        aiCharacters[0].Damage(health);


    }
    public void DiedAI()
    {
        aiCharacters[0].DestroyPiece();
        DiedAIBool = true;
        GetEnemyCountText();   
    }
    public void Attack() {
        aiCharacters[0].Attack(PlayerController.Instance.playerCharacters[0].MainCharacter.GetComponent<RectTransform>());

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
    public void RemoveAllAiPieces()
    {
        foreach(var aiPiece in aiPieces)
        {
            Destroy(aiPiece.gameObject);
        }
        aiPieces.Clear();
        GameManager.Instance.pieceSpawner.SpawnEnemyPieces(GameDatas.Instance.mainGameDatasSO.SpawnCount);

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
        if (DiedAIBool) {
            aiCharacters.RemoveAt(0);
            gameUnChangedDatas.RemoveAt(0);
            if (aiCharacters.Count <= 0)
            {
                Health.Instance.isLive = false;
                GameManager.Instance.DiedCase(PieceType.Enemy);
            }
            else
            {
                Health.Instance.Heal(2,PieceType.Player);
                Health.Instance.InitEnemy(gameUnChangedDatas[0].EnemyHP);
                GameManager.Instance.RunPlayer();
                aiCharacters[0].MovePosition(aiCharactersSpawnPoints[0]);
                RemoveAllAiPieces();
            }
        }
        DiedAIBool = false;
        //CheckSizePieces();
        if (pieceType == currentPlayer)
        {
            aiPieces[0].ChangeCellDelay(GameManager.Instance.board.EmptyCell(gameUnChangedDatas[0].WinEnemy));
            //aiPieces[0].ChangeCellDelay(XOEnemyMoveSuggester.GetBestMove(board, PieceType.Enemy, PieceType.Player));

            aiPieces.RemoveAt(0);
            GameActions.Instance?.InvokeEndTurn();

            if (!CheckSizePieces())
            {
               
            }
          
        }
    }

}
