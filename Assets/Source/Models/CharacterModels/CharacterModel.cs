using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModel : ObjectModel
{
    [SerializeField] private LevelBarController levelBarController;

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "PassArea":
                PassAreaModel passArea = other.GetComponent<PassAreaModel>();
                onEnterPassArea(passArea);
                break;
            case "Finish":
                onLevelCompleted();
                break;
            case "PowerUp":
                UpgradeModel upgrade = other.GetComponent<UpgradeModel>();
                upgrade.SetActiveGameObject(false);
                Debug.Log("UPGRADED");
                break;
            default:
                break;
        }
    }

    private void onLevelCompleted()
    {
        LevelController.Instance.NextLevel();
        PlayerController.Instance.OnLevelCompleted();
        GameController.ChangeState(GameStates.Win);
    }

    private void onEnterPassArea(PassAreaModel passArea)
    {
        PlayerController.Instance.OnEnterPassArea();
        this.Invoke(() => checkArea(passArea), 2.5f);
    }

    private void onExitPassArea() 
    {
        PlayerController.Instance.OnExitPassArea();
    }

    private void checkArea(PassAreaModel passArea)
    {
        if (passArea.CheckArea())
        {
            onExitPassArea();
            passArea.OnPassedArea();
            levelBarController.OnAreaPassed();
        }
        else
        {
            GameController.ChangeState(GameStates.Lose);
            passArea.OnFailArea();
            levelBarController.OnAreaFailed();
        }
    }
}