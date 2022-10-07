using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PassAreaVisualModel : ObjectModel
{
    [SerializeField] private Renderer groundRenderer;
    [SerializeField] private PassAreaVisualDataModel visualDataModel;
    [SerializeField] private Transform ground;

    public override void Initialize()
    {
        base.Initialize();
        onSpawn();
    }

    private void onSpawn() 
    {
        ground.transform.localPosition = new Vector3(0, visualDataModel.InitialYPosition, 0);
        groundRenderer.material.color = visualDataModel.InitialGroundColor;
    }

    public void OnPassed() 
    {
        ground.transform.DOLocalMoveY(visualDataModel.TargetYPosition, 0.25f);
        groundRenderer.material.DOColor(visualDataModel.TargetGroundColor, 0.25f);
    }
}