using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishModel : ObjectModel
{
    public void OnSpawn(Vector3 pos) 
    {
        this.SetActiveGameObject(true);
        transform.position = pos;
    }
}