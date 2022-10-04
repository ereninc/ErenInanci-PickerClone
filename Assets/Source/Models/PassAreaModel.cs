using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PassAreaModel : ObjectModel
{
    [SerializeField] private int maxPickableCount;
    [SerializeField] private int passedCounter;
    [SerializeField] private TextMeshPro passedCounterText;

    public override void Initialize()
    {
        base.Initialize();
        passedCounter = 0;
        passedCounterText.text = passedCounter.ToString() + " / " + maxPickableCount.ToString();
    }

    public void OnPickableEnter()
    {
        passedCounter++;
        passedCounterText.text = passedCounter.ToString() + " / " + maxPickableCount.ToString();
    }

    //SEND BALLS TO OBJECT POOL & INCREASE ROAD HEIGHT & OPEN ROAD LOCK
    public void OnPassedArea()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickable"))
        {
            OnPickableEnter();
        }
    }
}