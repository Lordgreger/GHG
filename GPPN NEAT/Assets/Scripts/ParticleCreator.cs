using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCreator : MonoBehaviour {
    public float maxSpeed;
    public GameObject particlePrefab;
    InnovationAssigner neuronIA = new InnovationAssigner(3, 2);
    InnovationAssigner connectionIA = new InnovationAssigner(3, 2);

    private void Start() {
        init();
        CPPN winner = createRandomAndGetWinner(100, 1000);
        spawnWinner(winner);
        StartCoroutine(autoSpawner(winner));
    }

    IEnumerator autoSpawner(CPPN cppn) {
        CPPN winner = cppn;
        while (true) {
            yield return new WaitForSeconds(2);
            winner = createFromWinnerAndGetWinner(winner, 100, 1000);
            spawnWinner(winner);
        }
    }

    private void init() {
        int seed = Random.Range(0, 10000);
        Random.InitState(seed);
    }

    private void spawnWinner(CPPN cppn) {
        ParticleController pc = Instantiate(particlePrefab).GetComponent<ParticleController>();
        pc.setup(cppn, maxSpeed);
    }

    private CPPN createRandomAndGetWinner(int amount, int evaluationIterations) {
        CPPN winner = null;
        float winnerScore = 0;
        for (int i = 0; i < amount; i++) {
            CPPN cppn = new CPPN(3, 2, neuronIA, connectionIA);
            float score = fitness(cppn, evaluationIterations);
            if (score > winnerScore) {
                winner = cppn;
                winnerScore = score;
            }
        }
        Debug.Log("Won with " + winnerScore);
        return winner;
    }

    private CPPN createFromWinnerAndGetWinner(CPPN cppn, int amount, int evaluationIterations) {
        CPPN winner = cppn;
        float winnerScore = fitness(cppn, evaluationIterations);
        for (int i = 0; i < amount; i++) {
            CPPN.Genome genome = cppn.genome.copy();
            genome.mutate(neuronIA, connectionIA);
            CPPN newCppn = new CPPN(genome, neuronIA, connectionIA);
            float score = fitness(newCppn, evaluationIterations);
            if (score > winnerScore) {
                winner = newCppn;
                winnerScore = score;
            }
        }
        Debug.Log("Won with " + winnerScore);
        return winner;
    }

    private float fitness(CPPN cppn, int evaluationIterations) {
        Vector2 vel = new Vector2();
        Vector2 prevVel = new Vector2();
        Vector2 pos = new Vector2();
        float traveledDistance = 0;

        for (int i = 0; i < evaluationIterations; i++) {
            prevVel = new Vector2(vel.x, vel.y);
            float[] input = { vel[0], vel[1], traveledDistance };
            float[] result = cppn.setInputGetoutput(input);
            Vector2 res = new Vector2(result[0], result[1]);
            if (res.magnitude > maxSpeed)
                res *= maxSpeed / res.magnitude;
            vel[0] += Mathf.Clamp(res[0], -maxSpeed * Time.deltaTime, maxSpeed * Time.deltaTime);
            vel[1] += Mathf.Clamp(res[1], -maxSpeed * Time.deltaTime, maxSpeed * Time.deltaTime);
            traveledDistance += vel.magnitude;
            pos += vel;
        }

        return (traveledDistance / (pos.magnitude * 0.5f));
    }
}
