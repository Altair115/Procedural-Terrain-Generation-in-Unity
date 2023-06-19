using System;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
        [Header("Parameters")]
        public int noiseSampleSize;
        public float scale;

        private MeshRenderer _tileMeshrenderer;
        private MeshFilter _tileMeshFilter;
        private MeshCollider _tileMeshCollider;

        private void Start()
        {
            //get tile components
            _tileMeshrenderer = GetComponent<MeshRenderer>();
            _tileMeshFilter = GetComponent<MeshFilter>();
            _tileMeshCollider = GetComponent<MeshCollider>();
            
            GenerateTile(); 
        }

        private void GenerateTile()
        {
            float[,] heightMap = NoiseGenerator.GenerateNoiseMap(noiseSampleSize, scale);
            Texture2D heighMapTexture = TextureBuilder.BuildTexture(heightMap);

            _tileMeshrenderer.material.mainTexture = heighMapTexture;
        }
}