using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour {
    #region Constants
    const int terrainsX = 1;
    const int terrainsY = 1;
    const int terrainSizeInTiles = 8; // Terrains are square
    const int tileSize = 4;
    const int terrainDepth = 32;
    const int tileResolution = 8; // Amount of points for the tile in the heightmap

    const int terrainSize = terrainSizeInTiles * tileSize;
    const int heightmapResolution = terrainSizeInTiles * tileResolution;
    const int areaSizeX = terrainSizeInTiles * terrainsX;
    const int areaSizeY = terrainSizeInTiles * terrainsY;
    #endregion

    public Material grass;

    AreaData ad;

    private void Start() {
        ad = new AreaData(areaSizeX, areaSizeY, terrainDepth);
        float[,] hm = ad.getSubArea(0, 0, terrainSizeInTiles, terrainSizeInTiles);
        print(Lib.hmToString(hm));
        hm = Lib.bloatArray(hm, tileResolution);

        createTerrain(hm, heightmapResolution, terrainSize, terrainDepth);
        


    }

    void createTerrain(float[,] hm, int hmResolution, int size, int depth) {
        TerrainData td = new TerrainData();
        td.heightmapResolution = hmResolution;
        td.size = new Vector3(size, depth, size);
        td.SetHeights(0, 0, hm);

        Terrain t = Terrain.CreateTerrainGameObject(td).GetComponent<Terrain>();
        t.materialType = Terrain.MaterialType.Custom;
        t.materialTemplate = grass;
    }
}

public class AreaData {
    float[,] heights;

    public AreaData(int x, int y, int terrainDepth) {
        heights = new float[x, y];
        setZero();
        createHills();
        
    }

    public float[,] getSubArea(int x, int y, int xs, int ys) {
        float[,] output = new float[xs, ys];
        for (int i = 0; i < xs; i++) {
            for (int j = 0; j < ys; j++) {
                output[i, j] = heights[x + i, y + j];
            }
        }
        return output;
    }

    void setRandomHeight() {
        for (int i = 0; i < heights.GetLength(0); i++) {
            for (int j = 0; j < heights.GetLength(1); j++) {
                heights[i, j] = Random.Range(0f, 0.5f);
            }
        }
    }

    void setZero() {
        for (int i = 0; i < heights.GetLength(0); i++) {
            for (int j = 0; j < heights.GetLength(1); j++) {
                heights[i, j] = 0f;
            }
        }
    }

    void createHills() {
        float sizeMin = 0.02f;
        float sizeMax = 0.1f;
        const int countMin = 3;
        const int countMax = 6;
        float steepnessMin = 0.002f;
        float steepnessMax = 0.02f;

        int count = Random.Range(countMin, countMax + 1);

        for (int i = 0; i < count; i++) {
            int x = Random.Range(0, heights.GetLength(0));
            int y = Random.Range(0, heights.GetLength(1));
            float size = Random.Range(sizeMin, sizeMax);
            float steepness = Random.Range(steepnessMin, steepnessMax);

            createHill(x, y, size, steepness);
        }
    }

    void createHill(int x, int y, float size, float steepness) {
        for (int i = 0; i < heights.GetLength(0); i++) {
            for (int j = 0; j < heights.GetLength(1); j++) {
                float pointSize = size - (Mathf.Sqrt(Mathf.Pow(((float)i - (float)x) * (1/32f), 2) + Mathf.Pow(((float)j - (float)y) * (1 / 32f), 2)));
                MonoBehaviour.print(pointSize);
                //pointSize *= steepness;
                if (heights[i, j] < pointSize) {
                    heights[i, j] = pointSize;
                }
            }
        }
    }
}
