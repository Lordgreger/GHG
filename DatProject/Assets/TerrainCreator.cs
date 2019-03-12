using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainCreator : MonoBehaviour {
    public TerrainLayer grass;

    const int depth = 100;
    const int width = 64;
    const int height = 64;
    const int tileResolution = 8;


	void Start () {
        createGrasslands();
	}

    void createGrasslands() {
        TerrainData td = new TerrainData();
        td.size = new Vector3(width, depth, height);
        td.heightmapResolution = width + 1;

        td.terrainLayers = new TerrainLayer[] { grass };
        createHeights(td);

        Terrain.CreateTerrainGameObject(td);
    }

    void createHeights(TerrainData td) {
        float[,] hm = new float[width, height];
        int tileSize = width / tileResolution;

        for (int x = 0; x < tileResolution; x++) {
            for (int y = 0; y < tileResolution; y++) {
                setTile(hm, x * tileSize, y * tileSize, tileSize, (1f / (float)depth) * Random.Range(0, 2));
            }
        }

        td.SetHeights(0, 0, hm);
    }

    void setTile(float[,] hm, int x, int y, int size, float value) {
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {                
                hm[x + i, y + j] = value;
            }
        }
    }

}
