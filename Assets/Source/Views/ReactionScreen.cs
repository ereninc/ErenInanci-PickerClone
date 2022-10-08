using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionScreen : ScreenElement
{
    [SerializeField] private ReactionModel reaction;
    public void ShowReaction() 
    {
        reaction.Show();
    }
}