using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseScreen : ScreenElement
{
    public void RestartLevel()
    {
        Animator.Play("Outro");
    }

    public void Reload()
    {
        GameController.ReloadGame();
    }
}