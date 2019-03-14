using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AreaConstants {
    public const int tileResolution = 64;
    public const int depth = 100;
    public const int width = 128;
    public const int height = 128;
}

public class TerrainCreator : MonoBehaviour {
    public TerrainLayer grass;

	void Start () {
        createGrasslands();
	}

    void createGrasslands() {
        TerrainData td = new TerrainData();
        td.size = new Vector3(AreaConstants.width, AreaConstants.depth, AreaConstants.height);
        td.heightmapResolution = AreaConstants.width + 1;

        td.terrainLayers = new TerrainLayer[] { grass };

        AreaData ad = new AreaData(AreaData.Type.hills, AreaConstants.tileResolution);

        createHeights(td, ad);

        Terrain.CreateTerrainGameObject(td);
    }

    void createHeights(TerrainData td, AreaData ad) {
        float[,] hm = new float[AreaConstants.width, AreaConstants.height];
        int tileSize = AreaConstants.width / AreaConstants.tileResolution;
        float scale = (1f / (float)AreaConstants.depth);

        for (int x = 0; x < AreaConstants.tileResolution; x++) {
            for (int y = 0; y < AreaConstants.tileResolution; y++) {
                setTile(hm, x * tileSize, y * tileSize, tileSize, scale * ad.heights[x, y]);
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

public class AreaData {

    public int[,] heights;

    public enum Type {
        hills
    }

    public AreaData(Type t, int resolution) {
        heights = new int[resolution, resolution];

        switch (t) {

            case Type.hills:
                createHills();
                break;

            default:
                break;
        }
    }

    void setZero() {
        for (int i = 0; i < heights.GetLength(0); i++) {
            for (int j = 0; j < heights.GetLength(1); j++) {
                heights[i, j] = 0;
            }
        }
    }

    void createHills() {
        const int sizeMin = 4;
        const int sizeMax = 9;
        const int countMin = 1;
        const int countMax = 4;

        setZero();

        createHill(6, 6, 16);



    }

    void createHill(int x, int y, int size) {
        for (int i = 0; i < heights.GetLength(0); i++) {
            for (int j = 0; j < heights.GetLength(1); j++) {
                float pointSize = (float)size - Mathf.Sqrt(Mathf.Pow((float)i - (float)x, 2) + Mathf.Pow((float)j - (float)y, 2));
                pointSize *= 0.75f;
                if (heights[i, j] < pointSize) {
                    heights[i, j] = Mathf.RoundToInt(pointSize);
                }
            }
        }
    }

    

}
