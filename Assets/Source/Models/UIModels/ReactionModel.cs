using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionModel : ObjectModel
{
    [SerializeField] private Animator animator;
    public void Show()
    {
        this.SetActiveGameObject(true);
        animator.Play("Intro", 0, 0);
        Invoke(nameof(Hide), 1f);
    }

    public void Hide() 
    {
        animator.Play("Outro", 0, 0);
    }

    public void Deactivate()
    {
        this.SetActiveGameObject(false);
    }
}