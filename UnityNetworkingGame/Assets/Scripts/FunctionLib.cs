using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FunctionLib {
    public static float distanceTo(Vector3 l, Vector3 r) {
        return (l - r).magnitude;
    }
}
