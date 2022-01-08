using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An A* pathfinding grid
/// </summary>
public class AStarGrid : MonoBehaviour
{
    float cellSize;
    float height;

    public static int cellNumber;

    public static AStarBlock[][] grid;

    [SerializeField] GameObject line;

    private void Awake()
    {
        InitGrid();
    }

    /// <summary>
    /// Initializes the A* pathfinding grid.
    /// </summary>
    private void InitGrid()
    {
        cellSize = Snake.bodySize;
        height = Camera.main.orthographicSize * 2;
        cellNumber = Mathf.RoundToInt(height / cellSize);

        grid = new AStarBlock[cellNumber][];

        transform.position = new Vector3(-height / 2 + cellSize / 2, -height / 2 + cellSize / 2);

        for (int i = 0; i < cellNumber; i++)
        {
            grid[i] = new AStarBlock[cellNumber];

            DrawLine(new Vector2(transform.position.x + (i - 0.5f) * cellSize, 0), false);
            DrawLine(new Vector2(0, transform.position.y + (i - 0.5f) * cellSize), true);

            for (int j = 0; j < cellNumber; j++)
            {
                float centerX = transform.position.x + i * cellSize;
                float centerY = transform.position.y + j * cellSize;
                AStarBlock block = new AStarBlock(centerX, centerY, new Vector2Int(i, j));

                grid[i][j] = block;
            }
        }
        DrawLine(new Vector2(transform.position.x + (cellNumber - 0.5f) * cellSize, 0), false);
        DrawLine(new Vector2(0, transform.position.y + (cellNumber - 0.5f) * cellSize), true);

    }

    /// <summary>
    /// Finds the block that corresponds to the given coordinates.
    /// </summary>
    /// <param name="center">The coordinates of the center of the required block</param>
    /// <returns></returns>
    public AStarBlock CenterToBlock(Vector3 center)
    {
        int x = Mathf.RoundToInt((center.x - transform.position.x) / cellSize);
        int y = Mathf.RoundToInt((center.y - transform.position.y) / cellSize);

        x = Mathf.Clamp(x, 0, cellNumber - 1);
        y = Mathf.Clamp(y, 0, cellNumber - 1);

        return grid[x][y];
    }

    private void DrawLine(Vector2 center, bool horizontal)
    {
        GameObject obj = Instantiate(line, center, horizontal ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(0, 0, 90), transform);
        obj.transform.localScale = new Vector3(height, obj.transform.localScale.y, obj.transform.localScale.z);
    }
}
