using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

//[System.Serializable]
//public class SaveData
//{
//    public int Coins = 0;
//    public int CurrentLevel = 1;

//    // Əgər sonra açmaq istəsən:
//     public List<SpecialPieceType> UnlockedWeapons = new List<SpecialPieceType>();
//    // public List<int> WeaponLevels = new List<int>();
//}

public static class SaveDataService
{
    public static int Coins
    {
        get => PlayerPrefs.GetInt("Coins", 0);
        set => PlayerPrefs.SetInt("Coins", value);
    }

    public static int CurrentLevel
    {
        get => PlayerPrefs.GetInt("CurrentLevel", 1);
        set => PlayerPrefs.SetInt("CurrentLevel", value);
    }

    public static List<SpecialPieceType> UnlockedWeapons
    {
        get
        {
            string raw = PlayerPrefs.GetString("UnlockedWeapons", "");
            List<SpecialPieceType> result = new List<SpecialPieceType>();
            if (!string.IsNullOrEmpty(raw))
            {
                foreach (var s in raw.Split(','))
                {
                    if (Enum.TryParse(s, out SpecialPieceType type))
                        result.Add(type);
                }
            }
            return result;
        }
        set
        {
            var raw = string.Join(",", value.Select(x => x.ToString()));
            PlayerPrefs.SetString("UnlockedWeapons", raw);
        }
    }

    public static void Save()
    {
        PlayerPrefs.Save();
    }

    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }
}
