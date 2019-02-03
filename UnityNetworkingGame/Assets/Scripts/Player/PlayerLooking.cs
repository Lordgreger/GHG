using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLooking : MonoBehaviour {

    public float speed;
    public Transform head;

    Player p;

    private void Start() {
        p = GetComponent<Player>();
    }

    private void Update() {
        if (!p.networkObject.IsOwner) {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, p.networkObject.rotation[0], transform.rotation.eulerAngles.z);
            head.rotation = Quaternion.Euler(p.networkObject.rotation[1], transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            return;
        }
        
        transform.Rotate(transform.up, Input.GetAxis("Mouse X") * speed, Space.World);
        head.Rotate(head.right, -Input.GetAxis("Mouse Y") * speed, Space.World);

        p.networkObject.rotation = new Vector2(transform.rotation.eulerAngles.y, head.rotation.eulerAngles.x);
    }


}
