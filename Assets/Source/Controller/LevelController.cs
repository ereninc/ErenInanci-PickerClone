using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelController : ControllerBaseModel
{
    public static LevelController Instance;
    public LevelModel ActiveLevel;
    [SerializeField] private MultiplePoolModel roadPools;
    [SerializeField] private PoolModel passAreaPool;
    [SerializeField] private List<LevelModel> levels;
    [SerializeField] private FinishController finishController;
    [SerializeField] private int levelSpawnDistance;
    [SerializeField] private int finishlineZOffset;
    private int roadIndex;
    private RoadModel lastSpawnedRoad;
    private Vector3 lastLevelPosition;
    private int currentLevelIndex;
    private int passAreaIndex;

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
        currentLevelIndex = PlayerDataModel.Data.LevelIndex;
        initializeLevel(currentLevelIndex);
    }

    public override void ControllerUpdate(GameStates currentState)
    {
        base.ControllerUpdate(currentState);
        if (currentState == GameStates.Game)
        {
            float playerPos = PlayerController.Instance.transform.position.z;
            if (roadIndex == ActiveLevel.RoadDatas.Count - 1)
            {
                onLevelSpawnComplete();
            }
            for (int i = roadIndex; i < ActiveLevel.RoadDatas.Count; i++)
            {
                if (Mathf.Abs(playerPos - (ActiveLevel.RoadDatas[roadIndex].Position.z + lastLevelPosition.z)) < levelSpawnDistance)
                {
                    switch (ActiveLevel.RoadDatas[i].Type)
                    {
                        case WorldItemType.Road:
                            roadIndex = 1 + i;
                            RoadModel road = roadPools.GetDeactiveItem<RoadModel>(0);
                            Vector3 roadSpawnPos = lastLevelPosition + ActiveLevel.RoadDatas[i].Position;
                            road.OnSpawn(roadSpawnPos);
                            lastSpawnedRoad.NextRoad = road;
                            lastSpawnedRoad = road;
                            break;
                        case WorldItemType.PassArea:
                            roadIndex = 1 + i;
                            PassAreaModel passArea = passAreaPool.GetDeactiveItem<PassAreaModel>();
                            passArea.OnSpawn(ActiveLevel.PassAreaCounts[passAreaIndex], ActiveLevel.RoadDatas[i].Position + lastLevelPosition);
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
        }
    }

    private void onLevelSpawnComplete()
    {
        lastLevelPosition += (ActiveLevel.RoadDatas.GetLastItem().Position);
        if (currentLevelIndex + 1 < levels.Count)
        {
            currentLevelIndex++;
        }
        else
        {
            currentLevelIndex = 0;
        }
        ActiveLevel = levels[currentLevelIndex];
        finishController.SetFinishLine(lastLevelPosition + new Vector3(0, 0, finishlineZOffset));
        roadIndex = 0;
        passAreaIndex = 0;
    }

    private void initializeLevel(int levelIndex)
    {
        lastLevelPosition = Vector3.zero;
        ActiveLevel = levels[levelIndex];
        float playerPos = PlayerController.Instance.transform.position.z;
        RoadModel firstRoad = null;

        for (int i = 0; i < ActiveLevel.RoadDatas.Count; i++)
        {
            if (Mathf.Abs(playerPos - (ActiveLevel.RoadDatas[roadIndex].Position.z + lastLevelPosition.z)) < levelSpawnDistance)
            {
                switch (ActiveLevel.RoadDatas[i].Type)
                {
                    case WorldItemType.Road:
                        roadIndex = i + 1;
                        RoadModel road = roadPools.GetDeactiveItem<RoadModel>(0);
                        Vector3 roadSpawnPos = lastLevelPosition + ActiveLevel.RoadDatas[i].Position;
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
                        passArea.OnSpawn(ActiveLevel.PassAreaCounts[passAreaIndex], ActiveLevel.RoadDatas[i].Position + lastLevelPosition);
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
    }

    public void NextLevel()
    {
        PlayerDataModel.Data.Level++;
        PlayerDataModel.Data.LevelIndex = PlayerDataModel.Data.LevelIndex + 1 < levels.Count ? PlayerDataModel.Data.LevelIndex + 1 : 0;
        PlayerDataModel.Data.Save();
    }

    [EditorButton]
    public void E_CreateLevelModel()
    {
#if UNITY_EDITOR
        LevelModel level = getActiveLevel();

        var path = EditorUtility.SaveFilePanel("Save Level", "Assets", "", "asset");
        if (path.Length > 0)
        {
            AssetDatabase.CreateAsset(level, path.Remove(0, path.IndexOf("Assets")));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        ActiveLevel = null;
#endif
    }

    private LevelModel getActiveLevel()
    {
        LevelModel levelData = ScriptableObject.CreateInstance<LevelModel>();
        return levelData;
    }
}