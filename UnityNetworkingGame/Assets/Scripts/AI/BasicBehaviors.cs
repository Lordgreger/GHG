using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BasicBehaviors {
    public class GoTowardsPlayer : Behavior {
        BehaviorUser user;
        NavMeshAgent nav;
        GameObject player = null;
        float maxDistance;

        public GoTowardsPlayer(BehaviorUser user, float maxDistance) {
            this.user = user;
            this.maxDistance = maxDistance;
            nav = user.GetComponent<NavMeshAgent>();
        }

        float distanceToPlayer() {
            return (user.transform.position - player.transform.position).magnitude;
        }

        void updatePlayer() {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        public bool evaluate() {
            updatePlayer();
            if (player == null) {
                return false;
            }

            if (distanceToPlayer() > maxDistance) {
                return true;
            }
            return false;
        }

        public void start() {
            MonoBehaviour.print("Started moving towards player");
            nav.isStopped = false;
        }

        public void stop() {
            MonoBehaviour.print("Stopped moving towards player");
            nav.SetDestination(user.transform.position);
            nav.isStopped = true;
        }

        public void update() {
            updatePlayer();
            if (player == null) {
                Debug.LogError("No player to follow! (Should not be possible)");
                return;
            }
            nav.SetDestination(player.transform.position);
        }
    }

    public class Idle : Behavior {
        public bool evaluate() {
            return true;
        }

        public void start() {
            MonoBehaviour.print("Started doing nothing");
        }

        public void stop() {
            MonoBehaviour.print("Stopped doing nothing");
        }

        public void update() {

        }
    }


}
