using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyMath {
    public static float Sigmoid(float value) {
        return 1.0f / (1.0f + Mathf.Exp(-value));
    }

    public static float Guassian(float value) {
        return (1.0f / Mathf.Sqrt(2.0f * Mathf.PI)) * Mathf.Exp(Mathf.Pow(-value, 2) / 2);
    }

    public static float StepBelow(float value, float thresh) {
        if (value > thresh)
            return 1;
        return 0;
    }
}

public class CPPN {

    public static int inputLayer = -1;
    public static int outputLayer = -2;

    List<Neuron> inputNeurons;
    SortedList<int, List<Neuron>> hiddenNeurons;
    List<Neuron> outputNeurons;

    public CPPN() {
        inputNeurons = new List<Neuron>();
        outputNeurons = new List<Neuron>();
        hiddenNeurons = new SortedList<int, List<Neuron>>();
    }

    public void setInputNeurons(List<Neuron> i) {
        inputNeurons = i;
    }

    public void setOutputNeurons(List<Neuron> o) {
        outputNeurons = o;
    }

    public void setHidden(SortedList<int, List<Neuron>> h) {
        hiddenNeurons = h;
    }

    public void generateHidden(int seed) {
        Random.InitState(seed);
        List<Neuron> neurons = new List<Neuron>();

        int numberOfNeuronsInLayer = Random.Range(2, 5);
        for (int i = 0; i < numberOfNeuronsInLayer; i++) {
            neurons.Add(new Neuron((Neuron.Type)Random.Range(0, (int)Neuron.Type.amountOfTypes), i));
        }
        hiddenNeurons.Add(0, neurons);
        Debug.Log("Generated " + hiddenNeurons[0].Count + " neurons in hidden layer");

        // Outgoing connections to input neurons
        List<Neuron> tmpCollection = new List<Neuron>();
        tmpCollection.AddRange(hiddenNeurons[0]);
        tmpCollection.AddRange(outputNeurons);
        foreach (var n in inputNeurons) {
            Connection c = new Connection();
            c.from = n;
            c.to = tmpCollection[Random.Range(0, tmpCollection.Count)];
            c.from.outputs.Add(c);
            c.to.inputs.Add(c);
            c.weight = Random.Range(-1f, 1f);
        }

        Debug.Log("Generated " + hiddenNeurons[0].Count + " neurons in hidden layer");

        // Incomming connections to output neurons
        tmpCollection.Clear();
        tmpCollection.AddRange(hiddenNeurons[0]);
        tmpCollection.AddRange(inputNeurons);
        foreach (var n in outputNeurons) {
            if (n.inputs.Count == 0) { // Add only if no inputs
                Connection c = new Connection();
                c.to = n;
                c.from = tmpCollection[Random.Range(0, tmpCollection.Count)];
                c.from.outputs.Add(c);
                c.to.inputs.Add(c);
                c.weight = Random.Range(-1f, 1f);
            }
        }

        // Incomming and outgoing connections to hidden neurons
        foreach (var n in hiddenNeurons[0]) {
            // Incomming
            if (n.inputs.Count == 0) { // Add only if no inputs
                Connection c = new Connection();
                c.to = n;
                c.from = inputNeurons[Random.Range(0, inputNeurons.Count)];
                c.from.outputs.Add(c);
                c.to.inputs.Add(c);
                c.weight = Random.Range(-1f, 1f);
            }
            // Outgoing
            if (n.outputs.Count == 0) { // Add only if no outputs
                Connection c = new Connection();
                c.from = n;
                c.to = outputNeurons[Random.Range(0, outputNeurons.Count)];
                c.from.outputs.Add(c);
                c.to.inputs.Add(c);
                c.weight = Random.Range(-1f, 1f);
            }
        }
    }

    public List<float> setInputGetoutput(List<float> inputVals) {
        List<float> output = new List<float>();

        if (inputNeurons.Count != inputVals.Count) {
            Debug.Log("Input mismatch");
            return output;
        }

        // Insert input
        for (int i = 0; i < inputVals.Count; i++) {
            inputNeurons[i].setVal(inputVals[i]);
            Debug.Log("Added input value " + inputVals[i]);
        }

        printInputVals();

        runHiddenNeurons();

        foreach (var n in outputNeurons) {
            //Debug.Log("Got Output " + n.getVal());
            output.Add(n.getVal());
        }

        return output;
    }

    public void runHiddenNeurons() {
        foreach (var i in hiddenNeurons.Keys) {
            foreach (var j in hiddenNeurons[i]) {
                j.setValFromInputs();
            }
        }
    }

    // Prints
    public void printOutputConnections() {

        string output = "INPUT CONNECTIONS, OUTPUT:";
        int i = 0;
        foreach (var n in outputNeurons) {
            output += "\n Out" + i + ":";
            foreach (var c in n.inputs) {
                output += " " + c.from.type;
            }
            i++;
        }
                        

        Debug.Log(output);
    }

    public void printHiddenInputs(int layer) {
        string output = "INPUT CONNECTIONS, LAYER " + layer + ", COUNT " + hiddenNeurons[layer].Count + ";";
        int i = 0;
        foreach (var n in hiddenNeurons[layer]) {
            output += "\n N" + i + ":";
            foreach (var c in n.inputs) {
                output += " " + c.from.type;
            }
            i++;
        }
        Debug.Log(output);
    }

    public void printHiddenOutputs(int layer) {
        string output = "OUTPUT CONNECTIONS, LAYER " + layer + ", COUNT " + hiddenNeurons[layer].Count + ";";
        int i = 0;
        foreach (var n in hiddenNeurons[layer]) {
            output += "\n N" + i + ":";
            foreach (var c in n.outputs) {
                output += " " + c.to.type;
            }
            i++;
        }
        Debug.Log(output);
    }

    public void printInputVals() {

        string output = "INPUT Vals:";
        int i = 0;
        foreach (var n in inputNeurons) {
            output += "\n I" + i + ": " + n.getVal();
            i++;
        }


        Debug.Log(output);
    }

    // Neuron = Node
    public class Neuron {
        public enum Type {
            output = -2,
            input = -1,
            sigmoid,
            gaussian,
            sine,
            step,
            amountOfTypes
        }

        public List<Connection> inputs = new List<Connection>();
        public List<Connection> outputs = new List<Connection>();
        public Type type;
        public int layer;
        float inputVal;

        public Neuron(Type itype, int ilayer) {
            type = itype;
            layer = ilayer;
            inputVal = 0;
        }

        public Neuron() {
            type = Type.input;
            layer = -1;
            inputVal = 0;
        }

        public void setVal(float val) {
            inputVal = val;
        }

        public void setValFromInputs() {
            inputVal = 0;
            foreach (var c in inputs) {
                inputVal += c.from.getVal() * c.weight;
            }
            Debug.Log("Input val " + inputVal);
        }

        public void addVal(float val) {
            inputVal += val;
        }

        public float getVal() {
            switch (type) {

                case Type.input:
                    return inputVal;

                case Type.output:
                    return inputVal;

                case Type.sigmoid:
                    return MyMath.Sigmoid(inputVal);

                case Type.gaussian:
                    return MyMath.Guassian(inputVal);

                case Type.sine:
                    return Mathf.Sin(inputVal);

                case Type.step:
                    return MyMath.StepBelow(inputVal, 0f);

                default:
                    return inputVal;
            }
        }
    }

    // Gene = Edge = Connection
    public class Gene {
        public Gene (int ifromLayer, int ifromID, int itoLayer, int itoID, bool ienabled, float iweight) {
            fromID = ifromID;
            fromLayer = ifromLayer;
            toID = itoID;
            toLayer = itoLayer;
            enabled = ienabled;
            weight = iweight;
        }

        public int fromID;
        public int fromLayer;
        public int toID;
        public int toLayer;
        public bool enabled;
        public float weight;
    }

    public struct Connection {
        public Neuron from;
        public Neuron to;
        public float weight;
    }

    public class HiddenLayer {
        Dictionary<int, List<Neuron>> neurons;

        public HiddenLayer() {
            neurons = new Dictionary<int, List<Neuron>>();
        }

        public HiddenLayer(List<Neuron> n) {

        }

        public void apply() {

        }

        public static HiddenLayer getNewRandomHiddenLayer(int seed, List<Neuron> inputs, List<Neuron> outputs) {

            Random.InitState(seed);
            List<Neuron> neurons = new List<Neuron>();

            int numberOfNeuronsInLayer = Random.Range(0, 3);
            for (int i = 0; i < numberOfNeuronsInLayer; i++) {
                neurons.Add(new Neuron((Neuron.Type)Random.Range(0, (int)Neuron.Type.amountOfTypes), i));
            }

            // Outgoing connections to input neurons
            List<Neuron> tmpCollection = neurons;
            tmpCollection.AddRange(outputs);
            foreach (var n in inputs) {
                Connection c = new Connection();
                c.from = n;
                c.to = tmpCollection[Random.Range(0, tmpCollection.Count)];
                c.from.outputs.Add(c);
                c.to.inputs.Add(c);
                c.weight = Random.Range(-1f, 1f);
            }

            // Incomming connections to output neurons
            tmpCollection.Clear();
            tmpCollection.AddRange(neurons);
            tmpCollection.AddRange(outputs);
            foreach (var n in outputs) {
                Connection c = new Connection();
                c.to = n;
                c.from = tmpCollection[Random.Range(0, tmpCollection.Count)];
                c.from.outputs.Add(c);
                c.to.inputs.Add(c);
                c.weight = Random.Range(-1f, 1f);
            }

            // Incomming and outgoing connections to hidden neurons
            foreach (var n in neurons) {
                // Incomming
                if (n.inputs.Count == 0) { // Add only if no inputs
                    Connection c = new Connection();
                    c.to = n;
                    c.from = inputs[Random.Range(0, inputs.Count)];
                    c.from.outputs.Add(c);
                    c.to.inputs.Add(c);
                    c.weight = Random.Range(-1f, 1f);
                }
                // Outgoing
                if (n.outputs.Count == 0) { // Add only if no outputs
                    Connection c = new Connection();
                    c.from = n;
                    c.to = outputs[Random.Range(0, outputs.Count)];
                    c.from.outputs.Add(c);
                    c.to.inputs.Add(c);
                    c.weight = Random.Range(-1f, 1f);
                }
            }

            HiddenLayer h = new HiddenLayer();
            h.neurons.Add(0, neurons);
            return h;

            /*
            Random.InitState(seed);
            List<Neuron> neurons = new List<Neuron>();
            List<Gene> genes = new List<Gene>();
            List<int> numberOfNeuronsInLayers = new List<int>();

            for (int i = 0; i < numberOfLayers; i++) {
                int numberOfNeuronsInLayer = Random.Range(0, maxNumberOfNeuronsInLayer);
                numberOfNeuronsInLayers.Add(numberOfNeuronsInLayer);
                for (int j = 0; j < numberOfNeuronsInLayer; j++) {
                    neurons.Add(new Neuron((Neuron.Type)Random.Range(0, (int)Neuron.Type.amountOfTypes), i));
                }
            }

            // Connections to layer 0 (only input as available from)
            int numberOfConnectionsToLayer = Random.Range(0, maxConnectionsToLayer + 1);
            for (int i = 0; i < numberOfConnectionsToLayer; i++) {
                int toNeuron = Random.Range(0, numberOfNeuronsInLayers[0]);
                int fromNeuron = Random.Range(0, numberOfInputs);
                genes.Add(new Gene(-1, fromNeuron, 0, toNeuron, true, Random.Range(-1f, 1f)));
            }

            // Connections layer 1+
            for (int i = 1; i < numberOfLayers; i++) {
                if (numberOfNeuronsInLayers[i] == 0)
                    continue;
                numberOfConnectionsToLayer = Random.Range(0, maxConnectionsToLayer + 1);
                for (int j = 0; j < numberOfConnectionsToLayer; j++) {
                    int toNeuron = Random.Range(0, numberOfNeuronsInLayers[i]);
                    int fromLayer = Random.Range(-1, i);
                    int fromNeuron;
                    if (fromLayer == -1) {
                        fromNeuron = Random.Range(0, numberOfInputs);
                    }
                    else {
                        fromNeuron = Random.Range(0, numberOfNeuronsInLayers[fromLayer]);
                    }
                    genes.Add(new Gene(fromLayer, fromNeuron, i, toNeuron, true, Random.Range(-1f, 1f)));
                }
            }

            // Connections layer output
            numberOfConnectionsToLayer = Random.Range(0, maxConnectionsToLayer + 1);
            for (int j = 0; j < numberOfConnectionsToLayer; j++) {
                int toNeuron = Random.Range(0, numberOfOutputs);
                int fromLayer = Random.Range(-1, numberOfLayers);
                int fromNeuron;
                if (fromLayer == -1) {
                    fromNeuron = Random.Range(0, numberOfInputs);
                }
                else {
                    fromNeuron = Random.Range(0, numberOfNeuronsInLayers[fromLayer]);
                }
                Debug.Log("Gene: { " + fromLayer + ", " + fromNeuron + " , { OUTPUT, " + toNeuron + " }");
                genes.Add(new Gene(fromLayer, fromNeuron, -2, toNeuron, true, Random.Range(-1f, 1f)));
            }

            return new HiddenLayer(neurons, genes);
            */
        }
    }
}