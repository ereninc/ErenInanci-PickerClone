using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelModel : ScriptableObject
{
    public List<Vector3> RoadDatas;
    public List<PassAreaDataModel> PassAreaDatas;
}
