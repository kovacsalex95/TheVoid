using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace lxkvcs.PlanetGen
{
    /*
    
        TODO:
        
            -   Weird spike bug

            -   Resolution change bug

            -   Edit mode / Finalize
                -   low res on edit mode
                -   save only on finalize

            -   Steepness map

            -   Normal map

    */

    public class Planet : MonoBehaviour
    {
        [System.NonSerialized]
        public bool previewMode = false;
        bool oldPreviewMode = false;

        [Range(0, 4)]
        public int previewModeScaleFactor = 1;
        int previewScale(int resolution)
        {
            if (!previewMode)
                return resolution;

            int divider = (int)Mathf.Pow(2, previewModeScaleFactor);

            if (divider <= 0)
                return resolution;

            return resolution / divider;
        }

        [SerializeField, HideInInspector]
        public int planetID = -1;

        public ComputeShader planetDataShader;


        [System.NonSerialized]
        public string textureName = "_BaseMap";


        public HeightmapResolution faceResolution = HeightmapResolution._256x256;
        int oldFaceResolution = -1;

        public HeightmapResolution heightmapResolution = HeightmapResolution._256x256;
        int oldHeightmapResolution = -1;


        public SurfaceData surfaceSettings;
        float oldRadius = 0;

        public float Seed = 500;


        public TextureData textureSettings;
        PlanetLayerStruct[] continentLayerStructs = null;
        float continentLayersSum;
        PlanetLayerStruct[] detailLayerStructs = null;
        float detailLayersSum;

        [HideInInspector]
        public bool surfaceSettingsFoldout;
        [HideInInspector]
        public bool textureSettingsFoldout;

        PlanetFace[] GetFaces()
        {
            return gameObject.GetComponentsInChildren<PlanetFace>();
        }
        PlanetFace[] faces = null;



        public static Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
        public static int FACECOUNT = 6;

        string assetsPath = string.Empty;
        public string AssetsPath
        {
            get
            {
#if UNITY_EDITOR
                if (assetsPath.Equals(string.Empty))
                {
                    var asset = "";

                    var guids = AssetDatabase.FindAssets(string.Format("{0} t:script", typeof(Planet).Name));

                    if (guids.Length > 1)
                    {
                        foreach (var guid in guids)
                        {
                            var assetPath = AssetDatabase.GUIDToAssetPath(guid);

                            var filename = Path.GetFileNameWithoutExtension(assetPath);

                            if (filename == typeof(Planet).Name)
                            {
                                asset = guid;
                                break;
                            }
                        }
                    }
                    else if (guids.Length == 1)
                    {
                        asset = guids[0];
                    }
                    else
                    {
                        Debug.LogErrorFormat("Unable to locate {0}", typeof(Planet).Name);
                        return null;
                    }

                    assetsPath = AssetDatabase.GUIDToAssetPath(asset).Replace("/Planet.cs", "");
                }
#else
                return "";
#endif

                return assetsPath;
            }
        }
        public string uniqueId
        {
            get
            {
                if (planetID == -1)
                    planetID = FindObjectsOfType<Planet>().Length;

                return string.Format("planet{0}", planetID);
            }
        }

        public void PropertyChanged()
        {
            if (previewMode != oldPreviewMode)
            {
                if (previewMode)
                    PreviewEnable();
                else
                    PreviewDisable();

                oldPreviewMode = previewMode;
            }

            if (previewMode)
                GeneratePlanet();
        }

        public void GeneratePlanet()
        {
            faces = GetFaces();

            CheckDataFolders();

            if (!previewMode)
                textureSettings.GenerateLayerMaps(AssetsPath + "/GeneratedData/" + uniqueId);

            CheckFaceObjects();

            faces = GetFaces();

            GenerateTerrainFaces();
        }

        void PreviewEnable()
        {

        }
        void PreviewDisable()
        {
            GeneratePlanet();
        }

        void CheckDataFolders()
        {
            if (!previewMode)
                return;
#if UNITY_EDITOR
            if (!AssetDatabase.IsValidFolder(AssetsPath + "/GeneratedData"))
                AssetDatabase.CreateFolder(AssetsPath, "GeneratedData");

            if (!AssetDatabase.IsValidFolder(AssetsPath + "/GeneratedData/" + uniqueId))
                AssetDatabase.CreateFolder(AssetsPath + "/GeneratedData", uniqueId);

            if (!AssetDatabase.IsValidFolder(AssetsPath + "/GeneratedData/" + uniqueId + "/Mesh"))
                AssetDatabase.CreateFolder(AssetsPath + "/GeneratedData/" + uniqueId, "Mesh");

            if (!AssetDatabase.IsValidFolder(AssetsPath + "/GeneratedData/" + uniqueId + "/Data"))
                AssetDatabase.CreateFolder(AssetsPath + "/GeneratedData/" + uniqueId, "Data");

            if (!AssetDatabase.IsValidFolder(AssetsPath + "/GeneratedData/" + uniqueId + "/Textures"))
                AssetDatabase.CreateFolder(AssetsPath + "/GeneratedData/" + uniqueId, "Textures");
#endif
        }

        void CheckFaceObjects()
        {
            if (faces.Length == FACECOUNT && surfaceSettings.radius == oldRadius && oldFaceResolution == previewScale((int)faceResolution) && oldHeightmapResolution == previewScale((int)heightmapResolution))
                return;

            oldRadius = surfaceSettings.radius;
            oldFaceResolution = previewScale((int)faceResolution);
            oldHeightmapResolution = previewScale((int)heightmapResolution);

            if (!previewMode)
                GenerateFaceObjects();
            else
            {
                int faceIndex = 0;
                foreach(PlanetFace face in faces)
                {
                    face.UpdateProperties(faceIndex, previewScale((int)faceResolution), previewScale((int)heightmapResolution), surfaceSettings.radius);
                    faceIndex++;
                }
            }
        }

        void GenerateFaceObjects()
        {
            if (faces.Length > 0) {
                while (transform.childCount > 0)
                    DestroyImmediate(transform.GetChild(0).gameObject);
            }

            for (int i = 0; i < FACECOUNT; i++)
                GenerateFaceObject(i);
        }

        void GenerateFaceObject(int faceIndex)
        {
            GameObject parentObj = new GameObject(string.Format("Face {0}", faceIndex + 1));
            parentObj.transform.parent = transform;
            parentObj.transform.localPosition = Vector3.zero;
            parentObj.layer = gameObject.layer;

            PlanetFace face = parentObj.AddComponent<PlanetFace>();

            parentObj.gameObject.AddComponent<MeshFilter>();
            parentObj.gameObject.AddComponent<MeshRenderer>().sharedMaterial = new Material(textureSettings.planetMaterial);
            parentObj.gameObject.AddComponent<MeshCollider>();

            face.UpdateProperties(faceIndex, (int)faceResolution, (int)heightmapResolution, surfaceSettings.radius);
        }

        void GenerateTerrainFaces()
        {
            if (!planetDataShader) {
                Debug.LogError("Planet shader not found!");
                return;
            }

            GenerateLayerBufferStructs();

            for (int i = 0; i < FACECOUNT; i++)
                GenerateTerrainFace(i);
        }

        void GenerateLayerBufferStructs()
        {
            continentLayerStructs = new PlanetLayerStruct[surfaceSettings.continents.Length];
            continentLayersSum = 0;
            for (int i = 0; i < continentLayerStructs.Length; i++)
            {
                if (!surfaceSettings.continents[i].enabled)
                    continue;

                continentLayersSum += surfaceSettings.continents[i].ratio;
                continentLayerStructs[i] = surfaceSettings.continents[i].Struct();
            }
            detailLayerStructs = new PlanetLayerStruct[surfaceSettings.detail.Length];
            detailLayersSum = 0;
            for (int i = 0; i < detailLayerStructs.Length; i++)
            {
                if (!surfaceSettings.detail[i].enabled)
                    continue;

                detailLayersSum += surfaceSettings.detail[i].ratio;
                detailLayerStructs[i] = surfaceSettings.detail[i].Struct();
            }
        }

        void GenerateTerrainFace(int faceIndex)
        {
            PlanetFace face = faces[faceIndex];

            int textureSize = previewScale((int)heightmapResolution);
            int layerStride = PlanetLayer.stride;

            // Vertices buffer
            ComputeBuffer verticesBuffer = new ComputeBuffer(face.dataVertices.Length, sizeof(float) * 3);
            verticesBuffer.SetData(face.dataVertices);

            // Continent layers buffer
            ComputeBuffer continentLayersBuffer = new ComputeBuffer(continentLayerStructs.Length, layerStride);
            continentLayersBuffer.SetData(continentLayerStructs);

            // Detail layers buffer
            ComputeBuffer detailLayersBuffer = new ComputeBuffer(detailLayerStructs.Length, layerStride);
            detailLayersBuffer.SetData(detailLayerStructs);

            // Water layes buffer
            // TODO

            // Create blank pixel array
            Vector3[] terrainPixels = new Vector3[textureSize * textureSize];

            // Pixel buffer
            ComputeBuffer colorsBuffer = new ComputeBuffer(terrainPixels.Length, sizeof(float) * 3);
            colorsBuffer.SetData(terrainPixels);


            // Add buffers
            int kernelIndex = planetDataShader.FindKernel("GenerateData");

            planetDataShader.SetBuffer(kernelIndex, "Vertices", verticesBuffer);
            planetDataShader.SetBuffer(kernelIndex, "Colors", colorsBuffer);
            planetDataShader.SetBuffer(kernelIndex, "Continents", continentLayersBuffer);
            planetDataShader.SetBuffer(kernelIndex, "Details", detailLayersBuffer);

            // Add other variables
            planetDataShader.SetFloat("TextureSize", textureSize);
            planetDataShader.SetVector("Normal", directions[faceIndex]);
            planetDataShader.SetVector("RemapHeight", surfaceSettings.remapRange);
            planetDataShader.SetFloat("ContinentsSum", continentLayersSum);
            planetDataShader.SetFloat("ContinentsLength", continentLayerStructs.Length);
            planetDataShader.SetFloat("DetailsSum", detailLayersSum);
            planetDataShader.SetFloat("DetailsLength", detailLayerStructs.Length);
            planetDataShader.SetFloat("Seed", Seed);

            // Dispatch
            planetDataShader.Dispatch(kernelIndex, textureSize / 8, textureSize / 8, 1);

            colorsBuffer.GetData(terrainPixels);
            Color[] pixels = new Color[terrainPixels.Length];
            for (int i = 0; i < terrainPixels.Length; i++)
                pixels[i] = new Color(terrainPixels[i].x, 0, 0);

            // Dispose
            verticesBuffer.Dispose();
            colorsBuffer.Dispose();
            continentLayersBuffer.Dispose();
            detailLayersBuffer.Dispose();

            // Create data texture
            Texture2D terrainData = new Texture2D(textureSize, textureSize, TextureFormat.ARGB32, false);
            terrainData.filterMode = FilterMode.Bilinear;
            terrainData.wrapMode = TextureWrapMode.Mirror;
            terrainData.SetPixels(pixels);
            terrainData.Apply();

            // Save data texture
            string dataPath = AssetsPath + "/GeneratedData/" + uniqueId + "/Data/face" + faceIndex.ToString() + ".asset";


#if UNITY_EDITOR
            if (!previewMode)
                AssetDatabase.CreateAsset(terrainData, dataPath);
            face.meshRenderer.sharedMaterial.SetTexture(textureName, previewMode ? terrainData : AssetDatabase.LoadAssetAtPath(dataPath, typeof(Texture2D)) as Texture2D);
#else
            face.meshRenderer.sharedMaterial.SetTexture(textureName, terrainData);
#endif

            face.meshRenderer.sharedMaterial.SetVector("_ElevationRange", surfaceSettings.heightRange * surfaceSettings.radius);

            face.ConstructMesh(faceIndex, terrainData, surfaceSettings.heightRange, previewMode);
        }
    }


    [System.Serializable]
    public enum HeightmapResolution
    {
        Unknown = -1,
        _32x32 = 32,
        _64x64 = 64,
        _128x128 = 128,
        _256x256 = 256,
        _512x512 = 512,
        _1024x1024 = 1024,
        _2048x2048 = 2048,
        _4096x4096 = 4096,
    }
}