
using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public static Board Instance;
    [SerializeField]public int boardSizeX;
    [SerializeField] public int boardSizeY;
    [SerializeField] public GameObject LinePlayer;
    [SerializeField] public GameObject LineEnemy;
    [SerializeField] public Transform Canvas;

    [SerializeField]private GameObject cellPrefab;
    GridLayoutGroup gridLayoutGroup;
    public List<Cell> Cells = new List<Cell>();
    public Cell[,] CellArray;
    private List<Cell> emptyCells = new List<Cell>();
    public Cell MiddleCell;
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
    private void OnEnable()
    {
        if(SaveDataService.CurrentLevel == 1)
        {
            boardSizeX = 3;
            boardSizeY = 3;
        }
        else
        {
            boardSizeX = GameDatas.Instance.mainGameDatasSO.BoardSizeX;
            boardSizeY = GameDatas.Instance.mainGameDatasSO.BoardSizeY;
        }
            

        CellArray = new Cell[boardSizeX, boardSizeY];
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
        gridLayoutGroup.constraintCount = boardSizeX;
        GameActions.Instance.OnStartGame += Generate;
        GameActions.Instance.OnEndTurn += SetMainColorCells;

    }

    public void GetSTRIKE(Cell middleCell, Vector3 rotation,PieceType playerPiece)
    {
        MiddleCell = middleCell;
        GameObject linePrefab = playerPiece == PieceType.Player ? LinePlayer : LineEnemy;

        linePrefab.SetActive(true); // Aktiv etmək
        linePrefab.transform.SetParent(middleCell.transform); // Canvas-a daxil etmək üçün
        linePrefab.transform.localPosition = Vector3.zero; // Mərkəzdə yerləşdirmək üçün
        // RectTransform-a daxil olub rotation-u qur
        RectTransform rectTransform = linePrefab.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            //rectTransform.localRotation = Quaternion.Euler(new Vector3(-rotation.x,-rotation.y,0));
            rectTransform.localRotation = Quaternion.Euler(new Vector3(0, 0, rotation.z+90));

            rectTransform.anchoredPosition3D = Vector3.zero; // mərkəzdə yerləşdirmək üçün
        }
    }

    private void OnDisable()
    {
        GameActions.Instance.OnStartGame -= Generate;
        GameActions.Instance.OnEndTurn -= SetMainColorCells;

    }

    public void SetMainColorCells()
    {
        foreach (var cell in Cells)
        {
            if (cell != null)
            {
                cell.SetImage(true);
            }
        }
    }
    public void SetCellColor(int x, int y, bool colorCell)
    {
        Cell cell = GetCell(x, y);
        if (cell != null)
        {
            cell.SetImage(colorCell);
        }
    }

    public void Generate()
    {
        for (int i = 0; i < boardSizeY; i++)
        {
            for (int j = 0; j < boardSizeX; j++)
            {
                GameObject cellObject = Instantiate(cellPrefab, transform);
                Cell cell = cellObject.GetComponent<Cell>();             
                cell.Init(j, i);
                Cells.Add(cell);
                CellArray[j,i] = cell;
            }
        }
    }
    public void DestroyPiece(int x, int y,PieceType pieceType, SpecialPieceData specialPieceData)
    {
       
        Cell cell = GetCell(x, y);
        if (cell == null)
        {
            return;
        }
       
        ParticleSystem particle = Instantiate(specialPieceData.SpecialParticleEffect, Vector3.zero, Quaternion.identity, GameManager.Instance.board.CellArray[x, y].transform);
        particle.transform.localPosition = new Vector3(0, 0, particle.transform.localPosition.z);
        particle.Play();
        if (cell != null && cell._PlayerPiece != null&&cell._PlayerPiece.playerValue!=pieceType)
        {
            cell._PlayerPiece.DestroyPiece();
            if (pieceType == PieceType.Player)
            {
                Coin.Instance.GetCoin(GameDatas.Instance.mainGameDatasSO.CoinGetFromEnemy);
                Coin.Instance.PlayCoinEffect(cell.GetComponent<RectTransform>());
            }
        }
    }

    public static void PlayCoinEffect(GameObject coinPrefab, Transform startParent, Transform targetUI, float duration = 1.0f)
    {
        // 1. Coin Image yarat (Particle kimi)
        GameObject coin = Instantiate(coinPrefab, startParent.position, Quaternion.identity, startParent.parent);
        RectTransform coinRect = coin.GetComponent<RectTransform>();

        // Başlanğıc mövqeyini UI koordinatına çevir
        Vector3 startPos = startParent.position;
        coinRect.position = startPos;

        // Kiçik scale ilə başlayır
        coinRect.localScale = Vector3.zero;

        // 2. Animasiya (DOTween)
        Sequence seq = DOTween.Sequence();

        // Scale In (sürətli böyümə)
        seq.Append(coinRect.DOScale(1f, 0.3f).SetEase(Ease.OutBack));

        // Move to Target UI (coin icon)
        seq.Append(coinRect.DOMove(targetUI.position, duration).SetEase(Ease.InQuad));

        // Fade Out (Image alpha ilə)
        Image img = coin.GetComponent<Image>();
        if (img != null)
        {
            seq.Join(img.DOFade(0, duration * 0.5f).SetDelay(duration * 0.5f));
        }

        // 3. Bitəndə obyekt silinsin
        seq.OnComplete(() => Destroy(coin));
    }
    public void DestroyAllPiece(PieceType pieceType)
    {
        foreach (var cell in Cells)
        {
            if (cell != null && cell._PlayerPiece != null && cell._PlayerPiece.playerValue != pieceType)
            {
                //Debug.Log($"Destroying piece at coordinates ({x}, {y})");
                cell._PlayerPiece.DestroyPiece();
            }
        }
      
    }
    Cell GetCell(int x, int y)
    {
        //Debug.Log($"Getting cell at coordinates ({x}, {y})cdsc"+boardSize);
        if (x < 0 || x >= boardSizeX || y < 0 || y >= boardSizeY)
        {
            return null;
        }
        //Debug.Log($"2  Getting cell at coordinates ({x}, {y})cdsc" + boardSize);

        return CellArray[x,y];
    }
    public Cell EmptyCell(int winCount)
    {
        // Limitləri yoxla (təhlükəsizlik üçün)
        winCount = winCount - GameManager.Instance.lastWinCase;
        if(winCount < 0) winCount = 0;
        int randomValue = UnityEngine.Random.Range(0, 100);

        if (randomValue < winCount)
        {
            Debug.Log("WinEnemy" + randomValue + " " + winCount);
            // Ağıllı qərar (yaxşı ehtimal daxilindədirsə)
            return XOEnemyMoveSuggester.GetBestMove(Cells, PieceType.Enemy, PieceType.Player);
        }
        else
        {
            Debug.Log("LoseEnemy" + randomValue + " " + winCount);

            // Pis qərar (qalan faiz daxilindədirsə)
            return XOEnemyMoveSuggester.GetWorstMove(Cells, PieceType.Enemy, PieceType.Player);
        }
    }
    public void CleanBoard() {

        foreach (var cell in Cells)
        {
            if(cell._PlayerPiece!=null)
            cell._PlayerPiece.DestroyPiece();
        }
    }
    public bool IsFull()
    {
        foreach (var cell in Cells)
            if (!cell.HasValue) return false;

        return true;
    }
}
