﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeModel : ObjectModel
{
    public void OnActive()
    { 
        this.SetActiveGameObject(true);
    }

    public void OnDeactive() 
    {
        this.SetActiveGameObject(false); 
    }
}
