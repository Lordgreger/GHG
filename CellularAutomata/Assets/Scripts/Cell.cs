using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Cell : ScriptableObject {

    bool state;
    bool nextState;
    Vector3Int pos;
    Tilemap map;

    Tile white;
    Tile black;

    public void setupCell(bool s, Vector3Int p, Tilemap m) {
        state = s;
        nextState = false;
        pos = p;
        map = m;
    }

    public void Start() {
        //Debug.Log("Got here!");
        white = Resources.Load<Tile>("TileWhite");
        black = Resources.Load<Tile>("TileBlack");
        setTile();
    }

    public void Update() {
        //Debug.Log("Got here!");
        int value = getAmountOfNeighbours();
        //Debug.Log("Value is: " + value);
        nextState = getNewState(value);
        Debug.Log("Next state is: " + nextState);
    }

    public void LateUpdate() {
        state = nextState;
        setTile();
    }

    void setTile() {
        if (state) {
            map.SetTile(pos, black);
        }
        else {
            map.SetTile(pos, white);
        }
        map.RefreshTile(pos);
    }

    bool getNewState(int value) {

        // Game of life ruleset
        if (state) { // Cell is alive
            if (value > 3) { // Overpopulation
                Debug.Log("Overpop at : " + pos);
                return false;
            }
            if (value < 2) { // Loneliness
                Debug.Log("Lonely at : " + pos);
                return false;
            }
        }
        else {
            if (value == 3) { // Birth
                Debug.Log("birth at : " + pos);
                return true;
            }
        }
        return state;
    }

    int getAmountOfNeighbours() {
        int res = 0;
        for (int x = -1; x < 2; x++) {
            for (int y = -1; y < 2; y++) {
                
                if (getTileValue(new Vector3Int(pos.x + x, pos.y + y, pos.z)) == "1") {
                    if (!(x == 0 && y == 0))
                        res++;
                }
            }
        }
        return res;
    }

    int getValue() {
        string sVal =
            getTileValue(new Vector3Int(pos.x - 1, pos.y - 1, pos.z)) + getTileValue(new Vector3Int(pos.x, pos.y - 1, pos.z)) + getTileValue(new Vector3Int(pos.x + 1, pos.y - 1, pos.z)) +
            getTileValue(new Vector3Int(pos.x - 1, pos.y, pos.z)) + getTileValue(new Vector3Int(pos.x, pos.y, pos.z)) + getTileValue(new Vector3Int(pos.x + 1, pos.y, pos.z)) +
            getTileValue(new Vector3Int(pos.x - 1, pos.y + 1, pos.z)) + getTileValue(new Vector3Int(pos.x, pos.y + 1, pos.z)) + getTileValue(new Vector3Int(pos.x + 1, pos.y + 1, pos.z));

        return Convert.ToInt32(sVal, 2);
    }

    string getTileValue(Vector3Int pos) {
        if (map.GetTile(pos) != null) {
            //Debug.Log("Name of tile: " + map.GetTile(pos).name);
            if (map.GetTile(pos).name == "TileBlack") {
                return "1";
            }
            else {
                return "0";
            }
        }
        Debug.Log("No tile here!");
        return "0";
    }
}
