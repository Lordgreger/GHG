using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumping : MonoBehaviour {

    public float power;

    Rigidbody rb;
    JumpCounter jc;

    private void Start() {
        if (!GetComponent<Player>().networkObject.IsOwner) {
            this.enabled = false;
            return;
        }
        rb = GetComponent<Rigidbody>();
        jc = GetComponentInChildren<JumpCounter>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (jc.canJump()) {
                jc.useJump();
                rb.AddForce(transform.up * power, ForceMode.Impulse);
            }
        }
    }

}
