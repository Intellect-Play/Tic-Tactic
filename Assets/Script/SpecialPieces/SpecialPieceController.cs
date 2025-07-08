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
    public SpecialPieceType specialPieceType; // Xüsusi parça tipi
    public GameObject piecePrefab;
    public Object pieceSprite; // Xüsusi parça üçün sprite
    public Animation EnemyAnime;
    public Animation PlayerAnime;
}

public enum SpecialPieceType
{
    TwoSideGun,
    ThunderGun,
    Bomb3Turn,
    EatEnemies,
    Snayper,
    Plus2,
    ImMortal,
    Random,
    Healer,
    Flame
}