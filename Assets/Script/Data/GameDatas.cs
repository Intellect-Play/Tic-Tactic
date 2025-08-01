using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using UnityEditor;

public class GameDatas : MonoBehaviour
{
    public static GameDatas Instance { get; private set; }
    [SerializeField] public MainGameDatasSO mainGameDatasSO;

    public GameUnChangedDatasSO2 Data2;
    public GameUnChangedDatasSO Data;

    public List<GameUnChangedData> gameUnChangedDatas;

    public string SaveFilePath;
    public List<SpecialPieceType> Specials;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        //DeleteData();
        Instance = this;
        if (!Data.LevelUp)
        {
            SaveDataService.DeleteAll();
            SaveDataService.CurrentLevel = Data.Level;

        }

        if (Data.gameUnChangedDatas[SaveDataService.CurrentLevel-1].PlayerSpecialUnlock != SpecialPieceType.Null)
        {
            mainGameDatasSO.BoardSizeX = 4;
            mainGameDatasSO.BoardSizeY = 3;

        }
        else
        {
            mainGameDatasSO.BoardSizeX = 3;
            mainGameDatasSO.BoardSizeY = 3;

        }
        if (SaveDataService.CurrentLevel == 1)
        {
            mainGameDatasSO.BoardSizeX = 3;
            mainGameDatasSO.BoardSizeY = 3;
        }
        DeepCopy();
        SaveFilePath = Path.Combine(Application.persistentDataPath, "/GameData/GameUnChangedDatas8");
        //LoadData();
        //ConvertData2ToData();
    }
    public void ConvertData2ToData()
    {
        Data = ScriptableObject.CreateInstance<GameUnChangedDatasSO>();
        Data.gameUnChangedDatas = new List<GameUnChangedData>();

        foreach (var data in Data.gameUnChangedDatas)
        {
            GameUnChangedData converted = new GameUnChangedData
            {
                PlayerHP = data.PlayerHP,
                PlayerSpecialUnlock = data.PlayerSpecialUnlock,
                Enemies = new List<EnemiesUnChangedData>()
            };

            foreach (var enemy in data.Enemies)
            {
                EnemiesUnChangedData enemyConverted = new EnemiesUnChangedData
                {
                    EnemyHP = enemy.EnemyHP,
                    EnemySpecials = new List<SpecialPieceType>(enemy.EnemySpecials)
                };

                converted.Enemies.Add(enemyConverted);
            }

            Data.gameUnChangedDatas.Add(converted);
        }

        Debug.Log("Data has been converted to ScriptableObject-compatible format.");
    }
    public void DeepCopy()
    {
       
        gameUnChangedDatas = new List<GameUnChangedData>();
        int levelCount = 1;
        foreach (var data in Data.gameUnChangedDatas)
        {
            GameUnChangedData copy = new GameUnChangedData
            {
                PlayerHP = data.PlayerHP,
                PlayerSpecialUnlock = data.PlayerSpecialUnlock,
                Enemies = new List<EnemiesUnChangedData>()
            };
            if (levelCount < SaveDataService.CurrentLevel && copy.PlayerSpecialUnlock != SpecialPieceType.Null)
            {
                var list = SaveDataService.UnlockedWeapons;
                list.Add(copy.PlayerSpecialUnlock);
                SaveDataService.UnlockedWeapons = list;
               
                SaveDataService.Save();
            }
                foreach (var enemy in data.Enemies)
            {
                EnemiesUnChangedData enemyCopy = new EnemiesUnChangedData
                {
                    EnemyHP = enemy.EnemyHP,
                    EnemySpecials = new List<SpecialPieceType>(enemy.EnemySpecials),
                    WinEnemy = enemy.WinEnemy // Uncomment if you want to copy WinEnemy as well
                };
                copy.Enemies.Add(enemyCopy);
            }

            gameUnChangedDatas.Add(copy);
            levelCount++;
        }
        Specials = SaveDataService.UnlockedWeapons;
    }
    private void OnDisable()
    {
        mainGameDatasSO = null;
    }
    private void LoadData()
    {
        if (File.Exists(SaveFilePath))
        {
            Debug.Log("GameDataService: Load from persistent data path: " + SaveFilePath);
            // Load from persistent data path
            string json = File.ReadAllText(SaveFilePath);
            //if (File.Exists(SaveFilePath))
            //{
            //    File.Delete(SaveFilePath);
            //    Debug.Log("Fayl silindi: " + SaveFilePath);
            //}
            //else
            //{
            //    Debug.Log("Fayl tapılmadı: " + SaveFilePath);
            //}
            Data2 = JsonUtility.FromJson<GameUnChangedDatasSO2>(json);
        }
        else
        {
            Debug.Log(Equals(null) ? "GameDataService is null" : "GameDataService is not null");
            // First time: copy from Resources to persistent path
            TextAsset jsonAsset = Resources.Load<TextAsset>("GameData/GameUnChangedDatas8");
            if (File.Exists(SaveFilePath))
            {
                File.Delete(SaveFilePath);
                Debug.Log("Fayl silindi: " + SaveFilePath);
            }
            else
            {
                Debug.Log("Fayl tapılmadı: " + SaveFilePath);
            }
            if (jsonAsset != null)
            {
                Data2 = JsonUtility.FromJson<GameUnChangedDatasSO2>(jsonAsset.text);

                // Save initial copy for editing at runtime
                string folder = Path.GetDirectoryName(SaveFilePath);
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                File.WriteAllText(SaveFilePath, jsonAsset.text);

            }
            else
            {
                Debug.LogError("ZombieGameData.json not found in Resources/GameData/");
            }
        }
    }


    public void DeleteData()
    {
        try
        {
            // Yaddaşdakı Data obyektini təmizlə
            Data = null;

            // Fayl mövcuddursa sil
            if (File.Exists(SaveFilePath))
            {
                File.Delete(SaveFilePath);
                Debug.Log("Game data file deleted: " + SaveFilePath);
            }
            else
            {
                Debug.Log("No file to delete at path: " + SaveFilePath);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to delete game data: " + ex.Message);
        }
    }


    public void SaveData()
    {
        try
        {
            string json = JsonUtility.ToJson(Data, true);
            string folder = Path.GetDirectoryName(SaveFilePath);
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            File.WriteAllText(SaveFilePath, json);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to save game data: " + ex.Message);
        }
    }

 
}
[Serializable]

public class GameUnChangedDatasSO2 
{
    public List<GameUnChangedData2> gameUnChangedDatas;
}

[Serializable]
public class GameUnChangedData2
{
    public int PlayerHP;
    public List<EnemiesUnChangedData2> Enemies = new List<EnemiesUnChangedData2>(1);
    public SpecialPieceType PlayerSpecialUnlock;
}
[Serializable]
public class EnemiesUnChangedData2
{
    public int EnemyHP;
    public List<SpecialPieceType> EnemySpecials = new List<SpecialPieceType>(1);
}

/*

TwoSideGun - 1, 
ThunderGun - 4,
Flame - 8,
Snayper - 12,
Plus2 - 16,
Healer - 20,
Bomb3Turn - 25,
Random - 30,
Immortal - 35

 */