using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItemSpawnController : ControllerBaseModel
{
    [SerializeField] private MultiplePoolModel roadPools;
    [SerializeField] private PoolModel passAreaPool;
    [SerializeField] private PoolModel pickableModelPool;
    [SerializeField] private PoolModel powerUpModelPool;
    [SerializeField] private FinishController finishController;
    private LevelModel activeLevel;
    private int roadIndex;
    private RoadModel lastSpawnedRoad;
    private Vector3 lastLevelPosition;
    private int passAreaIndex;
    private List<int> roadlineDataIndex;

    public override void Initialize()
    {
        base.Initialize();
        activeLevel = LevelController.Instance.ActiveLevel;
    }

    public override void ControllerUpdate(GameStates currentState)
    {
        base.ControllerUpdate(currentState);
        if (currentState == GameStates.Game)
        {
            float playerPos = PlayerController.Instance.transform.position.z;
            if (roadIndex == activeLevel.RoadDatas.Count - 1)
            {
                onLevelSpawnComplete();
            }
            for (int i = roadIndex; i < activeLevel.RoadDatas.Count; i++)
            {
                if (Mathf.Abs(playerPos - (activeLevel.RoadDatas[roadIndex].Position.z + lastLevelPosition.z)) < GameValues.LevelSpawnDistance)
                {
                    switch (activeLevel.RoadDatas[i].Type)
                    {
                        case WorldItemType.Road:
                            roadIndex = 1 + i;
                            RoadModel road = roadPools.GetDeactiveItem<RoadModel>(0);
                            Vector3 roadSpawnPos = lastLevelPosition + activeLevel.RoadDatas[i].Position;
                            road.OnSpawn(roadSpawnPos);
                            lastSpawnedRoad.NextRoad = road;
                            lastSpawnedRoad = road;
                            break;
                        case WorldItemType.PassArea:
                            roadIndex = 1 + i;
                            PassAreaModel passArea = passAreaPool.GetDeactiveItem<PassAreaModel>();
                            passArea.OnSpawn(activeLevel.PassAreaCounts[passAreaIndex], activeLevel.RoadDatas[i].Position + lastLevelPosition);
                            passArea.Initialize();
                            passAreaIndex++;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    break;
                }
            }

            for (int i = 0; i < activeLevel.LineDatas.Count; i++)
            {
                LineDataModel dataModel = activeLevel.LineDatas[i];
                int maxItemCount = dataModel.GetItemCount(PlayerDataModel.Data.CompletedLevelCount);
                maxItemCount = maxItemCount == 0 ? 1 : maxItemCount;

                for (int j = roadlineDataIndex[i]; j < maxItemCount; j++)
                {
                    Vector3 pos = dataModel.GetSpawnPosition(maxItemCount, j) + lastLevelPosition;

                    if (Mathf.Abs(playerPos - pos.z) < GameValues.RoadItemSpawnDistance)
                    {
                        roadlineDataIndex[i] = j + 1;
                        switch (dataModel.Type)
                        {
                            case RoadItemType.Pickable:
                                spawnPickable(dataModel.Id, pos);
                                break;
                            case RoadItemType.PowerUp:
                                spawnPowerUp(dataModel.Id, pos);
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }

    private void onLevelSpawnComplete()
    {
        lastLevelPosition += (activeLevel.RoadDatas.GetLastItem().Position);
        if (LevelController.Instance.CurrentLevelIndex + 1 < LevelController.Instance.Levels.Count)
        {
            LevelController.Instance.CurrentLevelIndex++;
        }
        else
        {
            LevelController.Instance.CurrentLevelIndex = 0;
        }
        activeLevel = LevelController.Instance.Levels[LevelController.Instance.CurrentLevelIndex];
        finishController.SetFinishLine(lastLevelPosition - new Vector3(0, 0, GameValues.FinishLineZOffset));
        roadIndex = 0;
        passAreaIndex = 0;
        roadlineDataIndex.Clear();
        for (int i = 0; i < activeLevel.LineDatas.Count; i++)
        {
            roadlineDataIndex.Add(0);
        }
    }

    public void InitializeLevel(int levelIndex)
    {
        lastLevelPosition = Vector3.zero;
        activeLevel = LevelController.Instance.Levels[levelIndex];
        float playerPos = PlayerController.Instance.transform.position.z;
        RoadModel firstRoad = null;
        roadlineDataIndex = new List<int>();

        for (int i = 0; i < activeLevel.RoadDatas.Count; i++)
        {
            if (Mathf.Abs(playerPos - (activeLevel.RoadDatas[roadIndex].Position.z + lastLevelPosition.z)) < GameValues.LevelSpawnDistance)
            {
                switch (activeLevel.RoadDatas[i].Type)
                {
                    case WorldItemType.Road:
                        roadIndex = i + 1;
                        RoadModel road = roadPools.GetDeactiveItem<RoadModel>(0);
                        Vector3 roadSpawnPos = lastLevelPosition + activeLevel.RoadDatas[i].Position;
                        road.OnSpawn(roadSpawnPos);
                        if (lastSpawnedRoad != null)
                        {
                            lastSpawnedRoad.NextRoad = road;
                        }
                        else
                        {
                            firstRoad = road;
                        }
                        lastSpawnedRoad = road;
                        break;
                    case WorldItemType.PassArea:
                        roadIndex = i + 1;
                        PassAreaModel passArea = passAreaPool.GetDeactiveItem<PassAreaModel>();
                        passArea.OnSpawn(activeLevel.PassAreaCounts[passAreaIndex], activeLevel.RoadDatas[i].Position + lastLevelPosition);
                        passArea.Initialize();
                        passAreaIndex++;
                        break;
                    default:
                        break;
                }

            }
            else
            {
                break;
            }
        }

        for (int i = 0; i < activeLevel.LineDatas.Count; i++)
        {
            roadlineDataIndex.Add(0);
            LineDataModel dataModel = activeLevel.LineDatas[i];
            int maxItemCount = dataModel.GetItemCount(PlayerDataModel.Data.CompletedLevelCount);
            maxItemCount = maxItemCount == 0 ? 1 : maxItemCount;

            for (int j = roadlineDataIndex[i]; j < maxItemCount; j++)
            {
                Vector3 pos = dataModel.GetSpawnPosition(maxItemCount, j) + lastLevelPosition;

                if (Mathf.Abs(playerPos - pos.z) < GameValues.RoadItemSpawnDistance)
                {
                    roadlineDataIndex[i] = j + 1;
                    switch (dataModel.Type)
                    {
                        case RoadItemType.Pickable:
                            spawnPickable(dataModel.Id, pos);
                            break;
                        case RoadItemType.PowerUp:
                            spawnPowerUp(dataModel.Id, pos);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    break;
                }
            }
        }
    }

    private void spawnPickable(int id, Vector3 pos)
    {
        PickableModel pickable = pickableModelPool.GetDeactiveItem<PickableModel>();
        pickable.OnSpawn(pos);
    }

    private void spawnPowerUp(int id, Vector3 pos)
    {
        UpgradeModel powerUp = powerUpModelPool.GetDeactiveItem<UpgradeModel>();
        powerUp.OnSpawn(pos);
    }
}