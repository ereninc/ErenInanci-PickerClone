using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModel : ObjectModel
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PassArea"))
        {
            Debug.Log("STOP");
            PassAreaModel passArea = other.GetComponent<PassAreaModel>();
            onEnterPassArea();
            this.Invoke(() => checkArea(passArea), 2f);
        }
    }

    private void onEnterPassArea()
    {
        PlayerController.Instance.OnEnterPassArea();
    }

    private void onExitPassArea() 
    {
        PlayerController.Instance.OnExitPassArea();
    }

    private void checkArea(PassAreaModel passArea)
    {
        if (passArea.IsPassed())
        {
            onExitPassArea();
        }
        else
        {
            Debug.Log("LEVEL FAILED");
        }
    }
}