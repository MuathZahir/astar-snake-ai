using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBrain : MonoBehaviour
{
    public Transform head;
    public NeuralNetwork network;
    float timeSurvived = 0f;
    
    float[] outputs;
    float[] inputs = new float[11];

    public float fitness = 0f;
    public static int score = 0;

    public bool AI = false;

    private void Awake()
    {
        network = new NeuralNetwork(11, 20, 20, 3);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 1; i < 10; i++)
        {
            Vector2 newVector = Quaternion.AngleAxis(18 * i, new Vector3(0, 0, 1)) * transform.right;
            Gizmos.DrawRay(transform.position, newVector * 10);
        }
    }

    private void FixedUpdate()
    {
        timeSurvived += Time.deltaTime;
        fitness = (timeSurvived + score * 10);

        for (int i = 1; i < 10; i++) //draws five debug rays as inputs
        {
            Vector2 newVector = Quaternion.AngleAxis(18 * i, new Vector3(0, 0, 1)) * transform.right;
            RaycastHit2D hit = Physics2D.Raycast(head.transform.position, newVector * 100);

            if (hit)
            {
                inputs[i - 1] = (100 - hit.distance) / 100;//return distance, 1 being close
                //foreach (var item in inputs)
                //{
                //    Debug.LogWarning(item);
                //}

            }
            else
            {
                inputs[i - 1] = 0;//if nothing is detected, will return 0 to network
            }
        }

        float distance = Vector2.Distance(FruitSpawner.fruitPos, transform.position);

        inputs[9] = (FruitSpawner.fruitPos.x - transform.position.x) / distance;
        inputs[10] = (FruitSpawner.fruitPos.y - transform.position.y) / distance;

        outputs = network.FeedForward(inputs);
        //Debug.Log("OUTPUT STARTED");
        //for (int i = 0; i < outputs.Length; i++)
        //{
        //    Debug.Log(outputs[i]);
        //}
        //Debug.Log("OUTPUT END");

        if (AI) ;

            //transform.parent.GetComponent<Snake>().MoveAI(outputs);
    }

    private void Update()
    {
        transform.position = Vector2.Lerp(transform.position, head.transform.position, Time.deltaTime * 30);
        transform.rotation = head.rotation;

        if (!AI)
            transform.GetComponentInParent<Snake>().Move();

    }

    public void Randomize()
    {
        network.InitWeights();
        network.InitBiases();
    }

    public void ToggleAI()
    {
        AI = !AI;
    }

}