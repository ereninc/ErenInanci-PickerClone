using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpModel : ObjectModel
{
    public void OnSpawn(Vector3 pos) 
    {
        this.SetActiveGameObject(true);
        transform.position = pos;
    }

    public void OnCollect() 
    {
        this.SetActiveGameObject(false);
    }
}