using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDatas : MonoBehaviour
{
    public static CharacterDatas Instance;

    public List<CharacterData> AnimationDataList;

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
    public CharacterBase SetupSpecial(Characters _character)
    {
        CharacterData characterData = AnimationDataList.Find(data => data.CharacterName == _character);
        CharacterBase characterBase = Instantiate(characterData.CharacterPrefab).GetComponent<CharacterBase>();
        characterBase.SetupSpecial(characterData);        

        return characterBase;
    }
}

[Serializable]
public class CharacterData
{
    public string CharacterNameString;
    public Characters CharacterName;
    public GameObject CharacterPrefab;
    public Sprite CharacterSprite;

    public AnimationClip AnimationIdle;
    public AnimationClip AnimationAttack;
    public AnimationClip AnimationHit;

}

public enum Characters
{
    PlayerCat,
    EnemyOrk,
    EnemyYork,
    Vodo,
    Pyramid
}