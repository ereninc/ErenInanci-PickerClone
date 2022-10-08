using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModel : ObjectModel
{
    [SerializeField] private LevelBarController levelBarController;
    [SerializeField] private UpgradeController upgradeModel;
    private int upgradeIndex;

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
                PowerUpModel powerUp = other.GetComponent<PowerUpModel>();
                powerUp.OnCollect();
                upgradeIndex = powerUp.Id;
                onUpgradeStart(upgradeIndex);
                break;
            default:
                break;
        }
    }

    private void onLevelCompleted()
    {
        IncomeController.Instance.OnLevelCompleted();
        LevelController.Instance.NextLevel();
        PlayerController.Instance.OnLevelCompleted();
        GameController.ChangeState(GameStates.Win);
    }

    private void onEnterPassArea(PassAreaModel passArea)
    {
        onUpgradeEnd(upgradeIndex);
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

    private void onUpgradeStart(int index) 
    { 
        upgradeModel.OnStart(index);
    }

    private void onUpgradeEnd(int index)
    {
        upgradeModel.OnEnd(index);
    }
}