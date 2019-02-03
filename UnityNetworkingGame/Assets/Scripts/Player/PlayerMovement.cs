using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float speed;

    Player p;
    Rigidbody rb;

    private void Start() {
        p = GetComponent<Player>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        if (!p.networkObject.IsOwner) {
            transform.position = p.networkObject.position;
            return;
        }

        // Get the movement based on the axis input values
        Vector3 input = new Vector3(Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));
        if (input.magnitude > 1f) {
            input.Normalize();
        }

        Vector3 forward = transform.forward * input.x;
        Vector3 sideways = transform.right * input.z;

        rb.velocity = new Vector3((forward.x + sideways.x) * speed, rb.velocity.y, (forward.z + sideways.z) * speed);

        // Since we are the owner, tell the network the updated position
        p.networkObject.position = transform.position;
    }

}
