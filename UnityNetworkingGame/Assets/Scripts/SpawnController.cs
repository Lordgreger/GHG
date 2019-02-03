using BeardedManStudios.Forge.Networking.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {
	void Start () {
        NetworkManager.Instance.InstantiatePlayer(0, transform.position, transform.rotation);
	}
}
