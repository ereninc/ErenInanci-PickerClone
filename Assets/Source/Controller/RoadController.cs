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
    public List<RoadDataModel> SampleRoads;

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
        for (int i = 0; i < activeLevel.RoadDatas.Count; i++)
        {
            RoadDataModel roadData = activeLevel.RoadDatas[i];
            RoadModel road = roadPools.GetDeactiveItem<RoadModel>(roadData.Id);
            roadData.Pickables = SampleRoads[Random.Range(0, SampleRoads.Count)].Pickables;
            road.OnSpawn(roadData.Position);

            for (int j = 0; j < roadData.Pickables.Count; j++)
            {
                PickableDataModel dataModel = roadData.Pickables[j];
                PickableModel pickableModel = pickablePool.GetDeactiveItem<PickableModel>();
                pickableModel.OnSpawn(road.PickableParent, dataModel.Position);
            }
        }

        for (int i = 0; i < activeLevel.PassAreaDatas.Count; i++)
        {
            PassAreaDataModel passAreaData = activeLevel.PassAreaDatas[i];
            PassAreaModel passArea = passAreaPool.GetDeactiveItem<PassAreaModel>();
            passArea.OnSpawn(passAreaData.PassCount, passAreaData.Position);
            passArea.Initialize();
        }
    }
}