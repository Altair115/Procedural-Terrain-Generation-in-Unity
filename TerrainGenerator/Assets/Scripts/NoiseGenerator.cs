using UnityEngine;

public class NoiseGenerator : MonoBehaviour
{
    /// <summary>
    /// Generate a Noise map with perlin noise
    /// based on a number of parameters
    /// </summary>
    /// <param name="noiseSampleSize">how large of an area of the sample do you want to capture</param>
    /// <param name="scale">How "zoomed in" should the sample be higher value = more zoom</param>
    /// <returns>a 2D float array</returns>
    public static float[,] GenerateNoiseMap(int noiseSampleSize, float scale)
    {
        float[,] noiseMap = new float[noiseSampleSize, noiseSampleSize];

        for (int x = 0; x < noiseSampleSize; x++)
        {
            for (int y = 0; y < noiseSampleSize; y++)
            {
                float samplePosX = x / scale;
                float samplePosY = y / scale;

                noiseMap[x, y] = Mathf.PerlinNoise(samplePosX, samplePosY);
            }
        }

        return noiseMap;
    }
}
