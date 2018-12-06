using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelateImage : MonoBehaviour {

    Texture2D sprite;
    public int resolution = 2;

	// Use this for initialization
	void Start () {
        sprite = GetComponent<SpriteRenderer>().sprite.texture;
        Texture2D pixelated = new Texture2D(resolution, resolution, TextureFormat.ARGB32, false);
        int pixelSize = sprite.width / resolution;
        for (int i=0; i< resolution; i++)
        {
            for (int j = 0; j < resolution; j++)
            {
                Color avg = new Color();
                Color[] pixels = sprite.GetPixels(pixelSize * i, pixelSize * j, pixelSize, pixelSize);
                foreach (Color c in pixels)
                {
                    avg += c;
                }
                avg /= pixels.Length;
                pixelated.SetPixel(i, j, avg);
            }
        }
        pixelated.Apply();
        pixelated.filterMode = FilterMode.Point;
        Sprite newSprite = Sprite.Create(pixelated, new Rect(0.0f, 0.0f, pixelated.width, pixelated.height), new Vector2(0.5f, 0.5f), pixelated.width);
        GetComponent<SpriteRenderer>().sprite = newSprite;
	}
	
}
