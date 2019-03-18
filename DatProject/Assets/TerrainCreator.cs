using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public static class AreaConstants {

    // Terrain modifiers
    public const int depth = 16;
    public const int size = 32;
    public const int tileResolution = 4;
    public const int tileSize = 2;
    public const float heightScale = 0.1f;

    // Area size by terrain amount
    public const int sizeTerrainsX = 2; // amount of terrains in x
    public const int sizeTerrainsY = 2; // amount of terrains in y

    // Calculated values
    public const int resolution = size * tileResolution;
    public const int tileTotalResolutionSize = tileResolution * tileSize;
    public const int areaSize = size / tileResolution;
}

public class TerrainCreator : MonoBehaviour {
    public Material grass;

	void Start () {
        //createGrasslands();
        createArea();
	}

    void createGrasslands() {
        TerrainData td = new TerrainData();
        td.heightmapResolution = AreaConstants.resolution;
        td.size = new Vector3(AreaConstants.size, AreaConstants.depth, AreaConstants.size);

        //AreaData ad = new AreaData(AreaData.Type.hills, AreaConstants.size / AreaConstants.tileSize);

        //createHeights(td, ad);

        Terrain t = Terrain.CreateTerrainGameObject(td).GetComponent<Terrain>();

        t.materialType = Terrain.MaterialType.Custom;
        t.materialTemplate = grass;

    }

    void createArea() {
        AreaData ad = new AreaData(AreaData.Type.hills, AreaConstants.areaSize * AreaConstants.sizeTerrainsX, AreaConstants.areaSize * AreaConstants.sizeTerrainsY);
        print(AreaConstants.areaSize * AreaConstants.sizeTerrainsX);

        for (int x = 0; x < AreaConstants.sizeTerrainsX; x++) {
            for (int y = 0; y < AreaConstants.sizeTerrainsY; y++) {
                TerrainData td = createTerrainData(ad, 1, 0);

                Terrain t = Terrain.CreateTerrainGameObject(td).GetComponent<Terrain>();
                t.materialType = Terrain.MaterialType.Custom;
                t.materialTemplate = grass;
            }
        }

        
    }

    TerrainData createTerrainData(AreaData ad, int x, int y) {
        TerrainData td = new TerrainData();
        td.heightmapResolution = AreaConstants.resolution;
        td.size = new Vector3(AreaConstants.size, AreaConstants.depth, AreaConstants.size);

        float[,] hm = createHeights(ad, x * AreaConstants.areaSize, y * AreaConstants.areaSize);

        td.SetHeights(0, 0, hm);

        return td;
    }

    float[,] createHeights(AreaData ad, int xDisplacement, int yDisplacement) {
        float[,] hm = new float[AreaConstants.resolution, AreaConstants.resolution];
        float scale = (1f / (float)AreaConstants.depth) * AreaConstants.heightScale;


        for (int x = 0; x < AreaConstants.areaSize * AreaConstants.tileSize; x++) {
            for (int y = 0; y < AreaConstants.areaSize * AreaConstants.tileSize; y++) {
                setTile(hm, x * AreaConstants.tileTotalResolutionSize, y * AreaConstants.tileTotalResolutionSize, AreaConstants.tileTotalResolutionSize, scale * ad.heights[x + xDisplacement, y + yDisplacement]);
            }
        }

        return hm;
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

    public AreaData(Type t, int tilesX, int tilesY) {
        heights = new int[tilesX, tilesY];

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
        const int sizeMin = 9;
        const int sizeMax = 18;
        const int countMin = 3;
        const int countMax = 6;
        const float steepnessMin = 0.2f;
        const float steepnessMax = 2f;

        setZero();

        int count = Random.Range(countMin, countMax + 1);

        for (int i = 0; i < count; i++) {
            int x = Random.Range(0, heights.GetLength(0));
            int y = Random.Range(0, heights.GetLength(1));
            int size = Random.Range(sizeMin, sizeMax + 1);
            float steepness = Random.Range(steepnessMin, steepnessMax);

            createHill(x, y, size, steepness);
        }
    }

    void createHill(int x, int y, int size, float steepness) {
        for (int i = 0; i < heights.GetLength(0); i++) {
            for (int j = 0; j < heights.GetLength(1); j++) {
                float pointSize = (float)size - Mathf.Sqrt(Mathf.Pow((float)i - (float)x, 2) + Mathf.Pow((float)j - (float)y, 2));
                pointSize *= steepness;
                if (heights[i, j] < pointSize) {
                    heights[i, j] = Mathf.RoundToInt(pointSize);
                }
            }
        }
    }

    

}
*/
