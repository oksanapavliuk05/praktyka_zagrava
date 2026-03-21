using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class LevelEditorWindow : EditorWindow
{
    private const string DefaultSaveFolder = "Assets/GameData/Levels";
    private const string PrefabsFolder = "Assets/Prefabs";

    private LevelData currentLevel;
    private string newLevelName = "Level_001";

    private GameObject selectedBrush;
    private readonly List<GameObject> brushPrefabs = new List<GameObject>();
    private readonly Dictionary<string, GameObject> prefabLookup = new Dictionary<string, GameObject>();

    private EditorTab selectedTab = EditorTab.Items;

    private Vector2 paletteScroll;
    private Vector2 boardScroll;

    private enum EditorTab
    {
        Items = 0
    }

    [MenuItem("Game Editor/Level editor")]
    public static void Init()
    {
        LevelEditorWindow window = GetWindow<LevelEditorWindow>("Level editor");
        window.minSize = new Vector2(900f, 600f);
        window.Show();
    }

    private void OnEnable()
    {
        ReloadBrushes();
    }

    private void OnFocus()
    {
        ReloadBrushes();
    }

    private void OnProjectChange()
    {
        ReloadBrushes();
        Repaint();
    }

    private void OnGUI()
    {
        DrawHeader();
        EditorGUILayout.Space(8);

        DrawAssetSection();
        EditorGUILayout.Space(8);

        if (currentLevel == null)
        {
            EditorGUILayout.HelpBox("Create or assign a LevelData asset to start editing.", MessageType.Info);
            return;
        }

        EnsureLevelInitialized();

        DrawLevelSettings();
        EditorGUILayout.Space(8);

        DrawTabs();
        EditorGUILayout.Space(8);

        switch (selectedTab)
        {
            case EditorTab.Items:
                DrawItemsTab();
                break;
        }

        EditorGUILayout.Space(8);
        DrawBoard();

        HandleSaveShortcut();
    }

    private void DrawHeader()
    {
        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
        {
            EditorGUILayout.LabelField("Match-3 Level Editor", EditorStyles.boldLabel);

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();

                GUI.enabled = currentLevel != null;
                if (GUILayout.Button("Save", GUILayout.Width(120), GUILayout.Height(28)))
                {
                    SaveCurrentLevel();
                }

                GUI.enabled = true;

                if (GUILayout.Button("Reload Brushes", GUILayout.Width(140), GUILayout.Height(28)))
                {
                    ReloadBrushes();
                }
            }
        }
    }

    private void DrawAssetSection()
    {
        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
        {
            EditorGUILayout.LabelField("Assets", EditorStyles.boldLabel);

            currentLevel = (LevelData)EditorGUILayout.ObjectField("Current Level", currentLevel, typeof(LevelData), false);

            using (new EditorGUILayout.HorizontalScope())
            {
                newLevelName = EditorGUILayout.TextField("New Level Name", newLevelName);

                if (GUILayout.Button("Create New Level", GUILayout.Width(160)))
                {
                    CreateNewLevelAsset();
                }
            }

            EditorGUILayout.Space(4);
            EditorGUILayout.LabelField("Brush Source Folder", PrefabsFolder, EditorStyles.miniLabel);
        }
    }

    private void DrawLevelSettings()
    {
        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
        {
            EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();

            string levelId = EditorGUILayout.TextField("Level ID", currentLevel.levelId);
            LevelLimitType limitType = (LevelLimitType)EditorGUILayout.EnumPopup("Limit Type", currentLevel.limitType);
            int moveLimit = EditorGUILayout.IntField("Move Limit", currentLevel.moveLimit);
            int columns = Mathf.Max(1, EditorGUILayout.IntField("Columns", currentLevel.columns));
            int rows = Mathf.Max(1, EditorGUILayout.IntField("Rows", currentLevel.rows));

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(currentLevel, "Change Level Settings");

                currentLevel.levelId = levelId;
                currentLevel.limitType = limitType;
                currentLevel.moveLimit = Mathf.Max(1, moveLimit);

                if (columns != currentLevel.columns || rows != currentLevel.rows)
                {
                    currentLevel.Resize(columns, rows);
                }

                EditorUtility.SetDirty(currentLevel);
            }
        }
    }

    private void DrawTabs()
    {
        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Toggle(selectedTab == EditorTab.Items, "Items", EditorStyles.toolbarButton))
                selectedTab = EditorTab.Items;
        }
    }

    private void DrawItemsTab()
    {
        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
        {
            EditorGUILayout.LabelField("Brush / Items", EditorStyles.boldLabel);

            if (brushPrefabs.Count == 0)
            {
                EditorGUILayout.HelpBox($"No prefabs found in folder: {PrefabsFolder}", MessageType.Warning);
                return;
            }

            paletteScroll = EditorGUILayout.BeginScrollView(paletteScroll, GUILayout.Height(180));

            const int buttonsPerRow = 4;
            int index = 0;

            while (index < brushPrefabs.Count)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    for (int i = 0; i < buttonsPerRow && index < brushPrefabs.Count; i++, index++)
                    {
                        GameObject prefab = brushPrefabs[index];

                        if (prefab == null)
                        {
                            GUILayout.Space(110);
                            continue;
                        }

                        DrawPaletteButton(prefab);
                    }
                }

                EditorGUILayout.Space(4);
            }

            EditorGUILayout.EndScrollView();

            string selectedName = selectedBrush != null ? selectedBrush.name : "None";
            EditorGUILayout.LabelField($"Selected Brush: {selectedName}", EditorStyles.miniBoldLabel);
            EditorGUILayout.HelpBox("Left click to paint prefab. Right click to clear cell.", MessageType.None);
        }
    }

    private void DrawPaletteButton(GameObject prefab)
    {
        Color oldBg = GUI.backgroundColor;
        GUI.backgroundColor = selectedBrush == prefab ? new Color(0.22f, 0.55f, 0.75f) : Color.white;

        GUIContent content = BuildPaletteContent(prefab);

        if (GUILayout.Button(content, GUILayout.Width(110), GUILayout.Height(48)))
        {
            selectedBrush = prefab;
            Repaint();
        }

        GUI.backgroundColor = oldBg;
    }

    private GUIContent BuildPaletteContent(GameObject prefab)
    {
        Texture preview = GetPrefabPreview(prefab);
        return new GUIContent(prefab.name, preview);
    }

    private void DrawBoard()
    {
        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
        {
            EditorGUILayout.LabelField("Board", EditorStyles.boldLabel);

            boardScroll = EditorGUILayout.BeginScrollView(boardScroll);

            const float cellSize = 64f;

            for (int y = 0; y < currentLevel.rows; y++)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    for (int x = 0; x < currentLevel.columns; x++)
                    {
                        DrawCell(x, y, cellSize);
                    }
                }
            }

            EditorGUILayout.EndScrollView();
        }
    }

    private void DrawCell(int x, int y, float cellSize)
    {
        LevelCellData cell = currentLevel.GetCell(x, y);
        GameObject prefab = ResolvePrefab(cell != null ? cell.prefabName : null);

        Rect rect = GUILayoutUtility.GetRect(cellSize, cellSize, GUILayout.Width(cellSize), GUILayout.Height(cellSize));

        EditorGUI.DrawRect(rect, new Color(0.19f, 0.19f, 0.19f));

        if (prefab != null)
        {
            DrawCellPrefabPreview(rect, prefab);
        }
        else
        {
            Rect innerRect = new Rect(rect.x + 4, rect.y + 4, rect.width - 8, rect.height - 8);
            EditorGUI.DrawRect(innerRect, new Color(0.24f, 0.24f, 0.24f));
            GUI.Label(rect, "Empty", GetCenteredMiniLabel());
        }

        DrawCellBorder(rect);

        Event evt = Event.current;
        if (evt.type == EventType.MouseDown && rect.Contains(evt.mousePosition))
        {
            if (evt.button == 0)
            {
                PaintCell(x, y, selectedBrush != null ? selectedBrush.name : null);
                evt.Use();
            }
            else if (evt.button == 1)
            {
                PaintCell(x, y, null);
                evt.Use();
            }
        }
    }

    private void DrawCellPrefabPreview(Rect rect, GameObject prefab)
    {
        Rect innerRect = new Rect(rect.x + 4, rect.y + 4, rect.width - 8, rect.height - 8);
        EditorGUI.DrawRect(innerRect, new Color(0.24f, 0.24f, 0.24f));

        Texture preview = GetPrefabPreview(prefab);

        if (preview != null)
        {
            GUI.DrawTexture(innerRect, preview, ScaleMode.ScaleToFit, true);
        }
        else
        {
            GUI.Label(innerRect, prefab.name, GetCenteredMiniLabel());
        }
    }

    private void DrawCellBorder(Rect rect)
    {
        EditorGUI.DrawRect(new Rect(rect.x, rect.y, rect.width, 1), new Color(0.35f, 0.35f, 0.35f));
        EditorGUI.DrawRect(new Rect(rect.x, rect.yMax - 1, rect.width, 1), new Color(0.35f, 0.35f, 0.35f));
        EditorGUI.DrawRect(new Rect(rect.x, rect.y, 1, rect.height), new Color(0.35f, 0.35f, 0.35f));
        EditorGUI.DrawRect(new Rect(rect.xMax - 1, rect.y, 1, rect.height), new Color(0.35f, 0.35f, 0.35f));
    }

    private void PaintCell(int x, int y, string prefabName)
    {
        if (currentLevel == null)
            return;

        Undo.RecordObject(currentLevel, "Paint Level Cell");
        currentLevel.SetCellPrefab(x, y, prefabName);
        EditorUtility.SetDirty(currentLevel);
        Repaint();
    }

    private void ReloadBrushes()
    {
        brushPrefabs.Clear();
        prefabLookup.Clear();

        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { PrefabsFolder });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab == null)
                continue;

            brushPrefabs.Add(prefab);

            if (!prefabLookup.ContainsKey(prefab.name))
            {
                prefabLookup.Add(prefab.name, prefab);
            }
            else
            {
                Debug.LogWarning($"Duplicate prefab name found: {prefab.name}. Only first one will be used by Level Editor.");
            }
        }

        brushPrefabs.Sort((a, b) => string.CompareOrdinal(a.name, b.name));

        if (selectedBrush != null && !brushPrefabs.Contains(selectedBrush))
        {
            selectedBrush = null;
        }
    }

    private GameObject ResolvePrefab(string prefabName)
    {
        if (string.IsNullOrEmpty(prefabName))
            return null;

        prefabLookup.TryGetValue(prefabName, out GameObject prefab);
        return prefab;
    }

    private Texture GetPrefabPreview(GameObject prefab)
    {
        if (prefab == null)
            return null;

        Texture preview = AssetPreview.GetAssetPreview(prefab);

        if (preview == null)
            preview = AssetPreview.GetMiniThumbnail(prefab);

        return preview;
    }

    private void EnsureLevelInitialized()
    {
        currentLevel.columns = Mathf.Max(1, currentLevel.columns);
        currentLevel.rows = Mathf.Max(1, currentLevel.rows);
        currentLevel.moveLimit = Mathf.Max(1, currentLevel.moveLimit);
        currentLevel.EnsureGrid();
    }

    private void CreateNewLevelAsset()
    {
        if (string.IsNullOrWhiteSpace(newLevelName))
        {
            EditorUtility.DisplayDialog("Invalid Name", "Level name cannot be empty.", "OK");
            return;
        }

        if (!AssetDatabase.IsValidFolder(DefaultSaveFolder))
        {
            CreateFolderRecursive(DefaultSaveFolder);
        }

        LevelData asset = CreateInstance<LevelData>();
        asset.levelId = newLevelName;
        asset.columns = 8;
        asset.rows = 8;
        asset.moveLimit = 30;
        asset.limitType = LevelLimitType.Moves;
        asset.Resize(asset.columns, asset.rows);

        string filePath = AssetDatabase.GenerateUniqueAssetPath($"{DefaultSaveFolder}/{newLevelName}.asset");

        AssetDatabase.CreateAsset(asset, filePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        currentLevel = asset;
        Selection.activeObject = asset;
        EditorUtility.SetDirty(asset);
    }

    private void SaveCurrentLevel()
    {
        if (currentLevel == null)
            return;

        EditorUtility.SetDirty(currentLevel);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void HandleSaveShortcut()
    {
        Event e = Event.current;
        if (e.type == EventType.KeyDown && e.control && e.keyCode == KeyCode.S)
        {
            SaveCurrentLevel();
            e.Use();
        }
    }

    private static GUIStyle GetCenteredMiniLabel()
    {
        GUIStyle style = new GUIStyle(EditorStyles.miniLabel);
        style.alignment = TextAnchor.MiddleCenter;
        style.wordWrap = true;
        return style;
    }

    private static void CreateFolderRecursive(string fullPath)
    {
        string[] parts = fullPath.Split('/');
        string currentPath = parts[0];

        for (int i = 1; i < parts.Length; i++)
        {
            string nextPath = $"{currentPath}/{parts[i]}";
            if (!AssetDatabase.IsValidFolder(nextPath))
            {
                AssetDatabase.CreateFolder(currentPath, parts[i]);
            }

            currentPath = nextPath;
        }
    }
}