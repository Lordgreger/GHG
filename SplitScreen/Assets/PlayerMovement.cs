using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float speed;
    public string xAxis;
    public string yAxis;
    Rigidbody rb;

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        Vector2 input = new Vector2(Input.GetAxis(xAxis), Input.GetAxis(yAxis));
        
        if (input.magnitude > 1f) {
            input.Normalize();
        }

        Vector3 newVelocity = new Vector3(input.x * speed, rb.velocity.y, input.y * speed);

        rb.velocity = newVelocity;
    }


}
