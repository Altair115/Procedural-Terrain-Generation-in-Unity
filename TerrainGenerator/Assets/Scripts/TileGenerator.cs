﻿using System;
using UnityEngine;
using UnityEngine.Serialization;

public class TileGenerator : MonoBehaviour
{
        [Header("Parameters")]
        public int noiseSampleSize;
        public float scale;
        public float maxHeight = 1.0f;
        public int textureResolution = 1;

        [Header("Terrain Types")] 
        public TerrainType[] heightTerrainTypes;

        private MeshRenderer _tileMeshRenderer;
        private MeshFilter _tileMeshFilter;
        private MeshCollider _tileMeshCollider;

        private void Start()
        {
            //get tile components
            _tileMeshRenderer = GetComponent<MeshRenderer>();
            _tileMeshFilter = GetComponent<MeshFilter>();
            _tileMeshCollider = GetComponent<MeshCollider>();
            
            GenerateTile(); 
        }

        /// <summary>
        /// generate a tile with a heightmap and a map texture
        /// </summary>
        private void GenerateTile()
        {
            float[,] heightMap = NoiseGenerator.GenerateNoiseMap(noiseSampleSize, scale);
            float[,] hdHeightMap = NoiseGenerator.GenerateNoiseMap(noiseSampleSize - 1, scale, textureResolution);

            Vector3[] verts = _tileMeshFilter.mesh.vertices;

            for (int x = 0; x < noiseSampleSize; x++)
            {
                for (int z = 0; z < noiseSampleSize; z++)
                {
                    int index = (x * noiseSampleSize) + z;

                    verts[index].y = heightMap[x, z] * maxHeight;
                }
            }

            _tileMeshFilter.mesh.vertices = verts;
            _tileMeshFilter.mesh.RecalculateBounds();
            _tileMeshFilter.mesh.RecalculateNormals();

            _tileMeshCollider.sharedMesh = _tileMeshFilter.mesh;
            
            Texture2D heightMapTexture = TextureBuilder.BuildTexture(hdHeightMap, heightTerrainTypes);

            _tileMeshRenderer.material.mainTexture = heightMapTexture;
        }
}

[System.Serializable]
public class TerrainType
{
    [Range(0.0f, 1.0f)]
    public float threshold;
    public Gradient colorGradient;

}