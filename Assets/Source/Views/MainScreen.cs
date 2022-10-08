using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainScreen : ScreenElement
{
    [SerializeField] private Transform levelParent, moneyParent;

    public void StartGame()
    {
        levelParent.SetParent(ScreenController.GetScreen<ScreenElement>(2).transform);
        moneyParent.SetParent(ScreenController.GetScreen<ScreenElement>(2).transform);
        GameController.ChangeState(GameStates.Game);
    }
}