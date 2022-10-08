﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeModel : ObjectModel
{
    public void OnStart() 
    {
        this.SetActiveGameObject(true);
    }

    public void OnEnd() 
    {
        this.SetActiveGameObject(false);
    }
}