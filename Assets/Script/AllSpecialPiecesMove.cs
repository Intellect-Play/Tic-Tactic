using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script;
using UnityEngine;

public class AllSpecialPiecesMove : MonoBehaviour
{
    public static AllSpecialPiecesMove Instance { get; private set; }
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

    public List<SpecialPieceCore> allMovePieces = new List<SpecialPieceCore>();
    void Start()
    {
        
    }

    public void MoveSpecialPieces(Action onAllComplete2)
    {
        StartCoroutine(MoveSequentially( onAllComplete2));
    }

    public IEnumerator MoveSequentially(Action onAllComplete)
    {

        //Debug.Log("allMovePieces " + allMovePieces.Count);
        foreach (var piece in allMovePieces)
        {
           

            bool isDone = false;
            //Debug.Log($"Moving piece: {piece.name}");
            piece.MoveStart(() =>
            {
                //Debug.Log($"Piece {piece.name} has finished moving.");
                // Bu callback MoveEnd çağırıldıqda gələcək
                isDone = true;
                 // Parçanı siyahıdan sil
            });

            // Gözlə bu piece bitirsin
            yield return new WaitUntil(() => isDone);
        }
        //Debug.Log("allMovePieces " + allMovePieces.Count);
        allMovePieces.Clear();
        onAllComplete?.Invoke();
    }

    public void AddPiece(SpecialPieceCore piece)
    {
        if (!allMovePieces.Contains(piece))
        {
            allMovePieces.Add(piece);
        }
    }

    public void RemovePiece(SpecialPieceCore piece)
    {
        if (allMovePieces.Contains(piece))
        {
            allMovePieces.Remove(piece);
        }
    }
}
