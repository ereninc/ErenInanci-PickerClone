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
            PlayerController.Instance.ForwardSpeed = 0f;
        }
    }
}