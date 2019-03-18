using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float speed;

    Rigidbody rb;

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        // Get the movement based on the axis input values
        Vector3 input = new Vector3(Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));
        if (input.magnitude > 1f) {
            input.Normalize();
        }

        Vector3 forward = transform.forward * input.x;
        Vector3 sideways = transform.right * input.z;

        rb.velocity = new Vector3((forward.x + sideways.x) * speed, rb.velocity.y, (forward.z + sideways.z) * speed);
    }

}
