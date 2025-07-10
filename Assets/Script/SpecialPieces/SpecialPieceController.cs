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
    public string pieceName; // Parçanın adı
    public SpecialPieceType specialPieceType; // Xüsusi parça tipi
    public GameObject piecePrefab;
    public Sprite OSprite; // Xüsusi parça üçün sprite
    public Sprite XSprite; // Xüsusi parça üçün sprite

    public ParticleSystem SpecialParticleEffect; // O üçün xüsusi effekt
    public GameObject SpecialAnimeEffect; // O üçün xüsusi effekt

    [Header("O Animations")]
    public AnimationClip EnemyAnimeIdle;
    public AnimationClip EnemyAnimeAttack;

    [Header("X Animations")]
    public AnimationClip PlayerAnimeIdle;
    public AnimationClip PlayerAnimeAttack;

    [Header("Bomb")]
    [Header("Bomb O")]
    public List<AnimationClip> EnemyBombs;
    [Header("Bomb X")]
    public List<AnimationClip> PlayerBombs;

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