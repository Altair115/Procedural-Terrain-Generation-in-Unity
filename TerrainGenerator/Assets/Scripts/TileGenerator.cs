using System;
using UnityEngine;
using UnityEngine.Serialization;

public class TileGenerator : MonoBehaviour
{
        [Header("Parameters")]
        public int noiseSampleSize;
        public float scale;
        public float maxHeight = 1.0f;
        public int textureResolution = 1;
        
        [HideInInspector]
        public Vector2 offset;

        [Header("Terrain Types")] 
        public TerrainType[] heightTerrainTypes;
        public TerrainType[] heatTerrainTypes;

        [Header("Waves")] 
        public Wave[] waves;
        public Wave[] heatWaves;

        [Header("Curves")] 
        public AnimationCurve heightCurve;

        private MeshRenderer _tileMeshRenderer;
        private MeshFilter _tileMeshFilter;
        private MeshCollider _tileMeshCollider;
        private MeshGenerator _meshGenerator;
        private MapGenerator _mapGenerator;

        private void Start()
        {
            //get tile components
            _tileMeshRenderer = GetComponent<MeshRenderer>();
            _tileMeshFilter = GetComponent<MeshFilter>();
            _tileMeshCollider = GetComponent<MeshCollider>();
            _meshGenerator = GetComponent<MeshGenerator>();
            _mapGenerator = FindObjectOfType<MapGenerator>();
            
            GenerateTile(); 
        }

        /// <summary>
        /// generate a tile with a heightmap and a map texture
        /// </summary>
        private void GenerateTile()
        {
            float[,] heightMap = NoiseGenerator.GenerateNoiseMap(noiseSampleSize, scale, waves, offset);
            float[,] hdHeightMap = NoiseGenerator.GenerateNoiseMap(noiseSampleSize - 1, scale, waves, offset, textureResolution);

            Vector3[] verts = _tileMeshFilter.mesh.vertices;

            for (int x = 0; x < noiseSampleSize; x++)
            {
                for (int z = 0; z < noiseSampleSize; z++)
                {
                    int index = (x * noiseSampleSize) + z;
                    
                    verts[index].y = heightCurve.Evaluate(heightMap[x, z]) * maxHeight;
                }
            }

            _tileMeshFilter.mesh.vertices = verts;
            _tileMeshFilter.mesh.RecalculateBounds();
            _tileMeshFilter.mesh.RecalculateNormals();

            //update mesh collider
            _tileMeshCollider.sharedMesh = _tileMeshFilter.mesh;
            
            //create the height map texture
            Texture2D heightMapTexture = TextureBuilder.BuildTexture(hdHeightMap, heightTerrainTypes);

            //apply the height map texture to the MeshRenderer
            //_tileMeshRenderer.material.mainTexture = heightMapTexture;
            float[,] heatMap = GenerateHeatMap(heightMap);
            _tileMeshRenderer.material.mainTexture = TextureBuilder.BuildTexture(heatMap, heatTerrainTypes);
        }

        /// <summary>
        /// Generates a new Heat mao
        /// </summary>
        /// <param name="heightmap">takes a height map to apply a heatmap on top of</param>
        /// <returns>a Heatmap</returns>
        float[,] GenerateHeatMap(float[,] heightmap)
        {
            float[,] uniformHeatMap = NoiseGenerator.GenerateUniformNoiseMap(noiseSampleSize,
                transform.position.z * (noiseSampleSize / _meshGenerator.xSize),
                (noiseSampleSize / 2 * _mapGenerator.numX + 1));
            float[,] randomHeatMap = NoiseGenerator.GenerateNoiseMap(noiseSampleSize, scale, heatWaves, offset);

            float[,] heatMap = new float[noiseSampleSize, noiseSampleSize];
            for (int x = 0; x < noiseSampleSize; x++)
            {
                for (int z = 0; z < noiseSampleSize; z++)
                {
                    heatMap[x, z] = randomHeatMap[x, z] * uniformHeatMap[x, z];
                    heatMap[x, z] += 0.5f * heightmap[x, z];

                    heatMap[x, z] = Mathf.Clamp(heatMap[x, z], 0.0f, 0.99f);
                }
            }

            return heatMap;
        }
}

[System.Serializable]
public class TerrainType
{
    [Range(0.0f, 1.0f)]
    public float threshold;
    public Gradient colorGradient;

}