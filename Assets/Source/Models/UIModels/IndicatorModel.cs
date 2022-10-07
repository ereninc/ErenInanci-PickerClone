using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorModel : ObjectModel
{
    [SerializeField] private Image indicator;
    [SerializeField] private Color initialColor;
    [SerializeField] private Color passedColor;
    [SerializeField] private Color failedColor;

    public void OnLevelStart() 
    {
        indicator.color = initialColor;
    }

    public void OnAreaPassed() 
    {
        indicator.color = passedColor;
    }

    public void OnAreaFailed() 
    {
        indicator.color = failedColor;
    }
}
