using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITest : MonoBehaviour {

    BehaviorController ai = new BehaviorControllerRandom();

	void Start () {
        (ai as BehaviorControllerRandom).behaviors = new List<Behavior> { new GoUp(), new GoDown(), new GoLeft(), new GoRight() };
	}
	
	void Update () {
        ai.update();
	}

    #region Behaviors
    public class GoUp : Behavior {
        public bool evaluate() {
            return true;
        }

        public void start() {
            
        }

        public void stop() {
            
        }

        public void update() {
            print("Going up");
        }
    }

    public class GoDown : Behavior {
        public bool evaluate() {
            return true;
        }

        public void start() {
            
        }

        public void stop() {
            
        }

        public void update() {
            print("Going down");
        }
    }

    public class GoLeft : Behavior {
        public bool evaluate() {
            return true;
        }

        public void start() {
            
        }

        public void stop() {
            
        }

        public void update() {
            print("Going Left");
        }
    }

    public class GoRight : Behavior {
        public bool evaluate() {
            return true;
        }

        public void start() {
            
        }

        public void stop() {
            
        }

        public void update() {
            print("Going right");
        }
    }
    #endregion
}
