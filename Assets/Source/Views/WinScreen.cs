using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinScreen : ScreenElement
{
    public void NextLevel() 
    {
        PlayerController.Instance.OnNextLevel();
        GameController.ChangeState(GameStates.Game);
    }
}