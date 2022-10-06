using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishController : ControllerBaseModel
{
    public static FinishController Instance;
    [SerializeField] private FinishModel finishModel;

    public override void Initialize()
    {
        base.Initialize();
        if (Instance != null)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }

    public void SetFinishLine(Vector3 pos)
    {
        finishModel.transform.SetActiveGameObject(true);
        finishModel.transform.position = pos;
        Debug.Log("FINISH LINE SPAWNED");
    }
}