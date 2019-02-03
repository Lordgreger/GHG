using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLock : MonoBehaviour {

    PlayerLooking pl;
    PlayerJumping pj;
    PlayerMovement pm;
    PlayerInteract pi;

    bool locked;

    private void Start() {
        if (!GetComponent<Player>().networkObject.IsOwner) {
            this.enabled = false;
            return;
        }
        pl = GetComponent<PlayerLooking>();
        pj = GetComponent<PlayerJumping>();
        pm = GetComponent<PlayerMovement>();
        pi = GetComponent<PlayerInteract>();
        locked = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.LeftAlt)) {
            locked = !locked;
            setLock(locked);
        }
    }

    void setLock(bool state) {
        if (state) {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else {
            Cursor.lockState = CursorLockMode.None;
        }
        pl.enabled = state;
        pj.enabled = state;
        pm.enabled = state;
        pi.enabled = state;
    }

}
