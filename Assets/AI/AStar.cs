using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// An A* pathfinding algorithm
/// </summary>
public class AStar : MonoBehaviour
{
    
    List<AStarBlock> unvisitedList; // or open list
    HashSet<AStarBlock> visitedList; // or closed list

    AStarBlock current;

    public static bool pathReady = false;

    public bool debugging = false;

    List<AStarBlock> path;

    float ManhattanDistance(Vector3 a, Vector3 b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    float EuclideanDistance(Vector3 a, Vector3 b)
    {
        return Vector3.Distance(a, b);
    }

    /// <summary>
    /// Searches for the shortest path between the "start" and "end" blocks using the A* algorithm.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    public void Search(AStarBlock start, AStarBlock end)
    {
        unvisitedList = AStarGrid.grid.SelectMany(x => x).ToList();
        visitedList = new HashSet<AStarBlock>();
        current = start;

        current.f_score = EuclideanDistance(start.center, end.center);
        current.g_score = 0;

        while(unvisitedList.Count() > 0)
        {
            AStarBlock min = unvisitedList[0];
            for (int i = 1; i < unvisitedList.Count; i++)
            {
                if(unvisitedList[i].f_score < min.f_score)
                {
                    min = unvisitedList[i];
                    min.AStarIndex = i;
                }
                current = min;
            }

            unvisitedList.Remove(current);
            visitedList.Add(current);

            if (current == end)
            {
                FinalPath(end);
                break;
            }

            List<AStarBlock> neighbors = GetNeighbors(current);

            for (int i = 0; i < neighbors.Count; i++)
            {
                if (visitedList.Contains(neighbors[i]) || neighbors[i].blocked)
                    continue;

                float tentative_gScore = current.g_score + 1;

                if(tentative_gScore < neighbors[i].g_score)
                {
                    neighbors[i].g_score = tentative_gScore;
                    neighbors[i].f_score = tentative_gScore + EuclideanDistance(neighbors[i].center, end.center);
                    neighbors[i].previous = current;
                }
            }            

        }
        if (current == end)
            return;
        FinalPath(visitedList.OrderBy(x => x.f_score).Last());
    }

    /// <summary>
    /// Finds all of the neighboring blocks.
    /// </summary>
    /// <param name="block"></param>
    /// <returns>Returns a list of all of the blocks that are adjacent to the chosen block</returns>
    public List<AStarBlock> GetNeighbors(AStarBlock block)
    {
        List<AStarBlock> neighbors = new List<AStarBlock>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                if (Mathf.Abs(x) + Mathf.Abs(y) > 1) continue;

                if(IsOnGrid(block.index.x + x, block.index.y + y))
                {
                    neighbors.Add(AStarGrid.grid[block.index.x + x][block.index.y + y]);
                }

            }
        }
        


        return neighbors;
    }

    /// <summary>
    /// Retraces the shortest path to the target.
    /// </summary>
    /// <param name="target"></param>
    public void FinalPath(AStarBlock target)
    {
        path = new List<AStarBlock>();

        AStarBlock currentBlock = target;

        while (currentBlock.previous != null)
        {
            path.Add(currentBlock);
            currentBlock = currentBlock.previous;
        }

        path.Reverse();
        visitedList.Clear();
        unvisitedList.Clear();

        Snake.currentPath = path;

        StartCoroutine(transform.parent.GetComponent<Snake>().MoveAI(path));

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
            debugging = !debugging;
    }

    private void OnDrawGizmos()
    {
        if (!debugging || path == null)
            return;
        for (int i = 0; i < AStarGrid.grid.Count(); i++)
        {
            for (int j = 0; j < AStarGrid.grid[i].Count(); j++)
            {
                Gizmos.color = Snake.currentPath.Contains(AStarGrid.grid[i][j]) ? Color.green : Color.clear;

                if (AStarGrid.grid[i][j].center == FruitSpawner.fruitPos)
                    Gizmos.color = Color.red;

                Gizmos.DrawCube(AStarGrid.grid[i][j].center, Vector3.one * 0.4f);

            }
        }
    }
    bool IsOnGrid(int x, int y)
    {
        return x >= 0 && y >= 0 && x < AStarGrid.cellNumber && y < AStarGrid.cellNumber;
    }
}
