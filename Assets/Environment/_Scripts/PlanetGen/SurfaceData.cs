using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lxkvcs.PlanetGen
{
    [CreateAssetMenu(), System.Serializable]
    public class SurfaceData : ScriptableObject
    {
        [SerializeField]
        public float radius = 10000;

        [SerializeField]
        public Vector2 heightRange = new Vector2(-0.1f, 0.1f);

        [SerializeField]
        public Vector2 remapRange = Vector2.up;

        [SerializeField]
        public PlanetLayer[] continents;
        [SerializeField]
        public PlanetLayer[] detail;
        [SerializeField]
        public PlanetLayer[] underwater;
    }

    [System.Serializable]
    public class PlanetLayer
    {
        public string name;
        public bool enabled = true;
        [Range(0.2f, 10f)]
        public float scale = 1f;
        [Range(0.5f, 10f)]
        public float smoothness = 1f;
        [Range(1f, 10f)]
        public float ratio = 1f;

        public PlanetLayerStruct Struct()
        {
            PlanetLayerStruct data = new PlanetLayerStruct();

            data.scale = this.scale / 100f;
            data.smoothness = this.smoothness;
            data.ratio = this.ratio;

            return data;
        }

        public static int stride => sizeof(float) * 3;
    }

    public struct PlanetLayerStruct
    {
        public float scale;
        public float smoothness;
        public float ratio;
    }
}
