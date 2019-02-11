using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Behavior {
    void start();
    void update();
    void stop();
    bool evaluate();
}

public interface BehaviorController {
    void update();
}

public class BehaviorUser : MonoBehaviour {
    public BehaviorController behaviorController;
}

public class BehaviorControllerPriority : BehaviorController {
    Behavior current = null;
    public SortedList<int, Behavior> behaviors = new SortedList<int, Behavior>();
    public bool behaviorLocked = false;

    public BehaviorControllerPriority(SortedList<int, Behavior> behaviors) {
        this.behaviors = behaviors;
    }

    void chooseBehavior() {
        for (int i = 0; i < behaviors.Values.Count; i++) {
            if (behaviors.Values[i].evaluate()) {
                if (behaviors.Values[i] != current) {
                    if (current != null) {
                        current.stop();
                    }
                    current = behaviors.Values[i];
                    current.start();
                }
                return;
            }
        }
    }

    public void update() {
        if (!behaviorLocked) {
            chooseBehavior();
        }
        current.update();
    }
}

public class BehaviorControllerRandom : BehaviorController {

    Behavior current = null;
    public IList<Behavior> behaviors = new List<Behavior>();

    public void update() {
        Behavior b = behaviors[Random.Range(0, behaviors.Count)];
        b.start();
        b.update();
        b.stop();
    }
}

public class BehaviorControllerRandomFixedSteps : BehaviorController {
    int steps;
    int step = 0;
    Behavior current = null;
    public IList<Behavior> behaviors = new List<Behavior>();

    public BehaviorControllerRandomFixedSteps(int steps) { this.steps = steps; }

    public void update() {
        if (step == 0) {
            if (current != null)
                current.stop();
            step = steps;
            current = behaviors[Random.Range(0, behaviors.Count)];
            current.start();
        }
        if (current != null)
            current.update();
    }
}