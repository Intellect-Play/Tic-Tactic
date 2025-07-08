using System.Collections;
using System.Collections.Generic;
using Assets.Script;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    [SerializeField] private GameObject PlayerPiecePrefab;
    [SerializeField] private GameObject EnemyPiecePrefab;
    [SerializeField] private Transform PlayerPieceParent;
    [SerializeField] private Transform EnemyPieceParent;

    private SpecialPieceController specialPieceController;
    public int StartSpawnCount = 5;

    private void Awake()
    {
        specialPieceController = GetComponent<SpecialPieceController>();
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
        StartSpawnCount = GameDatas.Instance.mainGameDatasSO.SpawnCount;
        SpawnPlayerPieces(StartSpawnCount);
        SpawnEnemyPieces(StartSpawnCount);
    }
    
    public void SpawnPlayerPieces(int count)
    {
        PieceType pieceType = PieceType.Player;
        for (int i = 1; i < count; i++)
        {
            SpawnPlayerPiece(i, PlayerPiecePrefab,"O", pieceType);
        }
        if (SaveDataService.Current.UnlockedWeapons.Count > 0) {
            string special = SaveDataService.Current.UnlockedWeapons[Random.Range(0, SaveDataService.Current.UnlockedWeapons.Count)];
            SpawnSpecialPieceEnemy(0, special, special, pieceType);
        }
        

        //SpawnSpecialPiece(3, SpecialPieceType.Healer, "+", pieceType);

        //SpawnSpecialPiece(4,SpecialPieceType.Plus2,"-2", pieceType);
        //SpawnSpecialPiece(5,SpecialPieceType.Bomb3Turn,"3B", pieceType);    
        //SpawnSpecialPiece(6, SpecialPieceType.Snayper, "S", pieceType);

        //SpawnSpecialPiece(7, SpecialPieceType.ThunderGun, "T", pieceType);
        //SpawnSpecialPiece(8, SpecialPieceType.TwoSideGun, "2S", pieceType);
        //SpawnSpecialPiece(2, SpecialPieceType.Random, "R",pieceType );


    }
    public void SpawnEnemyPieces(int count)
    {
        PieceType pieceType = PieceType.Enemy;
        //SpawnSpecialPiece(0, SpecialPieceType.TwoSideGun, "2SX", pieceType);
        //SpawnSpecialPiece(1, SpecialPieceType.Random, "RX", pieceType);
        for (int i = 1; i < count; i++)
        {
            SpawnPlayerPiece(i, EnemyPiecePrefab, "X", pieceType);
        }
        SpawnSpecialPieceEnemy(0, GameManager.Instance.currenGameUnChangedData.EnemySpecials[0], GameManager.Instance.currenGameUnChangedData.EnemySpecials[Random.Range(0, GameManager.Instance.currenGameUnChangedData.EnemySpecials.Count)], pieceType);


    }

    public void SpawnSpecialPiece(int count, SpecialPieceType specialPieceType,string text,PieceType pieceType)
    {
        SpawnPlayerPiece(count, specialPieceController.specialPieces.Find(x => x.specialPieceType == specialPieceType).piecePrefab, text,pieceType);
    }
    public void SpawnSpecialPieceEnemy(int count, string specialPieceType, string text, PieceType pieceType)
    {
        SpawnPlayerPiece(count, specialPieceController.specialPieces.Find(x => x.specialPieceType.ToString() == specialPieceType).piecePrefab, text, pieceType);
    }
    public void SpawnPlayerPiece(int i,GameObject gameObject,string text,PieceType pieceType)
    {
        GameObject piece;
        if (pieceType == PieceType.Enemy)
        {
            piece = Instantiate(gameObject, EnemyPieceParent.transform);
            GameManager.Instance.aiController.GetAiPiece(piece.GetComponent<PieceBase>());

        }
        else
        {
            piece = Instantiate(gameObject, PlayerPieceParent);
            GameManager.Instance.playerController.playerPieces.Add(piece.GetComponent<PieceBase>());

        }
        RectTransform rect = piece.GetComponent<RectTransform>();
        piece.GetComponent<PieceBase>().playerValue = pieceType;

        rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0.5f, 0.5f);

        // Başlama nöqtəsi -50, sonra 110px ara ilə (100 genişlik + 10 boşluq kimi)
        rect.anchoredPosition = new Vector2(-150f + i * 200f, -50f);
        piece.GetComponent<RectTransform>().localScale = new Vector2(2, 2);
    }
    public void SpawnPiece(GameObject piece, Transform parentTransform)
    {
        GameObject playerPiece = Instantiate(piece, parentTransform);
       
    }

}

