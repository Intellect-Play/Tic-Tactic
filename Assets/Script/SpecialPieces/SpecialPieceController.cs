using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialPieceController : MonoBehaviour
{
    public List<SpecialPieceData> specialPieces; // Xüsusi parçaların siyahısı
}

[System.Serializable]
public class SpecialPieceData
{
    public string pieceName;
    public int pieceId;
    public GameObject piecePrefab;
    public int spawnCount;
    public float spawnInterval;

    // Əlavə xüsusiyyətlər əlavə etmək istəsəniz, buraya əlavə edin
}

public enum SpecialPieceType
{

    ThunderGun,
    // Burada digər xüsusi parça tiplərini əlavə edə bilərsiniz
}