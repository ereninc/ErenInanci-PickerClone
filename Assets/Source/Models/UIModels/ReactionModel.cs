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
        Invoke(nameof(hide), 1.5f);
    }

    private void hide() 
    {
        animator.Play("Outro", 0, 0);
    }

    public void Deactivate()
    {
        this.SetActiveGameObject(false);
    }
}