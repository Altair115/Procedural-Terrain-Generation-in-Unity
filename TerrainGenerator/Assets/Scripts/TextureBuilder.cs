using UnityEngine;

public class TextureBuilder
{
       /// <summary>
       /// builds a texture based on the given noise map
       /// </summary>
       /// <param name="noiseMap">noise map needed to generate the texture</param>
       /// <returns>a texture2D with pixels colored by the noise map</returns>
       public static Texture2D BuildTexture(float[,] noiseMap)
       {
              Color[] pixels = new Color[noiseMap.Length];

              int pixelLength = noiseMap.GetLength(0);

              for (int x = 0; x < pixelLength; x++)
              {
                     for (int z = 0; z < pixelLength; z++)
                     {
                            int index = (x * pixelLength) + z;

                            pixels[index] = Color.Lerp(Color.black, Color.white, noiseMap[x,z]);
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
}