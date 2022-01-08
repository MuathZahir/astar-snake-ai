//using Numpy;
using System;
using System.IO;
using System.Collections.Generic;

public class NeuralNetwork : IComparable<NeuralNetwork>
{
    int[] layers;
    float[][][] weights;
    float[][] neurons;
    public float[][] biases;
    public float fitness = 0;

    public NeuralNetwork(params int[] layers)
    {
        this.layers = new int[layers.Length];

        for (int i = 0; i < layers.Length; i++)
        {
            this.layers[i] = layers[i];
        }

        InitNeurons();
        InitWeights();
        InitBiases();

    }

    //////////// Initializations ////////////
    private void InitNeurons()
    {

        neurons = new float[layers.Length][];

        for (int i = 0; i < layers.Length; i++)
        {
            neurons[i] = new float[layers[i]];
        }

    }

    public void InitWeights()
    {
        weights = new float[layers.Length - 1][][];

        for (int i = 0; i < layers.Length - 1; i++)
        {
            weights[i] = new float[layers[i+1]][];

            for (int j = 0; j < weights[i].Length; j++)
            {
                weights[i][j] = new float[layers[i]];

                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    weights[i][j][k] = UnityEngine.Random.Range(-0.5f, 0.5f);
                }
            }
        }

    }

    public void InitBiases()
    {
        biases = new float[layers.Length][];

        for (int i = 0; i < layers.Length; i++)
        {
            biases[i] = new float[layers[i]];

            for (int j = 0; j < biases[i].Length; j++)
            {
                biases[i][j] = UnityEngine.Random.Range(-0.5f, 0.5f);
            }
        }
    }

    //////////// Activation Function ////////////
    
    private float Activation(float n)
    {
        return MathF.Tanh(n);
    }

    //////////// Feed Forward ////////////

    public float[] FeedForward(params float[] inputs)
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            neurons[0][i] = inputs[i];
        }

        for (int i = 1; i < layers.Length; i++)
        {
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float value = 0f;
                for (int k = 0; k < neurons[i-1].Length; k++)
                {
                    value += weights[i - 1][j][k] * neurons[i - 1][k];
                }

                value += biases[i][j];

                neurons[i][j] = Activation(value);
            }
        }

        return neurons[neurons.Length - 1];
    }

    //////////// Mutations ////////////
    
    public void Mutate(int chance, float val)
    {
        for (int i = 0; i < biases.Length; i++)
        {
            for (int j = 0; j < biases[i].Length; j++)
            {

                biases[i][j] = (UnityEngine.Random.Range(0f, chance) <= 5) ? biases[i][j] += UnityEngine.Random.Range(-val, val) : biases[i][j];

            }
        }

        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    weights[i][j][k] = (UnityEngine.Random.Range(0f, chance) <= 5) ? weights[i][j][k] += UnityEngine.Random.Range(-val, val) : weights[i][j][k];
                }
            }
        }
    }


    //////////// Helper Functions ////////////

    public int CompareTo(NeuralNetwork other)
    {
        if (other == null) return 1;

        return (fitness < other.fitness) ? -1 : (fitness > other.fitness) ? 1 : 0;
    }

    //////////// Copy Network ////////////
    
    public NeuralNetwork(NeuralNetwork copyNetwork)
    {
        for (int i = 0; i < copyNetwork.biases.Length; i++)
        {
            for (int j = 0; j < copyNetwork.biases[i].Length; j++)
            {
                biases[i][j] = copyNetwork.biases[i][j];
            }
        }

        for (int i = 0; i < copyNetwork.weights.Length; i++)
        {
            for (int j = 0; j < copyNetwork.weights[i].Length; j++)
            {
                for (int k = 0; k < copyNetwork.weights[i][j].Length; k++)
                {
                    weights[i][j][k] = copyNetwork.weights[i][j][k];
                }
            }
        }

    }

    //////////// Load and Save ////////////

    public void Load(string path)
    {
        TextReader tr = new StreamReader(path);
        int NumberOfLines = (int)new FileInfo(path).Length;
        string[] ListLines = new string[NumberOfLines];
        int index = 1;
        for (int i = 1; i < NumberOfLines; i++)
        {
            ListLines[i] = tr.ReadLine();
        }
        tr.Close();
        if (new FileInfo(path).Length > 0)
        {
            for (int i = 0; i < biases.Length; i++)
            {
                for (int j = 0; j < biases[i].Length; j++)
                {
                    biases[i][j] = float.Parse(ListLines[index]);
                    index++;
                }
            }

            for (int i = 0; i < weights.Length; i++)
            {
                for (int j = 0; j < weights[i].Length; j++)
                {
                    for (int k = 0; k < weights[i][j].Length; k++)
                    {
                        weights[i][j][k] = float.Parse(ListLines[index]); ;
                        index++;
                    }
                }
            }
        }
    }

    public void Save(string path)
    {
        File.Create(path).Close();
        StreamWriter writer = new StreamWriter(path, true);

        for (int i = 0; i < biases.Length; i++)
        {
            for (int j = 0; j < biases[i].Length; j++)
            {
                writer.WriteLine(biases[i][j]);
            }
        }

        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    writer.WriteLine(weights[i][j][k]);
                }
            }
        }
        writer.Close();
    }

}
