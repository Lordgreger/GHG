using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BasicBehaviors;

public class SimpleMeleeAI : BehaviorUser {

    public float attackDelay;
    public float attackDistance;
    public float attackDamage;
    public ColliderContainer attackBox;
    public ColliderContainer attackTrigger;
    public MeshRenderer attackBoxMesh;

    private void Start() {
        SortedList<int, Behavior> behaviors = new SortedList<int, Behavior>();
        behaviors.Add(1, new Attack(this));
        behaviors.Add(2, new GoTowardsPlayer(this, 0.4f));
        behaviors.Add(3, new Idle());
        behaviorController = new BehaviorControllerPriority(behaviors);
    }

    private void Update() {
        behaviorController.update();
    }
    

    #region CustomBehaviors
    public class Attack : Behavior {
        SimpleMeleeAI user;
        bool canAttack;

        public Attack(SimpleMeleeAI user) {
            this.user = user;
        }

        public bool evaluate() {
            if (user.attackTrigger.getContainedObjects().Count > 0) {
                return true;
            }
            return false;
        }

        public void start() {
            canAttack = true;
            Debug.Log("Idle while charging attack");
        }

        public void stop() {
            Debug.Log("Done attacking");
        }

        public void update() {
            if (canAttack) {
                (user.behaviorController as BehaviorControllerPriority).behaviorLocked = true;
                user.StartCoroutine(AttackWithDelay(user.attackDelay));
                canAttack = false;
            }
        }

        IEnumerator AttackWithDelay(float delay) {
            yield return new WaitForSeconds(delay);
            attack();
            (user.behaviorController as BehaviorControllerPriority).behaviorLocked = false;
            canAttack = true;
        }

        void attack() {
            List<GameObject> hits = user.attackBox.getContainedObjects();
            foreach (GameObject g in hits) {
                g.GetComponent<PlayerStats>().damage(user.attackDamage);
            }
        }

    }

    #endregion

}
