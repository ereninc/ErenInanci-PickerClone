using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadModel : ObjectModel
{
    public void OnSpawn(Vector3 pos)
    {
        transform.position = pos;
        this.SetActiveGameObject(true);
    }
}