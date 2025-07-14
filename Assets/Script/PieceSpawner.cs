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
        if(GameManager.Instance.IsGameFinished) return;
        StartSpawnCount = GameDatas.Instance.mainGameDatasSO.SpawnCount;
        SpawnPlayerPieces(StartSpawnCount);
        SpawnEnemyPieces(StartSpawnCount);
    }
    
    public void SpawnPlayerPieces(int count)
    {
        if (GameManager.Instance.IsGameFinished) return;

        PieceType pieceType = PieceType.Player;
        for (int i = 1; i < count; i++)
        {
            SpawnPlayerPiece(i, PlayerPiecePrefab, pieceType);
        }
        if (SaveDataService.Current.UnlockedWeapons.Count > 0)
        {
            SpecialPieceType special = SaveDataService.Current.UnlockedWeapons[Random.Range(0, SaveDataService.Current.UnlockedWeapons.Count)];
            SpawnSpecialPieceEnemy(0, special, pieceType);
            //SpawnSpecialPieceEnemy(0, special, special, pieceType);
            //SpawnSpecialPieceEnemy(0, special, special, pieceType);
            //SpawnSpecialPieceEnemy(3, SpecialPieceType.Flame.ToString(), pieceType);
            //SpawnSpecialPieceEnemy(4, SpecialPieceType.ThunderGun.ToString(), pieceType);
            //SpawnSpecialPieceEnemy(5, SpecialPieceType.Bomb3Turn.ToString(), pieceType);
            //List<string> specialStrings = GameManager.Instance.currenGameUnChangedData.EnemySpecials;
            //SpawnSpecialPieceEnemy(0, specialStrings[Random.Range(0, specialStrings.Count - 1)], pieceType);
        }





    }
    public void SpawnEnemyPieces(int count)
    {
        if (GameManager.Instance.IsGameFinished) return;

        PieceType pieceType = PieceType.Enemy;
        //SpawnSpecialPiece(0, SpecialPieceType.TwoSideGun, "2SX", pieceType);
        //SpawnSpecialPiece(1, SpecialPieceType.Random, "RX", pieceType);
        for (int i = 1; i < count; i++)
        {
            SpawnPlayerPiece(i, EnemyPiecePrefab, pieceType);
        }  
        if(GameManager.Instance.currenGameUnChangedData.Enemies[0].EnemySpecials[0]!= SpecialPieceType.Null)
         SpawnSpecialPieceEnemy(0, GameManager.Instance.currenGameUnChangedData.Enemies[0].EnemySpecials[0], pieceType);


    }

    public void SpawnSpecialPiece(int count, SpecialPieceType specialPieceType,PieceType pieceType)
    {
        SpawnPlayerPiece(count, specialPieceController.specialPieces.Find(x => x.specialPieceType == specialPieceType).piecePrefab, pieceType);
    }
    public void SpawnSpecialPieceEnemy(int count, SpecialPieceType specialPieceType, PieceType pieceType)
    {
        Debug.Log(pieceType + " " + specialPieceType+" "+ pieceType);
       
        SpecialPieceData specialPieceData = specialPieceController.specialPieces.Find(x => x.specialPieceType == specialPieceType);
        SpawnPlayerPiece(count, specialPieceData.piecePrefab, pieceType, specialPieceData);
    }
    public void SpawnPlayerPiece(int i,GameObject gameObject,PieceType pieceType,SpecialPieceData specialPieceData=null)
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
        if(specialPieceData != null)
        {
            piece.GetComponent<SpecialPieceCore>().SetupSpecial(specialPieceData);
        }
    }
    public void SpawnPiece(GameObject piece, Transform parentTransform)
    {
        GameObject playerPiece = Instantiate(piece, parentTransform);
       
    }

}

