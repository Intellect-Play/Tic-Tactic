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
    public float MoveDuration;
    public int PlayerHealth;
    public int EnemyHealth;
}
