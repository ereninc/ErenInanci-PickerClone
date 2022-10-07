using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelEditor : EditorWindow
{
    private LevelEditorData data;
    private LevelView activeLevelView;
    private Vector2 lineScrollPos, roadScrollPos, areaScrollPos;
    private bool isAutoSaved;
    private bool showRoads;
    private bool showAreas;
    private bool showInstructions;
    private int createRoadCount;
    private int passAreaCount;
    private GUIStyle instructionStyle;

    [MenuItem("Level Editor/Editor")]
    static void Init()
    {
        LevelEditor editor = GetWindow<LevelEditor>();
        editor.loadEditorData();
        editor.Show();
    }

    private void loadEditorData()
    {
        data = JsonConvert.DeserializeObject<LevelEditorData>(EditorPrefs.GetString("LevelEditorData"));
        data.ActiveTab = 0;
        initInstruction();
        if (data == null)
        {
            data = new LevelEditorData();
            data.LineDebugColors = new List<Color32>();
            data.LineDebugShowLines = new List<bool>();
            data.BezierLineColor = Color.white;
            data.RoadSize = new Vector2Int(5, 30);
        }
    }

    void OnFocus()
    {
        SceneView.duringSceneGui -= this.OnSceneGUI;
        SceneView.duringSceneGui += this.OnSceneGUI;
    }

    void OnDestroy()
    {
        EditorPrefs.SetString("LevelEditorData", JsonConvert.SerializeObject(data));
        SceneView.duringSceneGui -= this.OnSceneGUI;
    }

    private void OnGUI()
    {
        if (data == null)
        {
            loadEditorData();
        }

        if (EditorApplication.isCompiling)
        {
            if (isAutoSaved == false)
            {
                isAutoSaved = true;
                EditorPrefs.SetString("LevelEditorData", JsonConvert.SerializeObject(data));
            }
        }
        else
        {
            isAutoSaved = false;
        }
        mainView();
    }

    void OnSceneGUI(SceneView sceneView)
    {
        sceneUpdate();
    }

    #region Window

    private void mainView()
    {
        EditorGUILayout.Space(10);
        if (activeLevelView == null)
        {
            EditorGUILayout.LabelField("--Pages--");
        }
        else
        {
            EditorGUILayout.LabelField("Active Level : ");
            EditorGUILayout.ObjectField(activeLevelView.Level.Name, activeLevelView.Level, typeof(LevelModel), true);
        }
        EditorGUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();

        EditorGUI.BeginDisabledGroup(data.ActiveTab == 0);
        if (GUILayout.Button("Setting"))
        {
            data.ActiveTab = 0;
        }
        EditorGUI.EndDisabledGroup();
        EditorGUI.BeginDisabledGroup(data.ActiveTab == 1);

        if (GUILayout.Button("Edit Loaded Level or Create New"))
        {
            data.ActiveTab = 1;
        }
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(10);

        switch (data.ActiveTab)
        {
            case 0:
                settingsView();
                break;
            case 1:
                levelView();
                break;
            default:
                break;
        }
    }

    private void settingsView()
    {
        EditorGUILayout.LabelField("Save & Load");
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Load Level"))
        {
            var path = EditorUtility.OpenFilePanel("Save Level", "Assets", "asset");

            if (path.Length > 0)
            {
                LevelModel loadedLevel = (LevelModel)AssetDatabase.LoadMainAssetAtPath(path.Remove(0, path.IndexOf("Assets")));

                if (loadedLevel != null)
                {
                    loadLevel(loadedLevel);
                    data.ActiveTab = 1;
                    createRoadCount = activeLevelView.Level.RoadDatas.Count;
                    passAreaCount = activeLevelView.Level.PassAreaCounts.Count;
                }
            }
        }

        EditorGUI.BeginDisabledGroup(activeLevelView == null);
        if (GUILayout.Button("Save Level"))
        {
            var path = EditorUtility.SaveFilePanel("Save Level", "Assets", "", "asset");
            if (path.Length > 0)
            {
                AssetDatabase.CreateAsset(activeLevelView.Level, path.Remove(0, path.IndexOf("Assets")));
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            activeLevelView = null;
        }

        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Levels are editable after load.");
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("TEST");
        EditorGUILayout.Space(10);

        EditorGUILayout.LabelField("Close Loaded Level");
        EditorGUI.BeginDisabledGroup(activeLevelView == null);
        if (GUILayout.Button("Close Level"))
        {
            activeLevelView = null;
        }
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Delete");

        EditorGUI.BeginDisabledGroup(activeLevelView == null);
        if (GUILayout.Button("Delete Level"))
        {
            activeLevelView = null;
        }
        EditorGUI.EndDisabledGroup();

        showInstructions = EditorGUILayout.Foldout(showInstructions, "Show Instructions");
        if (showInstructions)
            showInstruction();
        EditorGUI.EndDisabledGroup();
    }

    private void loadLevel(LevelModel loadedLevel)
    {
        activeLevelView = new LevelView
        {
            Level = ScriptableObject.CreateInstance<LevelModel>(),
            LineDataViews = new List<LineDataView>()
        };

        activeLevelView.Level.LineDatas = new List<LineDataModel>();
        activeLevelView.Level.RoadDatas = new List<WorldItemDataModel>();
        activeLevelView.Level.PassAreaCounts = new List<int>();

        for (int i = 0; i < loadedLevel.RoadDatas.Count; i++)
        {
            activeLevelView.Level.RoadDatas.Add(loadedLevel.RoadDatas[i]);
        }

        for (int i = 0; i < loadedLevel.LineDatas.Count; i++)
        {
            LineDataModel lineData = loadedLevel.LineDatas[i];
            activeLevelView.Level.LineDatas.Add(new LineDataModel()
            {
                Id = lineData.Id,
                StartPoint = lineData.StartPoint,
                ControlPointA = lineData.ControlPointA,
                ControlPointB = lineData.ControlPointB,
                EndPoint = lineData.EndPoint,
                StartItemCount = lineData.StartItemCount,
                IncPerLevelCount = lineData.IncPerLevelCount,
                MaxItemCount = lineData.MaxItemCount,
                Type = lineData.Type,
            });
        }

        for (int i = 0; i < loadedLevel.PassAreaCounts.Count; i++)
        {
            activeLevelView.Level.PassAreaCounts.Add(loadedLevel.PassAreaCounts[i]);
        }

        for (int i = 0; i < activeLevelView.Level.LineDatas.Count; i++)
        {
            activeLevelView.LineDataViews.Add(new LineDataView()
            {
                IsShowed = false,
                LineData = activeLevelView.Level.LineDatas[i]
            });
        }
    }

    private void levelView()
    {
        if (activeLevelView == null)
        {
            if (GUILayout.Button("Create New Level"))
            {
                activeLevelView = new LevelView();
                activeLevelView.Level = ScriptableObject.CreateInstance<LevelModel>();
                setDefaultLevel();
            }
            return;
        }

        EditorGUILayout.LabelField("Road Setting");

        EditorGUILayout.BeginHorizontal();
        createRoadCount = EditorGUILayout.IntField(new GUIContent("Road Count", "Total Road Count"), createRoadCount);

        if (GUILayout.Button("Update Road Count"))
        {
            if (createRoadCount > activeLevelView.Level.RoadDatas.Count)
            {
                int diff = createRoadCount - activeLevelView.Level.RoadDatas.Count;
                for (int i = 0; i < diff; i++)
                {
                    Vector3 pos = activeLevelView.Level.RoadDatas.Count > 0 ? activeLevelView.Level.RoadDatas.GetLastItem().Position + new Vector3(0, 0, 30) : Vector3.zero;
                    WorldItemDataModel road = new WorldItemDataModel();
                    road.Position = pos;
                    activeLevelView.Level.RoadDatas.Add(road);
                }
            }
            else
            {
                int diff = activeLevelView.Level.RoadDatas.Count - createRoadCount;
                for (int i = 0; i < diff; i++)
                {
                    activeLevelView.Level.RoadDatas.Remove(activeLevelView.Level.RoadDatas.GetLastItem());
                }
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        showRoads = EditorGUILayout.Foldout(showRoads, "Show Roads");
        EditorGUILayout.Space(5);

        if (showRoads)
            drawRoads();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Pass Area Settings");
        EditorGUILayout.BeginHorizontal();
        showAreas = EditorGUILayout.Foldout(showAreas, "Show Areas");
        EditorGUILayout.Space(5);
        if (showAreas)
            drawAreas();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField("Lines");
        EditorGUILayout.Space(5);

        drawLineDebug();

        if (activeLevelView.LineDataViews == null)
        {
            activeLevelView.LineDataViews = new List<LineDataView>();
            activeLevelView.Level.LineDatas = new List<LineDataModel>();
        }

        EditorGUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        data.NewLineItemType = (RoadItemType)EditorGUILayout.EnumPopup("Item Type", data.NewLineItemType);

        if (GUILayout.Button("Add Line"))
        {
            LineDataModel lineData = new LineDataModel();
            LineDataView lineDataView = new LineDataView();

            lineDataView.LineData = lineData;
            lineData.Type = data.NewLineItemType;

            activeLevelView.Level.LineDatas.Add(lineData);
            activeLevelView.LineDataViews.Add(lineDataView);
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(10);

        lineScrollPos = EditorGUILayout.BeginScrollView(lineScrollPos);
        if (activeLevelView.Level.LineDatas.Count >0)
        {
            for (int i = 0; i < activeLevelView.Level.LineDatas.Count; i++)
            {
                drawLineDataModel(activeLevelView.LineDataViews[i]);
            }
        }
        EditorGUILayout.EndScrollView();
    }

    private void drawRoads()
    {
        if (activeLevelView.Level.RoadDatas.Count > 0)
        {
            roadScrollPos = EditorGUILayout.BeginScrollView(roadScrollPos, "Roads", GUILayout.MaxHeight(200));
            for (int i = 0; i < activeLevelView.Level.RoadDatas.Count; i++)
            {
                activeLevelView.Level.RoadDatas[i].Type = (WorldItemType)EditorGUILayout.EnumPopup("Road Type", activeLevelView.Level.RoadDatas[i].Type);
                activeLevelView.Level.RoadDatas[i].Position = EditorGUILayout.Vector3Field("Road_" + i, activeLevelView.Level.RoadDatas[i].Position);
            }
        }
        EditorGUILayout.EndScrollView();
    }

    private void drawAreas()
    {
        if (activeLevelView.Level.PassAreaCounts.Count > 0)
        {
            areaScrollPos = EditorGUILayout.BeginScrollView(areaScrollPos, "Areas", GUILayout.MaxHeight(150));
            for (int i = 0; i < activeLevelView.Level.PassAreaCounts.Count; i++)
            {
                activeLevelView.Level.PassAreaCounts[i] = EditorGUILayout.IntField(activeLevelView.Level.PassAreaCounts[i]);
            }
        }
        EditorGUILayout.EndScrollView();
    }

    private void drawLineDebug()
    {
        activeLevelView.ShowLineDebug = EditorGUILayout.Foldout(activeLevelView.ShowLineDebug, "Show Debug Settings");
        string[] itemTypes = System.Enum.GetNames(typeof(RoadItemType));

        if (data.LineDebugColors.Count < itemTypes.Length)
        {
            for (int i = 0; i < itemTypes.Length; i++)
            {
                if (i >= data.LineDebugColors.Count)
                {
                    data.LineDebugColors.Add(Color.white);
                }
            }
        }

        if (data.LineDebugShowLines.Count < itemTypes.Length)
        {
            for (int i = 0; i < itemTypes.Length; i++)
            {
                if (i >= data.LineDebugShowLines.Count)
                {
                    data.LineDebugShowLines.Add(true);
                }
            }
        }

        if (activeLevelView.ShowLineDebug)
        {
            EditorGUI.indentLevel++;
            for (int i = 0; i < itemTypes.Length; i++)
            {
                string enumName = itemTypes[i];
                data.LineDebugColors[i] = EditorGUILayout.ColorField(enumName + " Color", data.LineDebugColors[i]);
            }
            data.BezierLineColor = EditorGUILayout.ColorField("Bezier Line Color", data.BezierLineColor);
            data.RoadColor = EditorGUILayout.ColorField("Road Color", data.RoadColor);

            EditorGUILayout.Space(5);
            for (int i = 0; i < itemTypes.Length; i++)
            {
                string enumName = itemTypes[i];
                data.LineDebugShowLines[i] = EditorGUILayout.Toggle("Show " + enumName + " Line", data.LineDebugShowLines[i]);
            }
            EditorGUI.indentLevel--;
        }
    }

    private void drawLineDataModel(LineDataView lineView)
    {
        EditorGUILayout.Space(5);
        string name = "";

        switch (lineView.LineData.Type)
        {
            case RoadItemType.Pickable:
                name = "Pickable Line";
                break;
            case RoadItemType.PowerUp:
                name = "PowerUp Line";
                break;
            default:
                break;
        }

        EditorGUILayout.BeginHorizontal();
        lineView.IsShowed = EditorGUILayout.Foldout(lineView.IsShowed, name);

        if (GUILayout.Button("++ Pickable", GUILayout.MaxWidth(75)))
        {
            LineDataView lineData = new LineDataView();
            lineData.IsShowed = lineView.IsShowed;
            lineData.IsShowPositions = lineView.IsShowPositions;
            lineData.LineData = new LineDataModel();

            lineData.LineData.Id = lineView.LineData.Id;
            lineData.LineData.StartPoint = lineView.LineData.StartPoint;
            lineData.LineData.EndPoint = lineView.LineData.EndPoint;
            lineData.LineData.ControlPointA = lineView.LineData.ControlPointA;
            lineData.LineData.ControlPointB = lineView.LineData.ControlPointB;

            lineData.LineData.IncPerLevelCount = lineView.LineData.IncPerLevelCount;
            lineData.LineData.StartItemCount = ++lineView.LineData.StartItemCount;
            lineData.LineData.MaxItemCount = ++lineView.LineData.MaxItemCount;
            lineData.LineData.Type = lineView.LineData.Type;
        }

        if (GUILayout.Button("-- Pickable", GUILayout.MaxWidth(75)))
        {
            LineDataView lineData = new LineDataView();
            lineData.IsShowed = lineView.IsShowed;
            lineData.IsShowPositions = lineView.IsShowPositions;
            lineData.LineData = new LineDataModel();

            lineData.LineData.Id = lineView.LineData.Id;
            lineData.LineData.StartPoint = lineView.LineData.StartPoint;
            lineData.LineData.EndPoint = lineView.LineData.EndPoint;
            lineData.LineData.ControlPointA = lineView.LineData.ControlPointA;
            lineData.LineData.ControlPointB = lineView.LineData.ControlPointB;

            lineData.LineData.IncPerLevelCount = lineView.LineData.IncPerLevelCount;
            lineData.LineData.StartItemCount = --lineView.LineData.StartItemCount;
            lineData.LineData.MaxItemCount = --lineView.LineData.MaxItemCount;
            lineData.LineData.Type = lineView.LineData.Type;
        }

        if (GUILayout.Button("Copy", GUILayout.MaxWidth(50)))
        {
            LineDataView lineData = new LineDataView();
            lineData.IsShowed = lineView.IsShowed;
            lineData.IsShowPositions = lineView.IsShowPositions;
            lineData.LineData = new LineDataModel();

            lineData.LineData.Id = lineView.LineData.Id;
            lineData.LineData.StartPoint = lineView.LineData.StartPoint;
            lineData.LineData.EndPoint = lineView.LineData.EndPoint;
            lineData.LineData.ControlPointA = lineView.LineData.ControlPointA;
            lineData.LineData.ControlPointB = lineView.LineData.ControlPointB;

            lineData.LineData.IncPerLevelCount = lineView.LineData.IncPerLevelCount;
            lineData.LineData.MaxItemCount = lineView.LineData.MaxItemCount;
            lineData.LineData.Type = lineView.LineData.Type;

            activeLevelView.Level.LineDatas.Add(lineData.LineData);
            activeLevelView.LineDataViews.Add(lineData);
        }

        if (GUILayout.Button("-", GUILayout.MaxWidth(50)))
        {
            activeLevelView.Level.LineDatas.Remove(lineView.LineData);
            activeLevelView.LineDataViews.Remove(lineView);
        }

        EditorGUILayout.EndHorizontal();

        if (lineView.IsShowed)
        {
            EditorGUI.indentLevel++;
            lineView.LineData.Type = (RoadItemType)EditorGUILayout.EnumPopup("Change Type", lineView.LineData.Type);

            switch (lineView.LineData.Type)
            {
                case RoadItemType.Pickable:
                    lineView.LineData.Id = EditorGUILayout.IntField(new GUIContent("Pickable Id", "-1 for Random Pickable"), lineView.LineData.Id);
                    lineView.LineData.StartItemCount = EditorGUILayout.IntField(new GUIContent("Start Pickable Count", "Start Pickable Count in Single Line"), lineView.LineData.StartItemCount);
                    lineView.LineData.MaxItemCount = EditorGUILayout.IntField(new GUIContent("Max Pickable Count", "Max Pickable Count in Single Line"), lineView.LineData.MaxItemCount);
                    lineView.LineData.IncPerLevelCount = EditorGUILayout.IntField(new GUIContent("Increase Count", "Increase count per level"), lineView.LineData.IncPerLevelCount);
                    break;
                case RoadItemType.PowerUp:
                    lineView.LineData.Id = EditorGUILayout.IntField(new GUIContent("Powerup Id", "-1 for Random Powerups"), lineView.LineData.Id);
                    lineView.LineData.StartItemCount = EditorGUILayout.IntField(new GUIContent("Total PowerUp Count", "Total PowerUp Count in Single Line"), lineView.LineData.StartItemCount);
                    break;
                default:
                    break;
            }

            lineView.IsShowPositions = EditorGUILayout.Foldout(lineView.IsShowPositions, "Show Control Points");

            if (lineView.IsShowPositions)
            {
                EditorGUI.indentLevel++;
                lineView.LineData.StartPoint = EditorGUILayout.Vector3Field("Start Point", lineView.LineData.StartPoint);
                lineView.LineData.ControlPointA = EditorGUILayout.Vector3Field("Control Point A", lineView.LineData.ControlPointA);
                lineView.LineData.ControlPointB = EditorGUILayout.Vector3Field("Control Point B", lineView.LineData.ControlPointB);
                lineView.LineData.EndPoint = EditorGUILayout.Vector3Field("End Point", lineView.LineData.EndPoint);
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
        }

    }

    private void showInstruction()
    {
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("LEVEL CREATION INSTRUCTIONS", instructionStyle);
        EditorGUILayout.LabelField("Step 1: To create level, go to Edit or Create Level Page", instructionStyle);
        EditorGUILayout.LabelField("Step 2: Create Level and edit variables.", instructionStyle);
        EditorGUILayout.LabelField("Step 3: Then go Setting page and Save Level.", instructionStyle);

        EditorGUILayout.Space(25);
        EditorGUILayout.LabelField("LEVEL EDIT INSTRUCTIONS", instructionStyle);
        EditorGUILayout.LabelField("Step 1: To edit level, Load Level from Setting page", instructionStyle);
        EditorGUILayout.LabelField("Step 2: Once you load level, page will changes automatically to edit page.", instructionStyle);
        EditorGUILayout.LabelField("Step 3: Edit level variables.", instructionStyle);
        EditorGUILayout.LabelField("Step 4: Then go Setting page and Save Level.", instructionStyle);
        EditorGUILayout.Space(10);
    }

    private void initInstruction()
    {
        showInstructions = true;
        instructionStyle = new GUIStyle();
        instructionStyle.normal.textColor = Color.red;
        instructionStyle.fontSize = 18;
        instructionStyle.fontStyle = FontStyle.Bold;
    }

    #endregion

    #region Scene

    private void sceneUpdate()
    {
        if (activeLevelView != null)
        {
            if (activeLevelView.Level.RoadDatas.Count > 0)
            {
                for (int i = activeLevelView.Level.RoadDatas.Count - 1; i >= 0; i--)
                {
                    activeLevelView.Level.RoadDatas[i].Position = drawRoad(activeLevelView.Level.RoadDatas[i].Position);
                }
            }
            else
            {
                activeLevelView.Level.RoadDatas.Add(new WorldItemDataModel());
            }

            if (activeLevelView.LineDataViews != null)
            {
                if (activeLevelView.Level.LineDatas.Count > 0)
                {
                    for (int i = 0; i < activeLevelView.LineDataViews.Count; i++)
                    {
                        drawSceneLineDataModel(activeLevelView.Level.LineDatas[i]);
                    }
                }
            }
            else
            {
                activeLevelView.Level.LineDatas.Add(new LineDataModel());
            }
        }
    }

    private Vector3 drawRoad(Vector3 pos)
    {
        Vector3[] positions = new Vector3[4];
        positions[0] = pos + new Vector3(-5, 0, -30);
        positions[1] = pos + new Vector3(-5, 0, 0);
        positions[2] = pos + new Vector3(5, 0, 0);
        positions[3] = pos + new Vector3(5, 0, -30);
        Handles.DrawSolidRectangleWithOutline(positions, data.RoadColor, data.RoadColor);
        drawSphere(pos, 1, data.RoadColor);

        if (showRoads)
            pos = drawTransformHandle(pos, 0);

        return pos;
    }

    private void drawSceneLineDataModel(LineDataModel lineData)
    {
        if (data.LineDebugShowLines[(int)lineData.Type] == false)
        {
            return;
        }

        lineData.StartPoint = drawTransformHandle(lineData.StartPoint, 0);
        lineData.ControlPointA = drawTransformHandle(lineData.ControlPointA, 0);
        lineData.ControlPointB = drawTransformHandle(lineData.ControlPointB, 0);
        lineData.EndPoint = drawTransformHandle(lineData.EndPoint, 0);

        drawLine(lineData.StartPoint, lineData.ControlPointA, 1, data.BezierLineColor);
        drawLine(lineData.ControlPointA, lineData.ControlPointB, 1, data.BezierLineColor);
        drawLine(lineData.ControlPointB, lineData.EndPoint, 1, data.BezierLineColor);

        drawLabel("Start Point", lineData.StartPoint, data.BezierLineColor);
        drawLabel("Control A Point", lineData.ControlPointA, data.BezierLineColor);
        drawLabel("Control B Point", lineData.ControlPointB, data.BezierLineColor);
        drawLabel("End Point", lineData.EndPoint, data.BezierLineColor);
        Color color = data.LineDebugColors[(int)lineData.Type];

        for (int i = 0; i < lineData.StartItemCount; i++)
        {
            float currentTime = (float)(i) / (float)lineData.StartItemCount;
            float nextTime = (float)(i + 1) / (float)lineData.StartItemCount;

            Vector3 currentPos = Helpers.Maths.CalculateCubicBezierPoint(currentTime, lineData.StartPoint, lineData.ControlPointA, lineData.ControlPointB, lineData.EndPoint);
            Vector3 nextPos = Helpers.Maths.CalculateCubicBezierPoint(nextTime, lineData.StartPoint, lineData.ControlPointA, lineData.ControlPointB, lineData.EndPoint);

            drawSphere(currentPos, 1, color);
            drawLine(currentPos, nextPos, 1, color);
        }
    }

    private void drawSphere(Vector3 pos, float r, Color color)
    {
        Color defColor = Handles.color;
        Handles.color = color;
        Handles.SphereHandleCap(0, pos, Quaternion.identity, r, EventType.Repaint);
        Handles.color = defColor;
    }

    private void drawLabel(string label, Vector3 pos, Color color)
    {
        Color defColor = Handles.color;
        Handles.color = color;
        Handles.Label(pos, label);
        Handles.color = defColor;
    }

    private void drawLine(Vector3 aPoint, Vector3 bPoint, float thickness, Color color)
    {
        Color defColor = Handles.color;
        Handles.color = color;
        Handles.DrawLine(aPoint, bPoint);
        Handles.color = defColor;
    }

    private Vector3 drawTransformHandle(Vector3 pos, int type)
    {
        if (Tools.current == Tool.Move)
        {
            EditorGUI.BeginChangeCheck();
            Handles.SetCamera(SceneView.lastActiveSceneView.camera);
            Vector3 position = Vector3.zero;

            switch (type)
            {
                case 0:
                    position = Handles.PositionHandle(pos, Quaternion.identity);
                    break;
                case 1:
                    position = Handles.FreeMoveHandle(pos, Quaternion.identity, 1, Vector3.one, Handles.SphereHandleCap);
                    break;
                default:
                    break;
            }

            if (EditorGUI.EndChangeCheck())
            {
                pos = position;
            }
        }

        return pos;
    }

    private void setDefaultLevel()
    {
        activeLevelView.Level = new LevelModel();
        activeLevelView.Level.RoadDatas = new List<WorldItemDataModel>();
        activeLevelView.Level.LineDatas = new List<LineDataModel>();
        activeLevelView.Level.PassAreaCounts = new List<int>();
        activeLevelView.LineDataViews = new List<LineDataView>();
        activeLevelView.Level.Name = "New Level";
        WorldItemDataModel roadTemp = new WorldItemDataModel();
        createRoadCount = 1;
        roadTemp.Type = WorldItemType.Road;
        roadTemp.Position = Vector3.zero;
        activeLevelView.Level.RoadDatas.Add(roadTemp);
        activeLevelView.Level.PassAreaCounts.Add(1);
        LineDataModel lineTemp = new LineDataModel();
        LineDataView lineView = new LineDataView();
        lineView.LineData = lineTemp;
        lineTemp.Type = RoadItemType.Pickable;
        lineTemp.StartPoint = Vector3.zero;
        lineTemp.EndPoint = Vector3.one;
        lineTemp.ControlPointA = Vector3.zero;
        lineTemp.ControlPointB = Vector3.one;
        lineTemp.StartItemCount = 1;
        lineTemp.MaxItemCount = 1;
        activeLevelView.Level.LineDatas.Add(lineTemp);
        activeLevelView.LineDataViews.Add(lineView);
    }

    #endregion
}

public class LevelEditorData
{
    public int ActiveTab = 0;
    public RoadItemType NewLineItemType;
    public WorldItemType WorldItemType;
    public List<Color32> LineDebugColors;
    public Color32 BezierLineColor;
    public Color32 RoadColor;
    public List<bool> LineDebugShowLines;
    public Vector2Int RoadSize;
}

public class LevelView
{
    public LevelModel Level;
    public List<LineDataView> LineDataViews;
    public bool ShowLineDebug;
}

public class LineDataView
{
    public LineDataModel LineData;
    public bool IsShowed;
    public bool IsShowPositions;
}