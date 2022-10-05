using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableModel : ObjectModel
{
    public void OnSpawn(Transform targetParent, Vector3 position) 
    {
        transform.SetParent(targetParent);
        transform.position = position;
        this.SetActiveGameObject(true);
    }

    public void OnEnterPassArea() 
    {
        this.SetActiveGameObject(false);
    }
}