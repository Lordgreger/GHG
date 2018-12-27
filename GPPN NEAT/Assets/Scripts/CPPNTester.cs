using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPPNTester : MonoBehaviour {

    List<CPPN.Neuron> inputNeurons = new List<CPPN.Neuron>() {
            new CPPN.Neuron(0, -1),
            new CPPN.Neuron(0, -1),
            new CPPN.Neuron(0, -1)
        };

    List<CPPN.Neuron> outputNeurons = new List<CPPN.Neuron>() {
            new CPPN.Neuron(0, -2),
            new CPPN.Neuron(0, -2)
        };

    List<CPPN.Neuron> hiddenNeurons = new List<CPPN.Neuron>() {
            new CPPN.Neuron(CPPN.Neuron.Type.sigmoid, 0),
            new CPPN.Neuron(CPPN.Neuron.Type.sine, 0),
            new CPPN.Neuron(CPPN.Neuron.Type.sigmoid, 1)
        };

    List<CPPN.Gene> hiddenGenes = new List<CPPN.Gene>() {
            new CPPN.Gene(CPPN.inputLayer, 0, 0, 0, true, 0.5f),
            new CPPN.Gene(CPPN.inputLayer, 1, 0, 1, true, 0.5f),
            new CPPN.Gene(CPPN.inputLayer, 2, 0, 1, true, 1f),

            new CPPN.Gene(0, 0, 1, 0, true, 0.5f),
            new CPPN.Gene(0, 1, 1, 0, true, 0.5f),

            new CPPN.Gene(0, 1, CPPN.outputLayer, 1, true, 1f),
            new CPPN.Gene(1, 0, CPPN.outputLayer, 0, true, 1f)
        };

    CPPN cppn = new CPPN();
    List<float> testVals;

    private void Start() {
        cppn.setInputNeurons(inputNeurons);
        cppn.setOutputNeurons(outputNeurons);
        testVals = new List<float>() { 1.0f, -1.0f, 0.0f };
    }

    private void FixedUpdate() {
        
        List<float> result = cppn.setInputGetoutput(testVals);
        Debug.Log("{ " + result[0] + ", " + result[1] + " }");
        testVals[0] = result[0];
        testVals[1] = result[1];
        testVals[2] += Time.deltaTime;
    }
}
