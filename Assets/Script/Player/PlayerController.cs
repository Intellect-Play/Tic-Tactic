using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

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
    public List<Image> Cells = new List<Image>();
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
        GameManager.Instance.EndTurnButton.GetIncretible(true);
    }
    private void EndTurn() { 
        if(currentPlayer != null)
        {
            currentPlayer.pieceImage.DOFade(true ? 1 : .5f, 0.2f).SetEase(Ease.OutQuad);

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
        //UIManager.Instance.LevelText(x + " Check " + playerPieces.Count);

        if (playerPieces.Count == 0)
        {
            GameManager.Instance.pieceSpawner.SpawnPlayerPieces(GameDatas.Instance.mainGameDatasSO.SpawnCount);
        }
        else foreach (var piece in playerPieces)
            {
                if (piece != null)
                {
                    piece.pieceImage.DOFade(1, 1).SetEase(Ease.OutQuad);
                }
            }
    }
    public void ChangeColorsActive(bool active)
    {
        foreach (var piece in playerPieces)
        {
            if (piece != null)
            {
                piece.pieceImage.DOFade(active ? 1 : .5f, 0.2f).SetEase(Ease.OutQuad);
            }
        }
        foreach (var piece in Cells)
        {
            if (piece != null)
            {
                piece.DOFade(active ? 1 : .5f, 0.1f).SetEase(Ease.OutQuad);
            }
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
        ActivePieces(pieceType == currentPlayerType);
        if(currentPlayerType != pieceType) return;
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
        if (!active)
        {
            foreach (var piece in playerPieces)
            {
                if (piece != null)
                {
                    piece.pieceImage.DOFade(.5f, 1).SetEase(Ease.OutQuad);
                }
            }
        }

    }
    private void InitPlayerPieces()
    {
 
        
        CharacterBase enemyCharacterBase = CharacterDatas.Instance.SetupSpecial(Characters.PlayerCat, GameManager.Instance.currenGameUnChangedData.PlayerHP);
        playerCharacters.Add(enemyCharacterBase);            
        enemyCharacterBase.GetPosition(playerCharactersSpawnPoints[0], 1);
       
    }
}
