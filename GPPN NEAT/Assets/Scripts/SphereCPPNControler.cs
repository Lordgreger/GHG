using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCPPNControler : MonoBehaviour {

    public float maxSpeed;

    Rigidbody rb;

    List<CPPN.Neuron> inputNeurons = new List<CPPN.Neuron>() {
            new CPPN.Neuron(CPPN.Neuron.Type.input, -1),
            new CPPN.Neuron(CPPN.Neuron.Type.input, -1),
            new CPPN.Neuron(CPPN.Neuron.Type.input, -1)
        };

    List<CPPN.Neuron> outputNeurons = new List<CPPN.Neuron>() {
            new CPPN.Neuron(CPPN.Neuron.Type.output, -2),
            new CPPN.Neuron(CPPN.Neuron.Type.output, -2)
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

    private void Start() {
        cppn.setInputNeurons(inputNeurons);
        cppn.setOutputNeurons(outputNeurons);
        //cppn.setHidden(CPPN.HiddenLayer.getNewRandomHiddenLayer(Random.Range(0, 10000), inputNeurons, outputNeurons));
        cppn.generateHidden(Random.Range(0, 10000));
        cppn.printOutputConnections();
        cppn.printHiddenInputs(0);
        cppn.printHiddenOutputs(0);
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        /*
        Debug.Log("{ " + rb.velocity.x + ", " + rb.velocity.z + " }");
        List<float> result = cppn.setInputGetoutput(new List<float>() { rb.velocity.x, rb.velocity.z, Time.time });
        result[0] = Mathf.Clamp(result[0], -maxSpeed, maxSpeed);
        result[1] = Mathf.Clamp(result[1], -maxSpeed, maxSpeed);
        rb.velocity = new Vector3(result[0], 0, result[1]);
        */

        Debug.Log("{ " + transform.position.x + ", " + transform.position.z + " }");
        List<float> result = cppn.setInputGetoutput(new List<float>() { transform.position.x, transform.position.x, Time.time + 1});
        transform.Translate(new Vector3(result[0], 0, result[1]) - transform.position);

    }


}
