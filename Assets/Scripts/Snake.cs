using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Snake : MonoBehaviour
{
    [SerializeField] GameObject body;
    [SerializeField] GameObject head;
    [SerializeField] List<GameObject> tail = new List<GameObject>();
    [SerializeField] AStarGrid AStarGrid;
    [SerializeField] public static float speed = 10f;
    
    //The current direction of the snake
    Vector3 dir = new Vector3(0, bodySize);

    float timeBTWMoves = 0f;

    bool moved = true;
    bool AI = false;
    bool runAI = true; // A bool to make sure "MoveAI" runs once

    public static AStarBlock currentBlock;
    public static List<AStarBlock> currentPath;
    public static int score = 0;
    public static bool add = false;
    public static float bodySize = 0.5f;


    [SerializeField] Text scoreText;


    private void Awake()
    {
        InitScore();
        currentBlock = AStarGrid.grid[8][1];
    }

    public void AddBody()
    {
        //Update the score when fruit is taken
        scoreText.text = score.ToString();

        GameObject thing = Instantiate(body, head.transform.position - dir, Quaternion.identity, transform);
        AStarGrid.CenterToBlock(thing.transform.position).blocked = true;
        tail.Insert(0, thing);
    }

    private void Update()
    {
        if (!AI)
        {
            Time.timeScale = 1;
            Move();
            runAI = true;
        }

        if (AI)
        {
            if (runAI)
            {
                head.GetComponent<AStar>().Search(currentBlock, AStarGrid.CenterToBlock(FruitSpawner.fruitPos));
                runAI = false;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) && Time.timeScale < 3)
                Time.timeScale *= 1.3f;
            else if(Input.GetKeyDown(KeyCode.LeftArrow) && Time.timeScale > 0)
                Time.timeScale /= 1.3f;
        }


        //Toggle the AI
        if (Input.GetKeyDown(KeyCode.Space))
            AI = !AI;
    }

    /// <summary>
    /// Returns a true or false value based on whether the given block has more than one exit
    /// </summary>
    /// <param name="block"></param>
    /// <returns></returns>
    private bool NoExit(AStarBlock block)
    {
        List<AStarBlock> neighbors = head.GetComponent<AStar>().GetNeighbors(block);

        int count = 0;

        foreach (AStarBlock neighbor in neighbors)
        {
            if (!neighbor.blocked)
                count++;
        }

        return count <= 1;
    }

    public IEnumerator MoveAI(List<AStarBlock> path)
    {
        if (path.Count != 0 && !NoExit(path[path.Count - 1]))
        {
            dir = path[0].center - currentBlock.center;
        }
        else
        {
            List<AStarBlock> neighbors = head.GetComponent<AStar>().GetNeighbors(currentBlock);

            foreach (AStarBlock block in neighbors)
            {
                if (!block.blocked && !NoExit(block))
                {
                    dir = block.center - currentBlock.center;
                }
            }
        }

        head.transform.position += dir;
        currentBlock = AStarGrid.CenterToBlock(new Vector3(head.transform.position.x, head.transform.position.y));
        if (add && dir != Vector3.zero)
        {
            AddBody();
            add = false;
        }
        else if (tail.Count != 0)
        {
            AStarGrid.CenterToBlock(tail[tail.Count - 1].transform.position).blocked = false;
            tail[tail.Count - 1].transform.position = head.transform.position - dir;
            AStarGrid.CenterToBlock(tail[tail.Count - 1].transform.position).blocked = true;
            tail.Insert(0, tail[tail.Count - 1]);
            tail.RemoveAt(tail.Count - 1);
        }

        yield return new WaitForSeconds(1/speed);

        for (int i = 0; i < AStarGrid.grid.Length; i++)
        {
            for (int j = 0; j < AStarGrid.grid[i].Length; j++)
            {
                AStarGrid.grid[i][j].previous = null;
                AStarGrid.grid[i][j].f_score = int.MaxValue;
                AStarGrid.grid[i][j].g_score = int.MaxValue;
                AStarGrid.grid[i][j].AStarIndex = 0;
            }
        }
        if(AI)
            head.GetComponent<AStar>().Search(currentBlock, AStarGrid.CenterToBlock(FruitSpawner.fruitPos));

    }
    

    public void Move()
    {
        if (Input.GetKeyDown("up") && dir != new Vector3(0, -bodySize) && moved)
        {
            dir = new Vector3(0, bodySize);
            head.transform.rotation = Quaternion.Euler(0, 0, 0);
            moved = false;
        }
        else if (Input.GetKeyDown("down") && dir != new Vector3(0, bodySize) && moved)
        {
            dir = new Vector3(0, -bodySize);
            head.transform.rotation = Quaternion.Euler(0, 0, 180);
            moved = false;
        }
        else if (Input.GetKeyDown("left") && dir != new Vector3(bodySize, 0) && moved)
        {
            dir = new Vector3(-bodySize, 0);
            head.transform.rotation = Quaternion.Euler(0, 0, 90);
            moved = false;
        }
        else if (Input.GetKeyDown("right") && dir != new Vector3(-bodySize, 0) && moved)
        {
            dir = new Vector3(bodySize, 0);
            head.transform.rotation = Quaternion.Euler(0, 0, -90);
            moved = false;
        }
        if (timeBTWMoves >= 1 / speed)
        {
            head.transform.position += dir;
            currentBlock = AStarGrid.CenterToBlock(new Vector3(head.transform.position.x, head.transform.position.y));

            if (add)
            {
                AddBody();
                add = false;
            }
            else
            {
                moved = true;
                if (tail.Count == 0) { timeBTWMoves = 0; return; }
                AStarGrid.CenterToBlock(tail[tail.Count - 1].transform.position).blocked = false;
                tail[tail.Count - 1].transform.position = head.transform.position - dir;
                AStarGrid.CenterToBlock(tail[tail.Count - 1].transform.position).blocked = true;
                tail.Insert(0, tail[tail.Count - 1]);
                tail.RemoveAt(tail.Count - 1);
            }
            timeBTWMoves = 0;
            moved = true;
        }
        else
        {
            timeBTWMoves += Time.deltaTime;
        }


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(head.transform.position, dir);
    }


    void InitScore()
    {
        score = 0;
        scoreText.text = score.ToString();
    }

}
