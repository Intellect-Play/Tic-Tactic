using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public static AIController Instance;

    public PieceType currentPlayer = PieceType.Enemy;

    public List<PieceBase> aiPieces = new List<PieceBase>();
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

    public void CheckSizePieces()
    {
        if (aiPieces.Count == 0)
        {
            GameManager.Instance.pieceSpawner.SpawnEnemyPieces(GameDatas.Instance.mainGameDatasSO.SpawnCount);
        }
    }
    public void MakeMove(PieceType pieceType)
    {
        Debug.Log("AI Make Move: " + pieceType);
        CheckSizePieces();
        if (pieceType == currentPlayer)
        {
            aiPieces[0].ChangeCellDelay(GameManager.Instance.board.EmptyCell());
            aiPieces.RemoveAt(0);
            GameActions.Instance?.InvokeEndTurn();
        }
    }
}
