using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{

    [SerializeField] GameObject fruit;
    public static Vector2 fruitPos;

    public bool spawned = false;
    float width, height;

    [SerializeField] Snake snake;
    [SerializeField] AStarGrid grid;


    private void Start()
    {
        height = Camera.main.orthographicSize * 2;
        width = height * Camera.main.aspect;
    }

    private void Update()
    {
        if(!spawned)
        {
            do
            {
                fruitPos = AStarGrid.grid[Random.Range(0, AStarGrid.cellNumber)][Random.Range(0, AStarGrid.cellNumber)].center;
            }
            while (grid.CenterToBlock(fruitPos).blocked);
            Instantiate(fruit, fruitPos, Quaternion.identity, transform);
            spawned = true;
        }
    }

}
