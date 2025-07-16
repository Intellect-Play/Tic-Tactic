using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public List<PieceBase> playerPieces = new List<PieceBase>();

    public PieceType currentPlayerType = PieceType.Player;
    public PieceBase currentPlayer;
    int x = 0;
    public List<SpecialPieceType> UnlockedWeapons;

    public List<CharacterBase> playerCharacters = new List<CharacterBase>();

    public List<RectTransform> playerCharactersSpawnPoints = new List<RectTransform>();
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
        UnlockedWeapons = SaveDataService.UnlockedWeapons;
        GameActions.Instance.OnEndTurn += EndTurn;
        GameActions.Instance.OnStartGame += InitPlayerPieces;
    }
    private void OnDisable()
    {
        GameActions.Instance.OnEndTurn -= EndTurn;
        GameActions.Instance.OnStartGame -= InitPlayerPieces;
    }

    public void GetPiece(PieceBase piece)
    {
        if (currentPlayer != null && !currentPlayer.IsPlaced && piece!=currentPlayer)
        {
            currentPlayer.Back();
        }
        currentPlayer = piece;
        GameManager.Instance.EndTurnButtonPressed = true;

    }
    private void EndTurn() { 
        if(currentPlayer != null)
        {
            playerPieces.Remove(currentPlayer);
        }
    }
    public void RemoveAllPlayerPieces()
    {
        foreach (var aiPiece in playerPieces)
        {
            Destroy(aiPiece.gameObject);
        }
        playerPieces.Clear();

    }
    public void CheckSizePieces()
    {
        x++;
        UIManager.Instance.LevelText(x+ " Check "+playerPieces.Count);

        if (playerPieces.Count == 0)
        {
            GameManager.Instance.pieceSpawner.SpawnPlayerPieces(GameDatas.Instance.mainGameDatasSO.SpawnCount);
        }
    }

    public void Attack()
    {
        playerCharacters[0].Attack(AIController.Instance.aiCharacters[0].MainCharacter.GetComponent<RectTransform>());
    }

    public void Damage(float health)
    {
        playerCharacters[0].Damage(health);

        
    }
    public void MakeMove(PieceType pieceType) {
        Debug.Log(pieceType.ToString());
        ActivePieces(pieceType == currentPlayerType);
        GameManager.Instance.pieceSpawner.CheckBuy();
        CheckSizePieces();
    }
    public void ActivePieces(bool active) {
        foreach (var piece in playerPieces)
        {
            if (piece != null)
            {
                piece.ActivePiece(active);
            }
        }


    }
    private void InitPlayerPieces()
    {
 
        
        CharacterBase enemyCharacterBase = CharacterDatas.Instance.SetupSpecial(Characters.PlayerCat);
        playerCharacters.Add(enemyCharacterBase);            
        enemyCharacterBase.GetPosition(playerCharactersSpawnPoints[0], 1);
       
    }
}
