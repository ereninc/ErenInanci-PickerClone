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
    public int CurrentLevelIndex;
    public List<LevelModel> Levels;
    [SerializeField] private EventModel onLevelComplete;
    [SerializeField] private WorldItemSpawnController worldSpawner;
    [SerializeField] private LevelBarController levelBarController;

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
        CurrentLevelIndex = PlayerDataModel.Data.LevelIndex;
        worldSpawner.Initialize();
        worldSpawner.InitializeLevel(CurrentLevelIndex);
    }

    public void NextLevel()
    {
        PlayerDataModel.Data.Level++;
        setLevel();
        PlayerDataModel.Data.Save();
        levelBarController.Initialize();
        onLevelComplete?.Invoke();
    }

    private void setLevel()
    {
        if (PlayerDataModel.Data.LevelIndex + 1 < Levels.Count)
        {
            PlayerDataModel.Data.LevelIndex++;
        }
        else
        {
            int randomLevel = Random.Range(Levels.Count - 5, Levels.Count);
            PlayerDataModel.Data.LevelIndex = randomLevel;
        }
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