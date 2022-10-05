using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoadDataModel
{
    public int Id;
    public Vector3 Position;
    public List<PickableDataModel> Pickables;
}