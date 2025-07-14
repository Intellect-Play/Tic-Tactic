using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public int Coins = 0;
    public int CurrentLevel = 1;

    // Əgər sonra açmaq istəsən:
     public List<SpecialPieceType> UnlockedWeapons = new List<SpecialPieceType>();
    // public List<int> WeaponLevels = new List<int>();
}

public static class SaveDataService
{
    private const string SaveKey = "SaveDataJson";
    public static SaveData Current { get; private set; }
    
    static SaveDataService()
    {
        Load(); // İlk dəfə yüklənir
    }

    public static void Save()
    {
        string json = JsonUtility.ToJson(Current, true);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
        Debug.Log("Game saved: " + json);
    }

    public static void Load()
    {
        DeleteSave();
        if (PlayerPrefs.HasKey(SaveKey))
        {
            string json = PlayerPrefs.GetString(SaveKey);
            Current = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("Game loaded: " + json);
            if(Current.UnlockedWeapons.Count>0) Debug.Log("Game loaded: " + json + Current.UnlockedWeapons[0]);

        }
        else
        {
            Current = new SaveData(); // default dəyərlər
            Save(); // İlk dəfə qeyd et
            Debug.Log("New save created with defaults.");
        }
    }

    public static void DeleteSave()
    {
        PlayerPrefs.DeleteKey(SaveKey);
        Current = new SaveData();
        Debug.Log("Save deleted.");
    }
}
