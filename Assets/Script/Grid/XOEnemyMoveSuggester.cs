using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class XOEnemyMoveSuggester
{
    private const int WinCount = 3;

    public static Cell GetBestMove(List<Cell> board, PieceType enemyPiece, PieceType playerPiece)
    {
        int rowCount = GameDatas.Instance.mainGameDatasSO.BoardSizeY;
        int colCount = GameDatas.Instance.mainGameDatasSO.BoardSizeX;

        // 1. Qalib gəlmək imkanı (ən yaxşı ehtimal)
        var winMove = FindThreatOrWinCell(board, rowCount, colCount, enemyPiece, WinCount - 1);
        if (winMove != null)
            return winMove;

        // 2. Qarşı tərəfin qalib gəlməsinin qarşısını almaq (defensive move)
        var blockMove = FindThreatOrWinCell(board, rowCount, colCount, playerPiece, WinCount - 1);
        if (blockMove != null)
            return blockMove;

        // 3. Oyunda üstünlük yaratmaq (məs: player 2 qoyubsa, onun yanına qoy)
        var strategicMove = FindThreatOrWinCell(board, rowCount, colCount, playerPiece, 1);
        if (strategicMove != null)
            return strategicMove;

        // 4. Ən pis ehtimal – random boş yer
        return board.FirstOrDefault(cell => cell.cellValue == PieceType.Null);
    }

    /// <summary>
    /// Verilmiş növ üçün 2 və ya 1 eyni cell olan ardıcıllığın yanına boş yer tapır.
    /// </summary>
    private static Cell FindThreatOrWinCell(List<Cell> board, int rowCount, int colCount, PieceType pieceType, int requiredInARow)
    {
        foreach (var direction in directions)
        {
            for (int y = 0; y < rowCount; y++)
            {
                for (int x = 0; x < colCount; x++)
                {
                    List<Cell> potentialLine = new List<Cell>();
                    int matchCount = 0;
                    Cell emptyCell = null;

                    for (int i = 0; i < WinCount; i++)
                    {
                        int checkX = x + i * direction.x;
                        int checkY = y + i * direction.y;

                        if (checkX < 0 || checkX >= colCount || checkY < 0 || checkY >= rowCount)
                            break;

                        int index = checkY * colCount + checkX;
                        var cell = board[index];
                        potentialLine.Add(cell);

                        if (cell.cellValue == pieceType)
                            matchCount++;
                        else if (cell.cellValue == PieceType.Null && emptyCell == null)
                            emptyCell = cell;
                    }

                    if (matchCount == requiredInARow && potentialLine.Count == WinCount && emptyCell != null)
                        return emptyCell;
                }
            }
        }

        return null;
    }
    public static Cell GetWorstMove(List<Cell> board, PieceType enemyPiece, PieceType playerPiece)
    {
        int rowCount = GameDatas.Instance.mainGameDatasSO.BoardSizeY;
        int colCount = GameDatas.Instance.mainGameDatasSO.BoardSizeX;

        List<Cell> emptyCells = board.Where(cell => cell.cellValue == PieceType.Null).ToList();
        List<(Cell cell, int score)> scoredCells = new List<(Cell, int)>();

        foreach (var cell in emptyCells)
        {
            int index = board.IndexOf(cell);

            // Temporarily simulate putting enemy's piece
            cell.cellValue = enemyPiece;

            int threatScore = EvaluateThreatPotential(board, rowCount, colCount, enemyPiece);
            int blockScore = EvaluateThreatPotential(board, rowCount, colCount, playerPiece);

            // Reset
            cell.cellValue = PieceType.Null;

            // Pis qərarlar: aşağı skorlu və opponentə imkan verənlər
            int totalScore = threatScore - blockScore;
            scoredCells.Add((cell, totalScore));
        }

        // Ən pis (ən aşağı skorlu və opponentin lehine olan) yeri tap
        return scoredCells
            .OrderBy(c => c.score) // ən aşağı scor
            .ThenBy(c => Random.value) // eyni scor varsa random
            .FirstOrDefault().cell;
    }
    private static int EvaluateThreatPotential(List<Cell> board, int rowCount, int colCount, PieceType target)
    {
        int score = 0;
        var directions = new List<Vector2Int>
    {
        new Vector2Int(1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(1, 1),
        new Vector2Int(-1, 1)
    };

        for (int y = 0; y < rowCount; y++)
        {
            for (int x = 0; x < colCount; x++)
            {
                foreach (var dir in directions)
                {
                    int match = 0;
                    for (int i = 0; i < WinCount; i++)
                    {
                        int nx = x + i * dir.x;
                        int ny = y + i * dir.y;
                        if (nx < 0 || ny < 0 || nx >= colCount || ny >= rowCount)
                            break;

                        var index = ny * colCount + nx;
                        if (board[index].cellValue == target)
                            match++;
                    }

                    // Daha çox ardıcıllıq daha yüksək riskdir
                    if (match >= 2)
                        score += match * match; // kvadrat verir daha kəskin qiymət
                }
            }
        }

        return score;
    }


    // Dörd istiqamət
    private static readonly List<Vector2Int> directions = new List<Vector2Int>
    {
        new Vector2Int(1, 0),  // →
        new Vector2Int(0, 1),  // ↓
        new Vector2Int(1, 1),  // ↘
        new Vector2Int(-1, 1)  // ↙
    };
}
