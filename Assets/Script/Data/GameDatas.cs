using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDatas : MonoBehaviour
{
    public static GameDatas Instance { get; private set; }
    [SerializeField] public MainGameDatasSO mainGameDatasSO;

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
}
