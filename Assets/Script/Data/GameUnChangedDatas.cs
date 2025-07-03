using System;
using System.Collections.Generic;

[Serializable]
public class GameUnChangedDatas
{
    public List<GameUnChangedData> gameUnChangedDatas;
}

[Serializable]
public class GameUnChangedData
{
    public int CurrentLevel;
    public int PlayerHP;
    public int EnemyHP;
    public List<string> EnemySpecials;
    public string PlayerSpecialUnlock;
}
