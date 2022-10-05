using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModel : ObjectModel
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PassArea"))
        {
            PassAreaModel passArea = other.GetComponent<PassAreaModel>();
            onEnterPassArea();
            this.Invoke(() => checkArea(passArea), 2f);
        }
    }

    private void onEnterPassArea()
    {
        Debug.Log("OnEnterPassArea -> Stop");
        PlayerController.Instance.OnEnterPassArea();
    }

    private void onExitPassArea() 
    {
        Debug.Log("OnExitPassArea -> Continue");
        PlayerController.Instance.OnExitPassArea();
    }

    private void checkArea(PassAreaModel passArea)
    {
        if (passArea.CheckArea())
        {
            onExitPassArea();
            passArea.OnPassedArea(); //This will have animations and particle
        }
        else
        {
            Debug.Log("LEVEL FAILED -> SetGameState = Lose");
            GameController.ChangeState(GameStates.Lose);
            passArea.OnFailArea(); //Animation or particle maybe
        }
    }
}