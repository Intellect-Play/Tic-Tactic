using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    [SerializeField] private GameObject PlayerPiecePrefab;
    [SerializeField] private GameObject EnemyPiecePrefab;
    [SerializeField] public List<PlayerSpawnButtons> PlayerPieceParent;
    [SerializeField] public List<PlayerSpawnButtons> EnemyPieceParent;

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
        if (GameManager.Instance.currenGameUnChangedData.PlayerSpecialUnlockTutorialBool)
        {
            for (; i < count - 1; i++)
            {
                SpecialPieceType special = SaveDataService.UnlockedWeapons[SaveDataService.UnlockedWeapons.Count-1];
                SpawnSpecialPieceEnemy(PlayerPieceParent[i], special, pieceType);
            }
        }
        else if (SaveDataService.UnlockedWeapons.Count > 0)
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

        for (int i = 0; i < PlayerPieceParent.Count-1; i++)
        {
            if (PlayerPieceParent[i].pieceBase == null)
            {
                if(j==0&&SaveDataService.Coins>=GameDatas.Instance.mainGameDatasSO.Buy1Piece)
                PlayerPieceParent[i].BuyPiece( BuyPlayerPieceOneA, 200,1);
                else if(SaveDataService.Coins >= GameDatas.Instance.mainGameDatasSO.Buy3Piece)
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
        if (GameManager.Instance.IsGameFinished) return;
        if (SaveDataService.UnlockedWeapons.Count > 0)
        {
            Coin.Instance.GetCoin(-GameDatas.Instance.mainGameDatasSO.Buy1Piece);
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
            Coin.Instance.GetCoin(-GameDatas.Instance.mainGameDatasSO.Buy3Piece);
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
        int EnemySpecialCount = GameManager.Instance.currenGameUnChangedData.Enemies[0].EnemySpecials.Count;
        if (EnemySpecialCount > 0 &&(EnemySpecialCount==1&& AIController.Instance.gameUnChangedDatas[0].EnemySpecials[0]!=SpecialPieceType.Null))
        {
            SpawnSpecialPieceEnemy(EnemyPieceParent[i], AIController.Instance.gameUnChangedDatas[0].EnemySpecials[UnityEngine.Random.Range(0, EnemySpecialCount)], pieceType);
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
        if (TutorialManager.Instance.IsTutorialActive)
        {

            StartCoroutine(tutorialTime());
        }
    }
    IEnumerator tutorialTime()
    {
        yield return new WaitForSeconds(2f);
        TutorialManager.Instance.TutorialStart();
    }
    public void SpawnPiece(GameObject piece, Transform parentTransform)
    {
        GameObject playerPiece = Instantiate(piece, parentTransform);
       
    }

}

