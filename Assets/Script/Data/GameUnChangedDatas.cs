using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "GameUnChangedDatas", menuName = "GameData/GameUnChangedDatas")]
public class GameUnChangedDatasSO : ScriptableObject
{
    public int Level;

    public List<GameUnChangedData> gameUnChangedDatas;
}

[Serializable]
public class GameUnChangedData
{
    public int PlayerHP;
    public List<EnemiesUnChangedData> Enemies = new List<EnemiesUnChangedData>(1);
    public SpecialPieceType PlayerSpecialUnlock;
}
[Serializable]
public class EnemiesUnChangedData
{
    public int EnemyHP;
    public List<SpecialPieceType> EnemySpecials=new List<SpecialPieceType>(1);
    [Range(1, 100)] public int WinEnemy;

}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(GameUnChangedData))]
public class GameUnChangedDataDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Bütün alt sahələrin hündürlüyünü alır
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        string indexString = property.propertyPath;
        string indexLabel = label.text;

        if (indexString.Contains("[") && indexString.Contains("]"))
        {
            int indexStart = indexString.IndexOf("[") + 1;
            int indexEnd = indexString.IndexOf("]");
            if (indexStart < indexEnd)
            {
                string index = indexString.Substring(indexStart, indexEnd - indexStart);
                indexLabel = "Level " + (int.Parse(index) + 1);
            }
        }

        label = new GUIContent(indexLabel);

        position.height = EditorGUI.GetPropertyHeight(property, label, true);
        EditorGUI.PropertyField(position, property, label, true);

        EditorGUI.EndProperty();
    }
}

[CustomPropertyDrawer(typeof(EnemiesUnChangedData))]
public class EnemiesUnChangedDataDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Enemy indexini çıxart
        string path = property.propertyPath;
        int start = path.LastIndexOf("[") + 1;
        int end = path.LastIndexOf("]");
        string index = path.Substring(start, end - start);

        label = new GUIContent("Enemy " + (int.Parse(index) + 1));

        position.height = EditorGUI.GetPropertyHeight(property, label, true);
        EditorGUI.PropertyField(position, property, label, true);

        EditorGUI.EndProperty();
    }
}
#endif