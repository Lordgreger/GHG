using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyMath {
    public static bool randomBool() {
        if (Random.Range(0, 2) == 0)
            return false;
        return true;
    }

    public static float Sigmoid(float value) {
        return 1.0f / (1.0f + Mathf.Exp(-value));
    }

    public static float Guassian(float value) {
        return (1.0f / Mathf.Sqrt(2.0f * Mathf.PI)) * Mathf.Exp(Mathf.Pow(-value, 2) / 2);
    }

    public static float Step(float value, float thresh) {
        if (value > thresh)
            return 1;
        return 0;
    }
}

public static class CPPNLib {
    public static CPPN.Genome Crossover(CPPN.Genome p0, CPPN.Genome p1) {
        CPPN.Genome outputGenome = new CPPN.Genome();

        // Combine neurons
        //Debug.Log("Keys1: " + p0.neuronGenes.Keys.Count);
        foreach (var n in p0.neuronGenes.Keys) {
            //Debug.Log(" - " + n);
            if (p1.neuronGenes.ContainsKey(n)) { // Contained in both
                if (MyMath.randomBool()) {
                    outputGenome.neuronGenes.Add(n, p0.neuronGenes[n]);
                }
                else {
                    outputGenome.neuronGenes.Add(n, p1.neuronGenes[n]);
                }
            }
            else {
                outputGenome.neuronGenes.Add(n, p0.neuronGenes[n]); // Unique for p0
            }
        }
        //Debug.Log("Keys2: " + p1.neuronGenes.Keys.Count);
        foreach (var n in p1.neuronGenes.Keys) {
            //Debug.Log(" - " + n);
            if (!outputGenome.neuronGenes.ContainsKey(n)) {
                outputGenome.neuronGenes.Add(n, p1.neuronGenes[n]); // Unique for p1
            }
        }

        // Combine connections
        foreach (var n in p0.connectionGenes.Keys) {
            if (p1.connectionGenes.ContainsKey(n)) { // Contained in both
                if (MyMath.randomBool()) {
                    outputGenome.connectionGenes.Add(n, p0.connectionGenes[n]);
                }
                else {
                    outputGenome.connectionGenes.Add(n, p1.connectionGenes[n]);
                }
            }
            else {
                outputGenome.connectionGenes.Add(n, p0.connectionGenes[n]); // Unique for p0
            }
        }
        foreach (var n in p1.neuronGenes.Keys) {
            if (!outputGenome.neuronGenes.ContainsKey(n)) {
                outputGenome.connectionGenes.Add(n, p1.connectionGenes[n]); // Unique for p1
            }
        }

        return outputGenome;
    }
}

public class InnovationAssigner {
    int innovationCounter = 0;
    public InnovationAssigner(int input, int output) {
        innovationCounter = input + output;
    }
    public int getInnovation() {
        return innovationCounter++;
    }
}

/*** NOTES ***
    Iteration 1:
    CPPN - done

    Iteration 2:
    Crossover - done

    Iteration 3:
    Mutation - modify neuron
    Mutation - add neuron - done
    Mutation - modify connection - done
    Mutation - add connection - done

    Iteration 4:
    Fitness for selecting good CPPNs
*/

public class CPPN {

    public static int inputLayer = -1;
    public static int outputLayer = int.MaxValue;

    InnovationAssigner neuronIA;
    InnovationAssigner connectionIA;

    IDictionary<int, Neuron> neurons = new Dictionary<int, Neuron>();
    int[] inputs;
    int[] outputs;

    public Genome genome;

    public CPPN(int numberOfInputs, int numberOfOutputs, InnovationAssigner nIA, InnovationAssigner cIA) {
        neuronIA = nIA;
        connectionIA = cIA;
        generateRandom(numberOfInputs, numberOfOutputs);
    }

    public CPPN(Genome genome, InnovationAssigner nIA, InnovationAssigner cIA) {
        this.genome = genome;
        neuronIA = nIA;
        connectionIA = cIA;
        createFromGenome();
    }

    void createFromGenome() {
        List<int> inputsL = new List<int>();
        List<int> outputsL = new List<int>();
        foreach (var gn in genome.neuronGenes.Values) {
            Neuron neuron = new Neuron(gn.type, gn.layer, gn.innovation);
            neurons[gn.innovation] = neuron;
            if (neuron.layer == inputLayer) {
                inputsL.Add(neuron.innovation);
            }
            else if (neuron.layer == outputLayer) {
                outputsL.Add(neuron.innovation);
            }
        }
        inputs = inputsL.ToArray();
        outputs = outputsL.ToArray();
        //Debug.Log("inputs: " + inputs.Length);
        // Create connections from genome
        foreach (var gc in genome.connectionGenes.Values) {
            neurons[gc.toNeuronInnovation].inputs.Add(new NeuronInput(neurons[gc.fromNeuronInnovation], gc.weight));
        }
    }

    void generateRandom(int numberOfInputs, int numberOfOutputs) {

        #region genome generation
        // Generate random Genome
        genome = new Genome();
        //-- Generate Neuron part (include input and output)
        IDictionary<int, NeuronGene> neuronGenes = new Dictionary<int, NeuronGene>();
        //---- Outputs
        for (int i = 0; i < numberOfOutputs; i++) {
            NeuronGene ng = new NeuronGene(outputLayer, (Neuron.Type)Random.Range(0, (int)Neuron.Type.amountOfTypes), i, false, true);
            neuronGenes.Add(ng.innovation, ng);
        }
        //---- Inputs 
        for (int i = 0; i < numberOfInputs; i++) {
            NeuronGene ng = new NeuronGene(inputLayer, Neuron.Type.input, i + numberOfOutputs, true, false);
            neuronGenes.Add(ng.innovation, ng);
        }
        //---- Generate hidden 0
        int nRandoms = Random.Range(2, 5);
        for (int i = 0; i < nRandoms; i++) {
            NeuronGene ng = new NeuronGene(0, (Neuron.Type)Random.Range(0, (int)Neuron.Type.amountOfTypes), neuronIA.getInnovation(), false, false);
            neuronGenes.Add(ng.innovation, ng);
        }
        genome.neuronGenes = neuronGenes;

        //-- Generate Connection part
        IDictionary<int, ConnectionGene> connectionGenes = new Dictionary<int, ConnectionGene>();
        List<int> hasInput = new List<int>();
        List<int> hasOutput = new List<int>();
        foreach (var n in genome.neuronGenes) {
            if (n.Value.layer != inputLayer) {
                if (!hasInput.Contains(n.Value.innovation)) {
                    int[] candidates = getInputCandidates(n.Value.layer);
                    //Debug.Log(candidates.Length);
                    int chosenCandidate = candidates[Random.Range(0, candidates.Length)];
                    ConnectionGene cg = new ConnectionGene(connectionIA.getInnovation(), chosenCandidate, n.Value.innovation, Random.Range(-1f, 1f));
                    connectionGenes.Add(cg.innovation, cg);
                    hasInput.Add(n.Value.innovation);
                    hasOutput.Add(chosenCandidate);
                    //Debug.Log("Added input");
                }
            }

            if (n.Value.layer != outputLayer) {
                if (!hasOutput.Contains(n.Value.innovation)) {
                    int[] candidates = getOutputCandidates(n.Value.layer);
                    int chosenCandidate = candidates[Random.Range(0, candidates.Length)];
                    ConnectionGene cg = new ConnectionGene(connectionIA.getInnovation(), n.Value.innovation, chosenCandidate, Random.Range(-1f, 1f));
                    connectionGenes.Add(cg.innovation, cg);
                    hasOutput.Add(n.Value.innovation);
                    hasInput.Add(chosenCandidate);
                    //Debug.Log("Added output");
                }
            }
        }
        genome.connectionGenes = connectionGenes;
        //printGenome();
        #endregion

        #region creation from genome
        // Create neurons from Genome
        inputs = new int[numberOfInputs];
        outputs = new int[numberOfOutputs];
        int ii = 0, io = 0;
        foreach (var gn in genome.neuronGenes.Values) {
            Neuron neuron = new Neuron(gn.type, gn.layer, gn.innovation);
            neurons[gn.innovation] = neuron;
            if (neuron.layer == inputLayer) {
                inputs[ii] = neuron.innovation;
            }
            else if (neuron.layer == outputLayer) {
                outputs[io] = neuron.innovation;
            }
        }
        //Debug.Log("inputs: " + inputs.Length);
        // Create connections from genome
        foreach (var gc in genome.connectionGenes.Values) {
            neurons[gc.toNeuronInnovation].inputs.Add(new NeuronInput(neurons[gc.fromNeuronInnovation], gc.weight));
        }
        #endregion
    }

    public float[] setInputGetoutput(float[] inputVals) {
        if (inputs.Length != inputVals.Length) {
            Debug.Log("Input mismatch");
            return null;
        }

        // Insert input
        for (int i = 0; i < inputVals.Length; i++) {
            neurons[inputs[i]].setVal(inputVals[i]);
            //Debug.Log("Added input value " + inputVals[i]);
        }

        float[] output = new float[outputs.Length];
        for (int i = 0; i < outputs.Length; i++) {
            //Debug.Log("Got Output " + n.getVal());
            output[i] = neurons[outputs[i]].getVal();
        }
        return output;
    }

    // Misc
    int[] getInputCandidates(int layer) {
        List<int> candidates = new List<int>();
        foreach (var n in genome.neuronGenes) {
            if (n.Value.layer < layer) {
                candidates.Add(n.Key);
            }
        }
        return candidates.ToArray();
    }

    int[] getOutputCandidates(int layer) {
        List<int> candidates = new List<int>();
        foreach (var n in genome.neuronGenes) {
            if (n.Value.layer > layer) {
                candidates.Add(n.Key);
            }
        }
        return candidates.ToArray();
    }

    public void printGenome() {
        string output = "Nodes: " + genome.neuronGenes.Count + " \n";
        foreach (var n in genome.neuronGenes) {
            output += " - inno: " + n.Value.innovation + " layer: " + n.Value.layer + " type: " + n.Value.type + "\n";
        }
        Debug.Log(output);
    }

    #region Classes and Structs
    public class Neuron {
        public enum Type {
            output = -2,
            input = -1,
            tanh,
            cos,
            sine,
            step,
            amountOfTypes
        }

        public IList<NeuronInput> inputs = new List<NeuronInput>();
        public Type type;
        public int layer;
        public int innovation;
        float inputVal;

        public Neuron(Type itype, int ilayer, int iinnovation) {
            type = itype;
            layer = ilayer;
            inputVal = 0;
            innovation = iinnovation;
        }

        public void setVal(float val) {
            inputVal = val;
        }

        public float getVal() {

            if (type != Type.input) {
                inputVal = 0;
                //Debug.Log("number of inputs: " + inputs.Count);
                foreach (var n in inputs) {
                    //Debug.Log("unweighted val, weight " + n.neuron.getVal() + ", " + n.weight);
                    inputVal += n.neuron.getVal() * n.weight;
                }
                //Debug.Log("Input val " + inputVal);
            }
            //Debug.Log("Input type, val " + type + ", " + inputVal);

            switch (type) {

                case Type.input:
                    return inputVal;

                case Type.output:
                    return inputVal;

                case Type.tanh:
                    return (float)System.Math.Tanh(inputVal);

                case Type.cos:
                    return (float)System.Math.Cos(inputVal);

                case Type.sine:
                    return Mathf.Sin(inputVal);

                case Type.step:
                    return MyMath.Step(inputVal, 0f);

                default:
                    return inputVal;
            }
        }
    }

    public struct NeuronInput {
        public Neuron neuron;
        public float weight;

        public NeuronInput(Neuron neuron, float weight) {
            this.neuron = neuron;
            this.weight = weight;
        }
    }

    public class Genome {
        public IDictionary<int, NeuronGene> neuronGenes;
        public IDictionary<int, ConnectionGene> connectionGenes;

        public Genome() {
            neuronGenes = new Dictionary<int, NeuronGene>();
            connectionGenes = new Dictionary<int, ConnectionGene>();
        }

        public void mutate(InnovationAssigner nIA, InnovationAssigner cIA) {
            mutationModifyConnection(0.8f);
            mutationAddConnection(0.05f, cIA);
            mutationAddNeuron(0.03f, nIA, cIA);
        }

        void mutationModifyConnection(float mutationChance) {
            foreach (var c in connectionGenes) {
                if (Random.Range(0f, 1f) <= mutationChance) {
                    if (Random.Range(0f, 1f) <= 0.9f) {
                        float newWeight;
                        if (MyMath.randomBool())
                            newWeight = c.Value.weight + 0.05f;
                        else
                            newWeight = c.Value.weight - 0.05f;

                        Debug.Log("Mutated weight from " + c.Value.weight + " to " + newWeight);
                        c.Value.setWeight(newWeight);
                    }
                    else {
                        float newWeight = Random.Range(-1f, 1f);
                        Debug.Log("New random weight from " + c.Value.weight + " to " + newWeight);
                        c.Value.setWeight(newWeight);
                    }
                }
            }
        }

        void mutationAddConnection(float mutationChance, InnovationAssigner cIA) {
            foreach (var n in neuronGenes) {
                if (!n.Value.isInput) {
                    if (Random.Range(0f, 1f) <= mutationChance) {
                        int[] candidates = getInputCandidates(n.Value.layer);
                        int chosen = candidates[Random.Range(0, candidates.Length)];
                        float weight = Random.Range(-1f, 1f);
                        ConnectionGene newConnection = new ConnectionGene(cIA.getInnovation(), chosen, n.Value.innovation, weight);
                        connectionGenes.Add(newConnection.innovation, newConnection);
                        Debug.Log("New connection from " + newConnection.fromNeuronInnovation + " to " + newConnection.toNeuronInnovation);
                    }
                }
            }
        }

        void mutationAddNeuron(float mutationChance, InnovationAssigner nIA, InnovationAssigner cIA) {
            IList<NeuronGene> ngs = new List<NeuronGene>();
            IList<ConnectionGene> cgs = new List<ConnectionGene>();
            foreach (var c in connectionGenes) {
                if (Random.Range(0f, 1f) <= mutationChance) {
                    // New neuron
                    NeuronGene ng = new NeuronGene(neuronGenes[c.Value.fromNeuronInnovation].layer + 1, (Neuron.Type)Random.Range(0, (int)Neuron.Type.amountOfTypes), nIA.getInnovation(), false, false);
                    Debug.Log("New neuron: " + ng.innovation + " layer " + ng.layer);

                    // Create connection from new to old target neuron
                    float weight = Random.Range(-1f, 1f);
                    ConnectionGene cg = new ConnectionGene(cIA.getInnovation(), ng.innovation, c.Value.toNeuronInnovation, weight);
                    Debug.Log("New connection from " + cg.fromNeuronInnovation + " to " + cg.toNeuronInnovation);

                    // Move connection to new neuron
                    c.Value.setTarget(ng.innovation);

                    ngs.Add(ng);
                    cgs.Add(cg);
                }
            }
            foreach (var n in ngs) {
                neuronGenes.Add(n.innovation, n);
            }
            foreach (var c in cgs) {
                connectionGenes.Add(c.innovation, c);
            }
        }

        int[] getInputCandidates(int layer) {
            List<int> candidates = new List<int>();
            foreach (var n in neuronGenes) {
                if (n.Value.layer < layer) {
                    candidates.Add(n.Key);
                }
            }
            return candidates.ToArray();
        }
    }

    public struct NeuronGene {
        public int innovation;
        public int layer;
        public Neuron.Type type;
        public bool isInput, isOutput;

        public NeuronGene(int layer, Neuron.Type type, int innovation, bool isInput, bool isOutput) {
            this.innovation = innovation;
            this.layer = layer;
            this.type = type;
            this.isInput = isInput;
            this.isOutput = isOutput;
        }
    }

    public struct ConnectionGene {
        public int innovation;
        public int fromNeuronInnovation;
        public int toNeuronInnovation;
        public float weight;

        public ConnectionGene(int innovation, int fromNeuronInnovation, int toNeuronInnovation, float weight) {
            this.innovation = innovation;
            this.fromNeuronInnovation = fromNeuronInnovation;
            this.toNeuronInnovation = toNeuronInnovation;
            this.weight = weight;
        }

        public void setWeight(float iweight) {
            weight = iweight;
        }

        public void setTarget(int itarget) {
            toNeuronInnovation = itarget;
        }
    }
    #endregion
}
