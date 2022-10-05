using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableModel : ObjectModel
{
    public int Id;
    [SerializeField] private List<Transform> gfxs;

    public void OnSpawn(int Id) 
    {
        gfxs[Id].SetActiveGameObject(true);
    }

    public void OnEnterPassArea() 
    {
        this.SetActiveGameObject(false);
    }
}