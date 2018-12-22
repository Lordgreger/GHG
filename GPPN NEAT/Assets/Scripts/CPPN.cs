using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyMath {
    public static float Sigmoid(float value) {
        return 1.0f / (1.0f + Mathf.Exp(-value));
    }
}

public class CPPN {

    public CPPN() {
        inputNeurons = new List<InputNeuron>();
        outputNeurons = new List<Neuron>();
        hiddenNeurons = new List<Neuron>();
        genome = new List<Gene>();
    }

    public List<InputNeuron> inputNeurons;
    public List<Neuron> outputNeurons;
    public List<Neuron> hiddenNeurons;
    public List<Gene> genome;

    public List<float> setInputGetoutput(List<float> inputVals) {
        List<float> output = new List<float>();

        if (inputVals.Count != inputNeurons.Count) {
            throw new UnityException("Number of inputs not matching neurons");
        }

        for (int i = 0; i < outputNeurons.Count; i++) {
            output.Add(outputNeurons[i].ActivationFunction());
        }

        return output;
    }

    // Neuron = Node
    public class Neuron {
        public enum Type {
            input,
            sigmoid,
            gaussian
        }

        Type type;
        List<Gene> inputGenes;
        float calculatedVal;
        bool hasBeenCalculated;

        public virtual float ActivationFunction() {

            if (hasBeenCalculated)
                return calculatedVal;

            float val = 0.0f;
            foreach (var gene in inputGenes) {
                val += gene.from.ActivationFunction();
            }

            if (type == Type.sigmoid) {
                val = MyMath.Sigmoid(val);
            }

            calculatedVal = val;
            hasBeenCalculated = true;

            return val;
        }

        public Neuron(Type itype) {
            type = itype;
            calculatedVal = 0;
            hasBeenCalculated = false;
        }

        public Neuron(Type itype, List<Gene> igenes) {
            type = itype;
            inputGenes = igenes;
            calculatedVal = 0;
            hasBeenCalculated = false;
        }

        public Neuron() {
            type = Type.input;
            calculatedVal = 0;
            hasBeenCalculated = false;
        }

        public void setInputs(List<Gene> igenes) {
            inputGenes = igenes;
        }
    }

    public class InputNeuron : Neuron {
        public float val;

        public override float ActivationFunction() {
            return val;
        }

        public InputNeuron() {

        }
    }

    // Gene = Edge = Connection
    public class Gene {
        public int innovation;
        public Neuron from;
        public Neuron to;
        public bool enabled;
        public float weight;
    }

}