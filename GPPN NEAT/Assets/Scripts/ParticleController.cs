using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour {
    float maxSpeed;
    Vector2 vel = new Vector2();
    float traveledDistance = 0;

    Rigidbody rb;
    CPPN cppn;

    public void setup(CPPN icppn, float imaxSpeed) {
        rb = GetComponent<Rigidbody>();
        cppn = icppn;
        maxSpeed = imaxSpeed;
    }

    private void FixedUpdate() {
        float[] input = { vel[0], vel[1], traveledDistance };
        float[] result = cppn.setInputGetoutput(input);
        Vector2 res = new Vector2(result[0], result[1]);
        if (res.magnitude > maxSpeed)
            res *= maxSpeed / res.magnitude;
        vel[0] += Mathf.Clamp(res[0], -maxSpeed * Time.deltaTime, maxSpeed * Time.deltaTime);
        vel[1] += Mathf.Clamp(res[1], -maxSpeed * Time.deltaTime, maxSpeed * Time.deltaTime);
        traveledDistance += vel.magnitude;
        transform.Translate(vel[0], 0, vel[1], Space.Self);
    }
}
