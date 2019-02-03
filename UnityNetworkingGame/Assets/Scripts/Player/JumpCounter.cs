using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCounter : MonoBehaviour {

    public int maxJumps;
    int jumps;

    private void Start() {
        jumps = maxJumps;
    }

    private void OnTriggerEnter(Collider other) {
        jumps = maxJumps;
    }

    public bool canJump() {
        if (jumps > 0) {
            return true;
        }
        return false;
    }

    public void useJump() {
        jumps--;
    }
}
