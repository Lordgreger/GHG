using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackBox : MonoBehaviour {

    List<Player> players = new List<Player>();

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            Player p = other.GetComponent<Player>();
            if (p == null) {
                return;
            }

            if (!players.Contains(p)) {
                players.Add(p);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            Player p = other.GetComponent<Player>();
            if (p == null) {
                return;
            }

            if (players.Contains(p)) {
                players.Remove(p);
            }
        }
    }

    public List<Player> getHits() {
        return players;
    }

}
