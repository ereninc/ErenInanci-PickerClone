using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadModel : ObjectModel
{
    public RoadModel NextRoad;

    public void OnSpawn(Vector3 pos)
    {
        transform.position = pos;
        this.SetActiveGameObject(true);
    }
}