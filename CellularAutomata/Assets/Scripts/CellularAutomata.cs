using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CellularAutomata : MonoBehaviour {

    public Tilemap map;
    public int size;

    List<Cell> cells = new List<Cell>();

    private void Start() {
        Tile white = Resources.Load<Tile>("TileWhite");
        for (int x = -(size + 1); x < (size + 1); x++) {
            for (int y = -(size + 1); y < (size + 1); y++) {
                map.SetTile(new Vector3Int(x,y,0),white);
            }
        }

        for (int x = -size; x < size; x++) {
            for (int y = -size; y < size; y++) {
                Cell c = ScriptableObject.CreateInstance<Cell>();
                c.setupCell(Random.value >= 0.5, new Vector3Int(x, y, 0), map);
                cells.Add(c);
            }
        }

        // Run cell like unity monobehavior
        foreach (Cell c in cells) {
            c.Start();
        }
    }

    private void FixedUpdate() {
        // Run cell like unity monobehavior
        foreach (Cell c in cells) {
            c.Update();
        }
    }

    private void LateUpdate() {
        // Run cell like unity monobehavior
        foreach (Cell c in cells) {
            c.LateUpdate();
        }
    }


}
