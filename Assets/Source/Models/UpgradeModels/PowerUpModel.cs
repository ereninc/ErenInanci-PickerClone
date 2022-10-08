using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpModel : ObjectModel
{
    public int Id;
    [SerializeField] private PowerUpTypeModel[] powerUpTypes;

    public void OnSpawn(Vector3 pos, int index) 
    {
        Id = index;
        powerUpTypes[Id].OnActivate();
        this.SetActiveGameObject(true);
        transform.position = pos;
    }

    public void OnCollect() 
    {
        powerUpTypes[Id].OnDeactive();
        this.SetActiveGameObject(false);
    }
}