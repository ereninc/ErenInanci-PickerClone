using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScreen : ScreenElement
{
    [SerializeField] private Text levelText, nextLevelText;

    public override void Initialize()
    {
        base.Initialize();
        UpdateLevel();
    }

    public void UpdateLevel()
    {
        levelText.text = PlayerDataModel.Data.Level.ToString();
        nextLevelText.text = (PlayerDataModel.Data.Level + 1).ToString();
    }
}