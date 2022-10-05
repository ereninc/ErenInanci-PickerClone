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
    [SerializeField] private List<LevelModel> levels;

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

        loadLevel();
    }

    private void loadLevel()
    {
        ActiveLevel = levels[PlayerDataModel.Data.LevelIndex];
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
