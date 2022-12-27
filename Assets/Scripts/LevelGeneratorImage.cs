using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class LevelGeneratorImage : MonoBehaviour
{
    public Texture2D map;
    public ColorToPrefab[] colorMappings;

    public float scaleFactor;
    public Vector3 startPos;
    
    void Start()
    {
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                GenerateTile(x,y);
            }
        }
    }

    private void GenerateTile(int x, int y)
    {
        Color pixelColor = map.GetPixel(x, y);
        

        if (pixelColor.a == 0)
        {
            // return if the pixel is transparent
            return;
        }

        foreach (var colorMapping in colorMappings)
        {
            if (colorMapping.color.Equals(pixelColor))
            {
                Vector3 position = new Vector3(x, 1, y) * scaleFactor;
                var spawnedObject = Instantiate(colorMapping.prefab, startPos + position, Quaternion.identity, transform).GetComponent<Collectable>();
                CollectableSpawner.Instance.availableCollectables.Add(spawnedObject);
                spawnedObject.SetColor(colorMapping.color);
            }
        }
    }
}
