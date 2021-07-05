using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace lxkvcs.PlanetGen
{
    [CreateAssetMenu()]
    public class TextureData : ScriptableObject
    {
        [SerializeField]
        public Material planetMaterial;

        [SerializeField]
        public int textureResolution = 1024;

        [SerializeField]
        public TextureLayer[] surfaceLayers;

        public void GenerateLayerMaps(string planetFolder)
        {
            // TODO: some alternative shit
            return;

            if (!planetMaterial)
            {
                Debug.LogError("Planet material not set!");
                return;
            }

            List<Texture2D> albedos = new List<Texture2D>();
            List<Texture2D> normals = new List<Texture2D>();

            foreach (TextureLayer layer in surfaceLayers)
            {
                if (layer.albedo == null || layer.normal == null || layer.albedo.width != textureResolution || layer.albedo.height != textureResolution || layer.normal.width != textureResolution || layer.normal.height != textureResolution)
                    continue;

                albedos.Add(layer.albedo);
                normals.Add(layer.normal);
            }

            if (albedos.Count == 0)
                return;

            Texture2DArray albedoArray = new Texture2DArray(textureResolution, textureResolution, albedos.Count, TextureFormat.ARGB32, false);
            Texture2DArray normalArray = new Texture2DArray(textureResolution, textureResolution, albedos.Count, TextureFormat.ARGB32, false);
            for (int i = 0; i < albedos.Count; i++)
            {
                albedoArray.SetPixels32(albedos[i].GetPixels32(), i);
                normalArray.SetPixels32(normals[i].GetPixels32(), i);
            }

            albedoArray.Apply();
            normalArray.Apply();

            planetMaterial.SetFloat("_MapsCount", albedos.Count);

            albedos.Clear();
            normals.Clear();

#if UNITY_EDITOR
            AssetDatabase.CreateAsset(albedoArray, planetFolder + "/Textures/AlbedoArray.asset");
            AssetDatabase.CreateAsset(normalArray, planetFolder + "/Textures/NormalArray.asset");

            planetMaterial.SetTexture("_AlbedoMaps", AssetDatabase.LoadAssetAtPath(planetFolder + "/Textures/AlbedoArray.asset", typeof(Texture2DArray)) as Texture2DArray);
            planetMaterial.SetTexture("_NormalMaps", AssetDatabase.LoadAssetAtPath(planetFolder + "/Textures/NormalArray.asset", typeof(Texture2DArray)) as Texture2DArray);
#else
            planetMaterial.SetTexture("_AlbedoMaps", albedoArray);
            planetMaterial.SetTexture("_NormalMaps", normalArray);
#endif
        }
    }

    [System.Serializable]
    public class TextureLayer
    {
        public Texture2D albedo = null;
        public Texture2D normal = null;
    }
}