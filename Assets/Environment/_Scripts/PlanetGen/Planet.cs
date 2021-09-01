using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace lxkvcs.PlanetGen
{
    /*
    
        TODO:

            -   Steepness map

    */
    [ExecuteInEditMode()]
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


        public SurfaceData surfaceSettings;
        float oldRadius = 0;

        public float Seed = 500;


        PlanetLayerStruct[] continentLayerStructs = null;
        float continentLayersSum;
        PlanetLayerStruct[] detailLayerStructs = null;
        float detailLayersSum;

        [HideInInspector]
        public bool surfaceSettingsFoldout;

        PlanetFace[] GetFaces()
        {
            return gameObject.GetComponentsInChildren<PlanetFace>();
        }
        PlanetFace[] faces = null;

        [SerializeField, HideInInspector]
        List<ControlPoint> controlPoints = new List<ControlPoint>();



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

            CheckFaceObjects();

            faces = GetFaces();

            GenerateTerrainFaces();

            controlPoints.Clear();

            foreach (PlanetFace face in faces)
                controlPoints.AddRange(face.controlPoints);

            Debug.Log($"Control points: {controlPoints.Count}");
        }

        public void OnDrawGizmosSelected()
        {
            int drawPoints = 5000;

            if (false)
            {
                int stepSize = Mathf.FloorToInt((float)controlPoints.Count / (float)drawPoints);

                for (int i = 0; i < controlPoints.Count; i += stepSize)
                {
                    Gizmos.color = Assets.UI._Scripts.Misc.UIUtil.ColorLerp(Color.red, Color.green, controlPoints[i].flatness);
                    Gizmos.DrawSphere(controlPoints[i].point, 8);
                }
            }
            else
            {
                int i = 0;

                foreach (ControlPoint cPoint in controlPoints)
                {
                    if (i >= drawPoints)
                        break;

                    i++;

                    Gizmos.color = Assets.UI._Scripts.Misc.UIUtil.ColorLerp(Color.red, Color.green, cPoint.flatness);
                    Gizmos.DrawSphere(cPoint.point, 4);
                }
            }
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
            if (faces.Length == FACECOUNT && surfaceSettings.radius == oldRadius && oldFaceResolution == previewScale((int)faceResolution))
                return;

            oldRadius = surfaceSettings.radius;
            oldFaceResolution = previewScale((int)faceResolution);

            if (!previewMode)
                GenerateFaceObjects();
            else
            {
                int faceIndex = 0;
                foreach(PlanetFace face in faces)
                {
                    face.UpdateProperties(faceIndex, previewScale((int)faceResolution), surfaceSettings.radius);
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
            parentObj.gameObject.AddComponent<MeshRenderer>().sharedMaterial = new Material(surfaceSettings.planetMaterial);
            parentObj.gameObject.AddComponent<MeshCollider>();

            face.UpdateProperties(faceIndex, (int)faceResolution, surfaceSettings.radius);
        }

        public void RefreshFaceMaterials()
        {
            PlanetFace[] faces = GetFaces();
            foreach(PlanetFace face in faces)
            {
                if (face.meshRenderer)
                    face.meshRenderer.sharedMaterial = new Material(surfaceSettings.planetMaterial);
            }
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

            int textureSize = previewScale((int)faceResolution);
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

            // Dispose
            verticesBuffer.Dispose();
            colorsBuffer.Dispose();
            continentLayersBuffer.Dispose();
            detailLayersBuffer.Dispose();

            // Contruct mesh
            face.ConstructMesh(faceIndex, terrainPixels, surfaceSettings.heightRange, previewMode);
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

    [System.Serializable]
    public struct ControlPoint
    {
        public Vector3 point;
        public Vector3 normal;
        public float flatness;

        public ControlPoint(Vector3 a, Vector3 b, Vector3 c, Vector3 worldCenter)
        {
            this.point = (a + c) / 2;

            Vector3 side1 = b - a;
            Vector3 side2 = c - a;

            this.normal = Vector3.Cross(side1, side2).normalized;

            Vector3 worldDifference = (this.point - worldCenter).normalized;
            this.flatness = Mathf.Clamp01(Vector3.Dot(this.normal, worldDifference));
            this.flatness = Mathf.Max(0, flatness - 0.95f) * 20;
        }
    }
}