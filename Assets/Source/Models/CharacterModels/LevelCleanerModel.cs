using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCleanerModel : ObjectModel
{
    private void OnTriggerEnter(Collider other)
    {
        ObjectModel obj = other.GetComponent<ObjectModel>();
        if (obj != null)
        {
            obj.SetActiveGameObject(false);
        }
    }
}