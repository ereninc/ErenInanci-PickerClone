using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelModel : ScriptableObject
{
    public string Name;
    public List<WorldItemDataModel> RoadDatas;
    public List<int> PassAreaCounts;
    public List<LineDataModel> LineDatas;
}