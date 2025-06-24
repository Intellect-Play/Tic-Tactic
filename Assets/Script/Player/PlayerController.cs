using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public List<PieceBase> playerPieces = new List<PieceBase>();

    public PieceType currentPlayerType = PieceType.Player;
    public PieceBase currentPlayer;
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
      
    }
    private void EndTurn() { 
        if(currentPlayer != null)
        {
            playerPieces.Remove(currentPlayer);
        }
    }
    public void CheckSizePieces()
    {
        
        if (playerPieces.Count == 0)
        {
            GameManager.Instance.pieceSpawner.SpawnPlayerPieces(GameDatas.Instance.mainGameDatasSO.SpawnCount);
        }
    }
    public void MakeMove(PieceType pieceType) {
        CheckSizePieces();
        ActivePieces(pieceType == currentPlayerType);
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
       
    }
}
