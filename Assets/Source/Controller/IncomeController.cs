using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncomeController : ControllerBaseModel
{
    public static IncomeController Instance;
    [SerializeField] private Text totalIncomeText;
    [SerializeField] private int levelIncome;
    [SerializeField] private Text levelIncomeText;

    public override void Initialize()
    {
        base.Initialize();
        if (Instance!=null)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
        UpdateTotalIncome(0);
    }

    public void UpdateIncome(int value) 
    {
        levelIncome += value;
        levelIncomeText.text = "+" + levelIncome.ToCoinValues();
    }

    public void OnLevelCompleted() 
    {
        UpdateTotalIncome(levelIncome);
        levelIncomeText.text = "+" + levelIncome.ToCoinValues();
    }

    public void UpdateTotalIncome(int value)
    {
        PlayerDataModel.Data.Money += value;
        totalIncomeText.text = PlayerDataModel.Data.Money.ToCoinValues();
    }
}