using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PassAreaModel : ObjectModel
{
    [SerializeField] private int maxPickableCount;
    [SerializeField] private int passedCounter;
    [SerializeField] private TextMeshPro passedCounterText;
    private bool isPassed;

    public override void Initialize()
    {
        base.Initialize();
        isPassed = false;
        passedCounter = 0;
        passedCounterText.text = passedCounter.ToString() + " / " + maxPickableCount.ToString();
    }

    //SEND BALLS TO OBJECT POOL & INCREASE ROAD HEIGHT & OPEN ROAD LOCK
    public void OnPassedArea() { }

    public void OnFailArea() { }

    public bool CheckArea()
    {
        if (passedCounter >= maxPickableCount)
        {
            OnPassedArea();
            isPassed = true;
            return true;
        }
        else
        {
            OnFailArea();
            isPassed = false;
            return false;
        }
    }

    public bool IsPassed() { return isPassed; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickable"))
        {
            onPickableEnter();
        }
    }

    private void onPickableEnter()
    {
        passedCounter++;
        passedCounterText.text = passedCounter.ToString() + " / " + maxPickableCount.ToString();
        CheckArea();
    }
}