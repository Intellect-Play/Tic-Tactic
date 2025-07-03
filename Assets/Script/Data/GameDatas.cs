using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class GameDatas : MonoBehaviour
{
    public static GameDatas Instance { get; private set; }
    [SerializeField] public MainGameDatasSO mainGameDatasSO;

    public GameUnChangedDatas Data;


    public string SaveFilePath;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        SaveFilePath = Path.Combine(Application.persistentDataPath, "/GameData/GameUnChangedDatas");
        LoadData();
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
            Data = JsonUtility.FromJson<GameUnChangedDatas>(json);
        }
        else
        {
            Debug.Log(Equals(null) ? "GameDataService is null" : "GameDataService is not null");
            // First time: copy from Resources to persistent path
            TextAsset jsonAsset = Resources.Load<TextAsset>("GameData/GameUnChangedDatas");
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
                Data = JsonUtility.FromJson<GameUnChangedDatas>(jsonAsset.text);

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
