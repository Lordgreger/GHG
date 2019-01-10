using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCPPNControler : MonoBehaviour {

    public float maxSpeed;
    Vector2 vel = new Vector2();
    float traveledDistance = 0;

    // 1844

    Rigidbody rb;

    CPPN cppn;
    InnovationAssigner neuronIA = new InnovationAssigner(3, 2);
    InnovationAssigner connectionIA = new InnovationAssigner(3, 2);

    private void Start() {
        int seed = Random.Range(0, 10000);
        Random.InitState(seed);
        Debug.Log("Seed: " + seed);
        CPPN cppn1 = new CPPN(3, 2, neuronIA, connectionIA);
        cppn1.printGenome();
        CPPN cppn2 = new CPPN(3, 2, neuronIA, connectionIA);
        cppn2.printGenome();
        CPPN.Genome genome = CPPNLib.Crossover(cppn1.genome, cppn2.genome);
        genome.mutate(neuronIA, connectionIA);
        cppn = new CPPN(genome, neuronIA, connectionIA);
        cppn.printGenome();
        Debug.Log("fitness: " + Fitness(cppn, 100));
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        float[] input = { vel[0], vel[1], traveledDistance };
        float[] result = cppn.setInputGetoutput(input);
        vel[0] = Mathf.Clamp(result[0], -maxSpeed * Time.deltaTime, maxSpeed * Time.deltaTime);
        vel[1] = Mathf.Clamp(result[1], -maxSpeed * Time.deltaTime, maxSpeed * Time.deltaTime);
        traveledDistance += vel.magnitude;
        transform.Translate(vel[0], 0, vel[1], Space.Self);
    }

    public float Fitness(CPPN cppn, int evaluationIterations) {
        //float output = 0;
        Vector2 vel = new Vector2();
        Vector2 pos = new Vector2();
        float traveledDistance = 0;

        for (int i = 0; i < evaluationIterations; i++) {
            float[] input = { vel[0], vel[1], traveledDistance };
            float[] result = cppn.setInputGetoutput(input);
            vel[0] = Mathf.Clamp(result[0], -maxSpeed * Time.deltaTime, maxSpeed * Time.deltaTime);
            vel[1] = Mathf.Clamp(result[1], -maxSpeed * Time.deltaTime, maxSpeed * Time.deltaTime);
            traveledDistance += vel.magnitude;
            pos += vel;
        }

        return pos.magnitude + traveledDistance;
    }
}
