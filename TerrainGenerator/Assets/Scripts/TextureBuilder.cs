using UnityEngine;

public class TextureBuilder
{
       /// <summary>
       /// builds a texture based on the given noise map
       /// </summary>
       /// <param name="noiseMap">noise map needed to generate the texture</param>
       /// <returns>a texture2D with pixels colored by the noise map</returns>
       public static Texture2D BuildTexture(float[,] noiseMap, TerrainType[] terrainTypes)
       {
              Color[] pixels = new Color[noiseMap.Length];

              int pixelLength = noiseMap.GetLength(0);

              for (int x = 0; x < pixelLength; x++)
              {
                     for (int z = 0; z < pixelLength; z++)
                     {
                            int index = (x * pixelLength) + z;

                            for (int t = 0; t < terrainTypes.Length; t++)
                            {
                                   if (noiseMap[x, z] < terrainTypes[t].threshold)
                                   {
                                          float minVal = t == 0 ? 0 : terrainTypes[t - 1].threshold;
                                          float maxVal = terrainTypes[t].threshold;

                                          pixels[index] = terrainTypes[t].colorGradient.Evaluate(1.0f - (maxVal - noiseMap[x, z]) / (maxVal - minVal));
                                          break;
                                   }
                            }
                     }
              }

              //create a new Texture2D and set it up
              Texture2D texture = new Texture2D(pixelLength, pixelLength);
              texture.wrapMode = TextureWrapMode.Clamp;
              texture.filterMode = FilterMode.Bilinear;
              texture.SetPixels(pixels);
              texture.Apply();

              return texture;
       }

       public static TerrainType[,] CreateTerrainTypeMap(float[,] noiseMap, TerrainType[] terrainTypes)
       {
              int size = noiseMap.GetLength(0);
              TerrainType[,] outputMap = new TerrainType[size, size];

              for (int x = 0; x < size; x++)
              {
                     for (int z = 0; z < size; z++)
                     {
                            for (int t = 0; t < terrainTypes.Length; t++)
                            {
                                   if (noiseMap[x, z] < terrainTypes[t].threshold)
                                   {
                                          outputMap[x, z] = terrainTypes[t];
                                          break;
                                   }
                            }
                     }
              }

              return outputMap;
       }
}