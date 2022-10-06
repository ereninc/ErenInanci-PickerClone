using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelBarController : ControllerBaseModel
{
    [SerializeField] private List<IndicatorModel> indicators;
    private int passedIndex;

    public override void Initialize()
    {
        base.Initialize();
        passedIndex = 0;
        onLevelStart();
    }

    private void onLevelStart()
    {
        for (int i = 0; i < indicators.Count; i++)
        {
            indicators[i].OnLevelStart();
        }
    }

    public void OnAreaPassed()
    {
        indicators[passedIndex].OnAreaPassed();
        passedIndex++;
    }

    public void OnAreaFailed() 
    {
        indicators[passedIndex].OnAreaFailed();
    }
}
