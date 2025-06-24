using System.Collections;
using System.Collections.Generic;
using Assets.Script;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    [SerializeField] private GameObject PlayerPiecePrefab;
    [SerializeField] private GameObject EnemyPiecePrefab;
    [SerializeField] private GameObject SpecialPiecePrefab; // Əlavə olaraq xüsusi parçalar üçün prefab
    [SerializeField] private Transform PlayerPieceParent;
    [SerializeField] private Transform EnemyPieceParent;
    [SerializeField] private ThunderGun ThunderScript;

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
        StartSpawnCount = GameDatas.Instance.mainGameDatasSO.SpawnCount;
        SpawnPlayerPieces(StartSpawnCount);
        SpawnEnemyPieces(StartSpawnCount);
    }
    
    public void SpawnPlayerPieces(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnPlayerPiece(i+1, PlayerPiecePrefab,"O");
        }
        SpawnPlayerPiece(0, SpecialPiecePrefab,"2");
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
            GameManager.Instance.aiController.GetAiPiece(piece.GetComponent<PieceBase>());
        }
    }
    
    public void SpawnPlayerPiece(int i,GameObject gameObject,string text)
    {
        GameObject piece = Instantiate(gameObject, PlayerPieceParent);
        RectTransform rect = piece.GetComponent<RectTransform>();
        piece.AddComponent<ThunderGun>();
        piece.GetComponent<PieceBase>().playerValue = PieceType.Player;

        rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0.5f, 0.5f);

        // Başlama nöqtəsi -50, sonra 110px ara ilə (100 genişlik + 10 boşluq kimi)
        rect.anchoredPosition = new Vector2(-50f + i * 110f, -50f);
        GameManager.Instance.playerController.playerPieces.Add(piece.GetComponent<PieceBase>());
        //piece.GetComponent<PieceBase>().valueText.text = text; // Oyuncu parçalarının dəyərini göstərmək üçün
    }
    public void SpawnPiece(GameObject piece, Transform parentTransform)
    {
        GameObject playerPiece = Instantiate(piece, parentTransform);
       
    }

}

