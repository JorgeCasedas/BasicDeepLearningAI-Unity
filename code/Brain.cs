using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
    public InputsOutputsController values;
    public int[] layers;
    public NeuralNetwork neuralNetwork;

    void Start()
    {
        values = GetComponent<InputsOutputsController>();
        neuralNetwork = new NeuralNetwork(layers);
    }

    // Update is called once per frame
    void Update()
    {
        neuralNetwork.CalculateOutputs();
        values.torque = neuralNetwork.outputs[0].value;
    }
}
