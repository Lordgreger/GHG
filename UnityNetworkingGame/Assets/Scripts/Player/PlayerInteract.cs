using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour {
    public float dist;
    public Transform cam;
    public Text interactionText;

    Interactable interactable;

    private void Start() {
        interactable = null;
    }

    private void Update() {
        updateInteractable();
        checkInput();
    }

    void updateInteractable() {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, dist)) {
            if (hit.collider.tag == "Interactable") {
                interactable = hit.transform.GetComponent<Interactable>();
                interactionText.text = "[E] " + interactable.msg;
                return;
            }
        }
        interactionText.text = "";
        interactable = null;
    }

    void checkInput() {
        if (Input.GetKeyDown(KeyCode.E)) {
            if (interactable != null)
                interactable.interact(this);
        }
    }

}
