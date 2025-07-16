using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    [SerializeField] private GameObject PlayerPiecePrefab;
    [SerializeField] private GameObject EnemyPiecePrefab;
    [SerializeField] private List<PlayerSpawnButtons> PlayerPieceParent;
    [SerializeField] private List<PlayerSpawnButtons> EnemyPieceParent;

    private SpecialPieceController specialPieceController;
    public int StartSpawnCount = 5;
    public Action<PlayerSpawnButtons> BuyPlayerPieceOneA;
    public Action<PlayerSpawnButtons> BuyPlayerPieceAllA;

    private void Awake()
    {
        specialPieceController = GetComponent<SpecialPieceController>();
    }
    private void Start()
    {
        BuyPlayerPieceOneA += BuyPlayerPieceOne;
        BuyPlayerPieceAllA += BuyPlayerPieceThree;
        GameActions.Instance.OnStartGame += StartSpawn;
    }
  
    private void OnDisable()
    {
        BuyPlayerPieceOneA -= BuyPlayerPieceOne;
        BuyPlayerPieceAllA -= BuyPlayerPieceThree;
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
        ResetBuys();
        PieceType pieceType = PieceType.Player;
        int i = 0;
        if (SaveDataService.UnlockedWeapons.Count > 0)
        {
            SpecialPieceType special = SaveDataService.UnlockedWeapons[UnityEngine.Random.Range(0, SaveDataService.UnlockedWeapons.Count)];
            SpawnSpecialPieceEnemy(PlayerPieceParent[i], special, pieceType);
            i = 1;
        }
        for (; i < count - 1; i++)
        {
            SpawnPlayerPiece(PlayerPieceParent[i], PlayerPiecePrefab, pieceType);
        }
    }
    public void CheckBuy() {
        int j = 0;
        Debug.Log("Check Buy F"  + PlayerPieceParent.Count);

        for (int i = 0; i < PlayerPieceParent.Count-1; i++)
        {
            if (PlayerPieceParent[i].pieceBase == null)
            {
                Debug.Log("Check Buy "+i+" "+j);
                if(j==0)
                PlayerPieceParent[i].BuyPiece( BuyPlayerPieceOneA, 200,1);
                else
                    PlayerPieceParent[i].BuyPiece( BuyPlayerPieceAllA, 300, 3);

                j++;
            }
        }
    }
    public void ResetBuys()
    {
        for (int i = 0; i < PlayerPieceParent.Count; i++)
        {
            PlayerPieceParent[i].ResetButton();
        }
    }
    public void BuyPlayerPieceOne(PlayerSpawnButtons playerSpawnButtons)
    {
        Debug.Log("Buy One");
        if (GameManager.Instance.IsGameFinished) return;
        if (SaveDataService.UnlockedWeapons.Count > 0)
        {
            PieceType pieceType = PieceType.Player;

            SpecialPieceType special = SaveDataService.UnlockedWeapons[UnityEngine.Random.Range(0, SaveDataService.UnlockedWeapons.Count)];
            SpawnSpecialPieceEnemy(playerSpawnButtons, special, pieceType);

        }
    }
    public void BuyPlayerPieceThree(PlayerSpawnButtons playerSpawnButtons)
    {
        if (GameManager.Instance.IsGameFinished) return;
        if (SaveDataService.UnlockedWeapons.Count > 0)
        {
            PieceType pieceType = PieceType.Player;
            PlayerController.Instance.RemoveAllPlayerPieces();
            foreach (var i in PlayerPieceParent)
            {

                SpecialPieceType special = SaveDataService.UnlockedWeapons[UnityEngine.Random.Range(0, SaveDataService.UnlockedWeapons.Count)];
                SpawnSpecialPieceEnemy(i, special, pieceType);
            }
        }
    }
    public void SpawnEnemyPieces(int count)
    {
        if (GameManager.Instance.IsGameFinished) return;

        PieceType pieceType = PieceType.Enemy;
        //SpawnSpecialPiece(0, SpecialPieceType.TwoSideGun, "2SX", pieceType);
        //SpawnSpecialPiece(1, SpecialPieceType.Random, "RX", pieceType);
        //SpawnPlayerPiece(0, EnemyPiecePrefab, pieceType);
        int i = 0;
       
        if (GameManager.Instance.currenGameUnChangedData.Enemies[0].EnemySpecials[0] != SpecialPieceType.Null)
        {
            SpawnSpecialPieceEnemy(EnemyPieceParent[i], GameManager.Instance.currenGameUnChangedData.Enemies[0].EnemySpecials[0], pieceType);
            i = 1;
        }
        for (; i < count - 1; ++i)
        {
            SpawnPlayerPiece(EnemyPieceParent[i], EnemyPiecePrefab, pieceType);
        }


    }

    public void SpawnSpecialPiece(PlayerSpawnButtons count, SpecialPieceType specialPieceType,PieceType pieceType)
    {
        SpawnPlayerPiece(count, specialPieceController.specialPieces.Find(x => x.specialPieceType == specialPieceType).piecePrefab, pieceType);
    }
    public void SpawnSpecialPieceEnemy(PlayerSpawnButtons count, SpecialPieceType specialPieceType, PieceType pieceType)
    {
        Debug.Log(pieceType + " " + specialPieceType+" "+ pieceType);
       
        SpecialPieceData specialPieceData = specialPieceController.specialPieces.Find(x => x.specialPieceType == specialPieceType);
        SpawnPlayerPiece(count, specialPieceData.piecePrefab, pieceType, specialPieceData);
    }
    public void SpawnPlayerPiece(PlayerSpawnButtons i,GameObject gameObject,PieceType pieceType,SpecialPieceData specialPieceData=null)
    {
        GameObject piece;
        if (pieceType == PieceType.Enemy)
        {
            piece = Instantiate(gameObject, i.transform);
            GameManager.Instance.aiController.GetAiPiece(piece.GetComponent<PieceBase>());

        }
        else
        {

            piece = Instantiate(gameObject, i.transform);
            GameManager.Instance.playerController.playerPieces.Add(piece.GetComponent<PieceBase>());

        }
        i.GetPiece(piece.GetComponent<PieceBase>());
        RectTransform rect = piece.GetComponent<RectTransform>();
        piece.GetComponent<PieceBase>().playerValue = pieceType;

        rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0.5f, 0.5f);

        // Başlama nöqtəsi -50, sonra 110px ara ilə (100 genişlik + 10 boşluq kimi)
        rect.anchoredPosition = Vector2.zero;
        //piece.GetComponent<RectTransform>().localScale = new Vector2(2, 2);
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

