using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NeuralNetwork 
{
    
    public int[] layers;
    public Neuron[] neurons;
    public Neuron[] inputs;
    public Neuron[] outputs;
    public NeuralNetwork(int[] _layers)
    {
        layers = _layers;
        List<Neuron> neuronsList = new List<Neuron>();
        List<Neuron> inputsList = new List<Neuron>();
        List<Neuron> outputsList = new List<Neuron>();
        int layersLength = layers.Length;
        int layer = 0;
        List<Neuron> lastNeurons = new List<Neuron>();
        List<Neuron> lastNeuronsTemp = new List<Neuron>();
        lastNeurons.Clear(); //just in case
        lastNeuronsTemp.Clear(); //just in case

        foreach (int neuron in layers)
        {
            //lastNeurons.Clear();
            lastNeurons = new List<Neuron>(lastNeuronsTemp);
            lastNeuronsTemp.Clear();
            int i = neuron;
            while (i > 0)
            {
                if (layer == 0)
                {
                    Neuron _neuron = new Neuron();
                    inputsList.Add(_neuron);
                    lastNeuronsTemp.Add(_neuron);
                }
                else
                {
                    Neuron _neuron = new Neuron(lastNeurons.ToArray());
                    if (layer == layersLength-1)
                        outputsList.Add(_neuron);
                    neuronsList.Add(_neuron);
                    lastNeuronsTemp.Add(_neuron);
                }
                i--;
            }
            layer++;
        }
        inputs = inputsList.ToArray();
        outputs = outputsList.ToArray();
        neurons = neuronsList.ToArray();
    }

    public void changeNetwork(NeuralNetwork network)
    {
        for (int i = 0; i < network.neurons.Length; i++)
        {
            for (int j = 0; j < network.neurons[i].weights.Length; j++)
            {
                neurons[i].weights[j] = network.neurons[i].weights[j];
            }
        }

        for (int i = 0; i < network.outputs.Length; i++)
        {
            for (int j = 0; j < network.outputs[i].weights.Length; j++)
            {
                outputs[i].weights[j] = network.outputs[i].weights[j];
            }
        }
    }

    public void CalculateOutputs()
    {
        foreach(Neuron neuron in neurons)
        {
            neuron.value = neuron.CalculateValue(); //Since they are in order there will be no disorder bugs
        }
    }
   public void Mutate()
    {
        foreach (Neuron neuron in neurons)
            neuron.Mutate();
    }
}
[System.Serializable]
public class Neuron
{
    public float[] weights;
    public float value;
    public Neuron[] linkedNeurons;


    public Neuron() { } // For inputs & outputs

    public Neuron(Neuron[] neurons) //For neuron inside the hidden layers
    {
        Debug.Log("alv");
        linkedNeurons = neurons;
        List<float> weigthsList = new List<float>();
        foreach(Neuron neuron in linkedNeurons)
        {
            weigthsList.Add(Random.Range(-1f, 1f));
        }
        weights = weigthsList.ToArray();
    }

    public float CalculateValue()
    {
        int i=0;
        float _value = 0;
        foreach (float weight in weights)
        {
            _value += weight * linkedNeurons[i].value;
            i++;
        }
        if (value < -45.0) value= 0.0f;
        else if (value > 45.0) value= 1.0f;
        else value =(float)( 1.0 / (1.0 + Mathf.Exp(-value)));
        //value = value <= 0 ? 0 : value; // ReLU (Rectified Linear Unit) Function

        return _value;
    }
    public void Mutate()
    {
        List<float> newWeights = new List<float>();
        foreach(float weight in weights)
        {
            float rnd = Random.Range(-0.3f, 0.3f);
            newWeights.Add( weight + rnd);
        }
        weights = new List<float>(newWeights).ToArray();
    }
}
