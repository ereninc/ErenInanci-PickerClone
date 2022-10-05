using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelModel : ScriptableObject
{
    public List<RoadDataModel> RoadDatas;
    public List<PassAreaDataModel> PassAreaDatas;
}
