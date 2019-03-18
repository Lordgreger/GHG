using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLooking : MonoBehaviour {

    public float speed;
    public Transform head;

    private void Update() {       
        transform.Rotate(transform.up, Input.GetAxis("Mouse X") * speed, Space.World);
        head.Rotate(head.right, -Input.GetAxis("Mouse Y") * speed, Space.World);
    }
}
