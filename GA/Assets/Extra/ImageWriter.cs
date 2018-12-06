using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageWriter {

	public static void WriteImage(int resolution, Color[] pixels, SpriteRenderer renderer)
    {
        Texture2D oldSprite = renderer.sprite.texture;
        Texture2D pixelated = new Texture2D(resolution, resolution, TextureFormat.ARGB32, false);
        int pixelSize = oldSprite.width / resolution;
        for (int i = 0; i < resolution; i++)
        {
            for (int j = 0; j < resolution; j++)
            {
                pixelated.SetPixel(i, j, pixels[i + j*resolution]);
            }
        }
        pixelated.Apply();
        pixelated.filterMode = FilterMode.Point;
        Sprite newSprite = Sprite.Create(pixelated, new Rect(0.0f, 0.0f, pixelated.width, pixelated.height), new Vector2(0.5f, 0.5f), pixelated.width);
        renderer.sprite = newSprite;
    }

    public static float CompareImage(int resolution, Color[] pixels, SpriteRenderer renderer)
    {
        Texture2D sprite = renderer.sprite.texture;
        int pixelSize = sprite.width / resolution;
        float fitness = 0;
        for (int i = 0; i < resolution; i++)
        {
            for (int j = 0; j < resolution; j++)
            {
                Color avg = new Color();
                Color[] localPixels = sprite.GetPixels(pixelSize * i, pixelSize * j, pixelSize, pixelSize);
                foreach (Color c in localPixels)
                {
                    avg += c;
                }
                avg /= localPixels.Length;
                Color difference = avg - pixels[i + j * resolution];
                fitness += -Mathf.Abs( difference.g + difference.b + difference.r );
            }
        }
        return fitness;
    }
}
