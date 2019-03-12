using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : Interactable {
    public Transform dest;
    public override void interact(PlayerInteract player = null) {
        player.transform.SetPositionAndRotation(dest.position, dest.rotation);
    }

}
