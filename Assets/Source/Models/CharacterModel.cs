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
            PlayerController.Instance.ForwardSpeed = 0f;
            checkArea(passArea);
        }
    }

    private void checkArea(PassAreaModel passArea) 
    {
        if (passArea.CheckArea())
        {
            PlayerController.Instance.ForwardSpeed = 5f;
        }
        else
        {
            Debug.Log("LEVEL FAILED");
        }
    }
}