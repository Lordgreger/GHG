using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Lib  {

    public static float[,] bloatArray(float[,] arr, int bloatingFactor) {
        float[,] output = new float[arr.GetLength(0) * bloatingFactor, arr.GetLength(1) * bloatingFactor];

        for (int i = 0; i < arr.GetLength(0); i++) {
            for (int j = 0; j < arr.GetLength(1); j++) {

                for (int bi = 0; bi < bloatingFactor; bi++) {
                    for (int bj = 0; bj < bloatingFactor; bj++) {
                        output[i * bloatingFactor + bi, j * bloatingFactor + bj] = arr[i, j];
                    }
                }

            }
        }

        return output;
    }

    public static string hmToString(float[,] hm) {
        string output = "";
        for (int i = 0; i < hm.GetLength(0); i++) {
            for (int j = 0; j < hm.GetLength(1) - 1; j++) {
                output += hm[i, j] + ", ";
            }
            output += hm[i, hm.GetLength(1) - 1] + ";\n"; 
        }
        return output;
    }

}
