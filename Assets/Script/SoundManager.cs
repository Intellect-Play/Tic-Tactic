using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [System.Serializable]
    public class SoundData
    {
        public string name;
        public SoundType type;
        public AudioClip clip;
    }

    [Header("Audio")]
    public AudioSource sfxSource;
    public List<SoundData> sounds;

    private Dictionary<SoundType, AudioClip> soundDict;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            BuildDictionary();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void BuildDictionary()
    {
        soundDict = new Dictionary<SoundType, AudioClip>();
        foreach (var s in sounds)
        {
            if (!soundDict.ContainsKey(s.type))
                soundDict.Add(s.type, s.clip);
        }
    }

    public void PlaySound(SoundType type)
    {
        Debug.Log("Playing sound: " + type.ToString());
        if (soundDict.TryGetValue(type, out AudioClip clip))
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Sound not found: " + type);
        }
    }
}

public enum SoundType
{
    PutPiece,
    Flame,
    Thunder,
    Shoot,
    Bomb,
    Heal,
    Win,
    Swoop,
    Reward,
    Line,
    Click
    // Əlavə səs tipləri...
}
