using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeController : ControllerBaseModel
{
    [SerializeField] private UpgradeModel[] upgrades;

    public void OnStart(int index) 
    {
        upgrades[index].OnActive();
    }

    public void OnEnd(int index)
    {
        upgrades[index].OnDeactive();
    }
}