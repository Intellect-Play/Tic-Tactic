using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    [SerializeField] private GameObject PlayerPiecePrefab;
    [SerializeField] private GameObject EnemyPiecePrefab;

    [SerializeField] private Transform PlayerPieceParent;
    [SerializeField] private Transform EnemyPieceParent;

    public int StartSpawnCount = 5;
    private void Start()
    {
        GameActions.Instance.OnStartGame += StartSpawn;
    }
  
    private void OnDisable()
    {
        GameActions.Instance.OnStartGame -= StartSpawn;
    }

    public void StartSpawn()
    {
        //Debug.Log("Spawning pieces...");
        SpawnPlayerPieces(StartSpawnCount);
        SpawnEnemyPieces(StartSpawnCount);
    }
    
    public void SpawnPlayerPieces(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject piece = Instantiate(PlayerPiecePrefab, PlayerPieceParent);
            RectTransform rect = piece.GetComponent<RectTransform>();

            rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0.5f, 0.5f);

            // Başlama nöqtəsi -50, sonra 110px ara ilə (100 genişlik + 10 boşluq kimi)
            rect.anchoredPosition = new Vector2(-50f + i * 110f, -50f);
        }
    }
    public void SpawnEnemyPieces(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject piece = Instantiate(EnemyPiecePrefab, EnemyPieceParent.transform);
            RectTransform rect = piece.GetComponent<RectTransform>();

            rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0.5f, 0.5f);

            // Başlama nöqtəsi -50, sonra 110px ara ilə (100 genişlik + 10 boşluq kimi)
            rect.anchoredPosition = new Vector2(-50f + i * 110f, -50f);
        }
    }
    public void SpawnPiece(GameObject piece, Transform parentTransform)
    {
        GameObject playerPiece = Instantiate(piece, parentTransform);
       
    }
}

public enum PieceType
{
    Player,
    Enemy,
    Null
}