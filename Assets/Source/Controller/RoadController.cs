using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadController : ControllerBaseModel
{
    public static RoadController Instance;
    [SerializeField] private MultiplePoolModel roadPools;
    [SerializeField] private PoolModel passAreaPool;
    [SerializeField] private PoolModel pickablePool;
    [SerializeField] private LevelModel activeLevel;

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
        activeLevel = LevelController.Instance.ActiveLevel;
        LoadLevel();
    }

    public void LoadLevel()
    {

    }
}