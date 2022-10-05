using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PassAreaModel : ObjectModel
{
    [SerializeField] private int maxPickableCount;
    [SerializeField] private int passedCounter;
    [SerializeField] private TextMeshPro passedCounterText;
    [SerializeField] private PassAreaVisualModel passAreaVisualModel;

    private List<PickableModel> pickables = new List<PickableModel>();

    public override void Initialize()
    {
        base.Initialize();
        pickables = new List<PickableModel>();
        passAreaVisualModel.Initialize();
        passedCounter = 0;
        passedCounterText.text = passedCounter.ToString() + " / " + maxPickableCount.ToString();
    }

    public void OnSpawn(int passCount, Vector3 position) 
    {
        maxPickableCount = passCount;
        transform.position = position;
        this.SetActiveGameObject(true);
    }

    //SEND BALLS TO OBJECT POOL & INCREASE ROAD HEIGHT & OPEN ROAD LOCK
    public void OnPassedArea() 
    {
        passAreaVisualModel.OnPassed();
        for (int i = 0; i < pickables.Count; i++)
        {
            pickables[i].OnEnterPassArea();
        }
    }

    public void OnFailArea() { }

    public bool CheckArea()
    {
        if (passedCounter >= maxPickableCount)
        {
            OnPassedArea();
            return true;
        }
        else
        {
            OnFailArea();
            return false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickable"))
        {
            pickables.Add(other.GetComponent<PickableModel>());
            onPickableEnter();
        }
    }

    private void onPickableEnter()
    {
        passedCounter++;
        passedCounterText.text = passedCounter.ToString() + " / " + maxPickableCount.ToString();
    }
}