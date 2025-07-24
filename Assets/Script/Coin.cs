using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public static Coin Instance;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void GetCoin(int coin)
    {
        SaveDataService.Coins += coin;
        if(SaveDataService.Coins < 0)
        {
            SaveDataService.Coins = 0;
        }
        UIManager.Instance.UpdateCoinText(SaveDataService.Coins);
        
    }
}
