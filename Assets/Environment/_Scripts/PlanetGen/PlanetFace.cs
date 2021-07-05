using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace lxkvcs.PlanetGen
{
    public class PlanetFace : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        int faceResolution;

        [SerializeField, HideInInspector]
        Vector3 localUp;

        [SerializeField, HideInInspector]
        float radius;

        [SerializeField, HideInInspector]
        public MeshFilter meshFilter = null;

        [SerializeField, HideInInspector]
        public MeshCollider meshCollider = null;

        [SerializeField, HideInInspector]
        public MeshRenderer meshRenderer = null;

        [SerializeField, HideInInspector]
        public int dataResolution = -1;


        [SerializeField, HideInInspector]
        Vector3 axisA;
        [SerializeField, HideInInspector]
        Vector3 axisB;

        [SerializeField, HideInInspector]
        public Vector3[] dataVertices = null;


        Planet _planet = null;
        Planet planet
        {
            get
            {
                if (_planet == null)
                    _planet = GetComponentInParent<Planet>();

                return _planet;
            }
        }

        public void UpdateProperties(int faceIndex, int faceResolution, int dataResolution, float radius)
        {
            meshFilter = GetComponent<MeshFilter>();
            meshRenderer = GetComponent<MeshRenderer>();
            meshCollider = GetComponent<MeshCollider>();

            if (dataVertices == null || dataResolution != this.dataResolution)
            {
                dataVertices = SphereFaceVertices(faceIndex, dataResolution);
                this.dataResolution = dataResolution;
            }

            meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            meshRenderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
            meshRenderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            meshRenderer.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;

            if (meshFilter != null && Planet.directions[faceIndex] == this.localUp && faceResolution == this.faceResolution && radius == this.radius)
                return;

            this.localUp = Planet.directions[faceIndex];
            this.faceResolution = faceResolution;
            this.radius = radius;

            axisA = new Vector3(Planet.directions[faceIndex].y, Planet.directions[faceIndex].z, Planet.directions[faceIndex].x);
            axisB = Vector3.Cross(Planet.directions[faceIndex], axisA);
        }

        public void ConstructMesh(int faceIndex, Texture2D heightmap, Vector2 heightRange, bool previewMode)
        {
            Mesh faceMesh = new Mesh();
            faceMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            int colliderResolution = faceResolution;

            Vector3[] vertices = new Vector3[colliderResolution * colliderResolution];
            Vector3[] normals = new Vector3[vertices.Length];
            Vector2[] uvs = new Vector2[vertices.Length];
            int[] triangles = new int[(colliderResolution - 1) * (colliderResolution - 1) * 6];
            int triIndex = 0;

            for (int y = 0; y < colliderResolution; y++)
            {
                for (int x = 0; x < colliderResolution; x++)
                {
                    int i = x + y * colliderResolution;

                    Vector2 percent = new Vector2(x, y) / (colliderResolution - 1);
                    Vector3 pointOnUnitCube = localUp + (percent.x - .5f) * 2 * axisA + (percent.y - .5f) * 2 * axisB;

                    int terrainHeightX = Mathf.RoundToInt(percent.x * (float)(heightmap.width - 1));
                    int terrainHeightY = Mathf.RoundToInt(percent.y * (float)(heightmap.width - 1));

                    float terrainHeightRaw = Mathf.Clamp01(heightmap.GetPixel(terrainHeightX, terrainHeightY).r);
                    float terrainHeight = Mathf.Lerp(Mathf.Min(heightRange.x, heightRange.y), Mathf.Max(heightRange.x, heightRange.y), terrainHeightRaw);

                    Vector3 pointOnUnitSphere = pointOnUnitCube.normalized * (1 + terrainHeight);

                    vertices[i] = pointOnUnitSphere * radius;
                    normals[i] = pointOnUnitSphere;

                    if (x != colliderResolution - 1 && y != colliderResolution - 1)
                    {
                        triangles[triIndex] = i;
                        triangles[triIndex + 1] = i + colliderResolution + 1;
                        triangles[triIndex + 2] = i + colliderResolution;

                        triangles[triIndex + 3] = i;
                        triangles[triIndex + 4] = i + 1;
                        triangles[triIndex + 5] = i + colliderResolution + 1;
                        triIndex += 6;
                    }

                    uvs[i] = new Vector2(x, y) / (faceResolution - 1);
                }
            }

            faceMesh.vertices = vertices;
            faceMesh.triangles = triangles;
            faceMesh.uv = uvs;
            faceMesh.normals = normals;

            string meshPath = planet.AssetsPath + "/GeneratedData/" + planet.uniqueId + "/Mesh/faceCollider" + faceIndex.ToString() + ".asset";

#if UNITY_EDITOR
            if (!previewMode)
                AssetDatabase.CreateAsset(faceMesh, meshPath);
            Mesh generatedMesh = previewMode ? faceMesh : AssetDatabase.LoadAssetAtPath(meshPath, typeof(Mesh)) as Mesh;
#else
            Mesh generatedMesh = faceMesh;
#endif

            meshCollider.sharedMesh = generatedMesh;
            meshFilter.sharedMesh = generatedMesh;
            meshCollider.convex = false;
        }

        Vector3[] SphereFaceVertices(int faceIndex, int textureSize)
        {
            Vector3 axisA = new Vector3(Planet.directions[faceIndex].y, Planet.directions[faceIndex].z, Planet.directions[faceIndex].x);
            Vector3 axisB = Vector3.Cross(Planet.directions[faceIndex], axisA);

            Vector3[] vertices = new Vector3[textureSize * textureSize];

            Vector2 percent;
            Vector3 pointOnUnitCube;

            for (int y = 0; y < textureSize; y++)
            {
                for (int x = 0; x < textureSize; x++)
                {
                    int i = y * textureSize + x;

                    percent = new Vector2(x, y) / (textureSize - 1);

                    pointOnUnitCube = Planet.directions[faceIndex] + (percent.x - .5f) * 2 * axisA + (percent.y - .5f) * 2 * axisB;

                    vertices[i] = pointOnUnitCube.normalized;
                }
            }

            return vertices;
        }
    }
}