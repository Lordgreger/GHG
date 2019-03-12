using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableEvent : Interactable {
    public UnityEvent interactEvent = new UnityEvent();

    public override void interact(PlayerInteract player = null) {
        interactEvent.Invoke();
    }
}
