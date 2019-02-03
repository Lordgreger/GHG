using BeardedManStudios.Forge.Networking.Generated;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropable : DropableBehavior {

    private void Start() {
        InteractableEvent inter = GetComponent<InteractableEvent>();
        inter.interactEvent.AddListener(onInteraction);
        inter.msg = "Pickup dropable";
    }

    void onInteraction() {
        networkObject.Destroy();
    }

}
