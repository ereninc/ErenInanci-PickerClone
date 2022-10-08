using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpTypeModel : ObjectModel
{
    public void OnActivate() 
    { 
        this.SetActiveGameObject(true); 
    }

    public void OnDeactive() 
    { 
        this.SetActiveGameObject(false); 
    }
}
