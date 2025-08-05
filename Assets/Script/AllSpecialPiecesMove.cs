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

        for (int i = 0; i < allMovePieces.Count; i++)
        {
            bool isDone = false;
            if(allMovePieces[i] == null)
            {
                Debug.LogWarning($"Piece at index {i} is null. Skipping.");
                continue;
            }
            allMovePieces[i].MoveStart(() =>
            {
                //Debug.Log($"Piece {piece.name} has finished moving.");
                // Bu callback MoveEnd çağırıldıqda gələcək
                isDone = true;
                // Parçanı siyahıdan sil
            });
            // Gözlə bu piece bitirsin
            yield return new WaitUntil(() => isDone);
            //Debug.Log($"Moving piece: {piece.name}");
        }
       
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
/*
1 2side
4 thunder
8 flame
12 snayper
16 plus2
20 healer
25 bomb3turn
30 random
35 immortal

*/