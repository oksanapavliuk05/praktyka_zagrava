using System;
using System.Collections.Generic;
using UnityEngine;

public enum LevelLimitType
{
    Moves = 0
}

[Serializable]
public class LevelCellData
{
    public string prefabName;
}

[CreateAssetMenu(fileName = "LevelData", menuName = "Match3/Level Data", order = 1)]
public class LevelData : ScriptableObject
{
    [Header("Meta")]
    public string levelId = "Level_001";

    [Header("Limits")]
    public LevelLimitType limitType = LevelLimitType.Moves;
    public int moveLimit = 30;

    [Header("Board")]
    [Min(1)] public int columns = 8;
    [Min(1)] public int rows = 8;

    [SerializeField]
    private List<LevelCellData> cells = new List<LevelCellData>();

    public IReadOnlyList<LevelCellData> Cells => cells;

    public void Resize(int newColumns, int newRows)
    {
        newColumns = Mathf.Max(1, newColumns);
        newRows = Mathf.Max(1, newRows);

        List<LevelCellData> newCells = new List<LevelCellData>(newColumns * newRows);

        for (int y = 0; y < newRows; y++)
        {
            for (int x = 0; x < newColumns; x++)
            {
                LevelCellData newCell = new LevelCellData();

                if (x < columns && y < rows)
                {
                    int oldIndex = GetIndex(x, y, columns);
                    if (oldIndex >= 0 && oldIndex < cells.Count && cells[oldIndex] != null)
                    {
                        newCell.prefabName = cells[oldIndex].prefabName;
                    }
                }

                newCells.Add(newCell);
            }
        }

        columns = newColumns;
        rows = newRows;
        cells = newCells;
    }

    public void EnsureGrid()
    {
        int targetCount = Mathf.Max(1, columns) * Mathf.Max(1, rows);

        if (cells == null)
            cells = new List<LevelCellData>(targetCount);

        while (cells.Count < targetCount)
            cells.Add(new LevelCellData());

        while (cells.Count > targetCount)
            cells.RemoveAt(cells.Count - 1);
    }

    public LevelCellData GetCell(int x, int y)
    {
        EnsureGrid();

        if (x < 0 || x >= columns || y < 0 || y >= rows)
            return null;

        return cells[GetIndex(x, y, columns)];
    }

    public void SetCellPrefab(int x, int y, string prefabName)
    {
        EnsureGrid();

        if (x < 0 || x >= columns || y < 0 || y >= rows)
            return;

        cells[GetIndex(x, y, columns)].prefabName = prefabName;
    }

    public static int GetIndex(int x, int y, int width)
    {
        return y * width + x;
    }
}