using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "MainGameDatasSO", menuName = "ScriptableObjects/MainGameDatasSO", order = 1)]
public class MainGameDatasSO : ScriptableObject
{
   
    //public int BoardSize;
    public int BoardSizeX;
    public int BoardSizeY;

    public int SpawnCount;
    public float PieceSize=1.8f;

    public float MoveDuration;
    public int CoinGet;

    public float EnemyPieceSize=0.5f;
    public float PlayerPieceSize=1.2f;
}
