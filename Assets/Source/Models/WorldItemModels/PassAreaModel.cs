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
    [SerializeField] private Animator animator;
    [SerializeField] private ReactionScreen reaction;
    [SerializeField] private IncomeController incomeController;
    private List<PickableModel> pickables;

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
        animator.Play("OnIdle", 0, 0);
        this.SetActiveGameObject(true);
    }

    public void OnPassedArea()
    {
        reaction.ShowReaction();
        passAreaVisualModel.OnPassed();
        animator.Play("OnPass", 0, 0);
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
            PickableModel pickable = other.GetComponent<PickableModel>();
            pickables.Add(pickable);
            onPickableEnter();
        }
    }

    private void onPickableEnter()
    {
        passedCounter++;
        passedCounterText.text = passedCounter.ToString() + " / " + maxPickableCount.ToString();
        incomeController.UpdateIncome(1);
    }
}