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
        Vector3 axisA;
        [SerializeField, HideInInspector]
        Vector3 axisB;

        [SerializeField, HideInInspector]
        public Vector3[] dataVertices = null;

        [SerializeField, HideInInspector]
        public ControlPoint[] controlPoints = null;


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

        public void UpdateProperties(int faceIndex, int faceResolution, float radius)
        {
            meshFilter = GetComponent<MeshFilter>();
            meshRenderer = GetComponent<MeshRenderer>();
            meshCollider = GetComponent<MeshCollider>();

            if (dataVertices == null || faceResolution != this.faceResolution)
                dataVertices = SphereFaceVertices(faceIndex, faceResolution);

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

        public void ConstructMesh(int faceIndex, Vector3[] terrainData, Vector2 heightRange, bool previewMode)
        {
            Mesh faceMesh = new Mesh { indexFormat = UnityEngine.Rendering.IndexFormat.UInt32 };
            int colliderResolution = faceResolution;

            Vector3[] vertices = new Vector3[colliderResolution * colliderResolution];
            Color[] colors = new Color[vertices.Length];
            Vector3[] normals = new Vector3[vertices.Length];
            Vector2[] uvs = new Vector2[vertices.Length];
            int[] triangles = new int[(colliderResolution - 1) * (colliderResolution - 1) * 6];
            int triIndex = 0;

            for (int y = 0; y < colliderResolution; y++)
            {
                for (int x = 0; x < colliderResolution; x++)
                {
                    int i = x + y * colliderResolution;

                    Vector2 percent = SpherizeVector(new Vector2(x, y) / (colliderResolution - 1));


                    Vector3 pointOnUnitCube = localUp + (percent.x - .5f) * 2 * axisA + (percent.y - .5f) * 2 * axisB;

                    Vector3 dataRaw = terrainData[i];
                    colors[i] = new Color(dataRaw.x, dataRaw.y, dataRaw.z);

                    float terrainHeightRaw = Mathf.Clamp01(dataRaw.x);
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

            List<ControlPoint> controlPoints = new List<ControlPoint>();

            for (int x = 1; x < colliderResolution; x++)
            {
                for (int y = 1; y < colliderResolution; y++)
                {
                    int topLeftIndex = (x - 1) + (y - 1) * colliderResolution;
                    int topRightIndex = x + (y - 1) * colliderResolution;
                    int bottomRightIndex = x + y * colliderResolution;

                    Vector3 topLeftVertice = vertices[topLeftIndex];
                    Vector3 topRightVertice = vertices[topRightIndex];
                    Vector3 bottomRightVertice = vertices[bottomRightIndex];

                    ControlPoint controlPoint = new ControlPoint(topLeftVertice, topRightVertice, bottomRightVertice, transform.position);

                    if (x % 2 == 0 && y % 2 == 0)
                        controlPoints.Add(controlPoint);

                    colors[bottomRightIndex].g = controlPoint.flatness;
                }
            }

            // Fill the holes
            for (int i = 0; i < colors.Length; i++)
            {
                int y = Mathf.FloorToInt((float)i / (float)colliderResolution);
                int x = i - y * colliderResolution;

                if (x == 0 && y == 0)
                    colors[i].g = colors[i + colliderResolution + 1].g;
                else if (x == 0)
                    colors[i].g = colors[i + 1].g;
                else if (y == 0)
                    colors[i].g = colors[i + colliderResolution].g;
            }

            this.controlPoints = controlPoints.ToArray();

            faceMesh.vertices = vertices;
            faceMesh.triangles = triangles;
            faceMesh.uv = uvs;
            faceMesh.normals = normals;
            faceMesh.colors = colors;

            meshCollider.sharedMesh = faceMesh;
            meshFilter.sharedMesh = faceMesh;
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

                    percent = SpherizeVector(new Vector2(x, y) / (textureSize - 1));


                    pointOnUnitCube = Planet.directions[faceIndex] + (percent.x - .5f) * 2 * axisA + (percent.y - .5f) * 2 * axisB;

                    vertices[i] = pointOnUnitCube.normalized;
                }
            }

            return vertices;
        }

        Vector2 SpherizeVector(Vector2 source)
        {
            // https://www.desmos.com/calculator/0ysgntuaov

            float x = source.x;
            float y = source.y;

            float sinX = (Mathf.Sin(Mathf.Deg2Rad * (x - 0.5f) * 180) + 1) / 2;
            float sinY = (Mathf.Sin(Mathf.Deg2Rad * (y - 0.5f) * 180) + 1) / 2;

            x = x + (x - sinX);
            y = y + (y - sinY);

            Vector2 spherizedPoint = new Vector2(x, y);

            return Vector2.Lerp(spherizedPoint, source, 0.5f);
        }
    }
}