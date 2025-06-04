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
    private void OnEnable()
    {
        GameActions.Instance.OnStartGame += StartSpawn;
    }
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
        //SpawnPlayerPieces(StartSpawnCount);
        //SpawnEnemyPieces(StartSpawnCount);
    }
    
    public void SpawnPlayerPieces(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnPiece(PlayerPiecePrefab, PlayerPieceParent);
        }
    }
    public void SpawnEnemyPieces(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnPiece(EnemyPiecePrefab, EnemyPieceParent.transform);
        }
    }
    public void SpawnPiece(GameObject piece, Transform parentTransform)
    {
        GameObject playerPiece = Instantiate(piece, parentTransform);      
    }
}
