using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Unity;

public class InteractableTest : Interactable {
    public override void interact() {
        Debug.Log("Interacted with test!");
        NetworkManager.Instance.InstantiateDropable(0, transform.position + transform.up);
    }
}
