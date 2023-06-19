using UnityEngine;

public class NoiseGenerator : MonoBehaviour
{
    /// <summary>
    /// Generate a Noise map with perlin noise
    /// based on a number of parameters
    /// </summary>
    /// <param name="noiseSampleSize">how large of an area of the sample do you want to capture</param>
    /// <param name="scale">How "zoomed in" should the sample be higher value = more zoom</param>
    /// <param name="resolution">how large the texture is going to be (a resolution of 1 will make each face of the mesh equal to 1 pixel)</param>
    /// <returns>a 2D float array</returns>
    public static float[,] GenerateNoiseMap(int noiseSampleSize, float scale, int resolution = 1)
    {
        float[,] noiseMap = new float[noiseSampleSize * resolution, noiseSampleSize * resolution];

        for (int x = 0; x < noiseSampleSize * resolution; x++)
        {
            for (int y = 0; y < noiseSampleSize * resolution; y++)
            {
                float samplePosX = x / scale / resolution;
                float samplePosY = y / scale / resolution;

                noiseMap[x, y] = Mathf.PerlinNoise(samplePosX, samplePosY);
            }
        }

        return noiseMap;
    }
}
