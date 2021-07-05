using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode()]
public class PlanetAtmosphereScaler : MonoBehaviour
{
    lxkvcs.PlanetGen.Planet _planet = null;
    lxkvcs.PlanetGen.Planet planet
    {
        get
        {
            if (_planet == null)
                _planet = transform.parent.gameObject.GetComponentInChildren<lxkvcs.PlanetGen.Planet>();

            return _planet;
        }
    }

    MeshRenderer _renderer = null;

    new MeshRenderer renderer
    {
        get
        {
            if (_renderer == null)
                _renderer = GetComponent<MeshRenderer>();

            return _renderer;
        }
    }

    [Range(1f, 2f)]
    public float atmosphereScale = 1.3f;

    public bool showRenderer = false;

    void Start()
    {
        ScaleAtmo();
        renderer.enabled = true;
    }

    private void OnEnable()
    {
        Camera.main.depthTextureMode = DepthTextureMode.Depth;
    }

    void ScaleAtmo()
    {
        lxkvcs.PlanetGen.Planet p = planet;
        if (p == null)
            return;

        transform.localScale = (p.surfaceSettings.radius * atmosphereScale) * Vector3.one * 2f;
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Application.isEditor)
            ScaleAtmo();

        renderer.enabled = Application.isPlaying || showRenderer;
#endif
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (renderer.enabled)
            return;

        Gizmos.color = new Color(1f, 1f, 0f, 0.1f);
        Gizmos.DrawSphere(transform.position, transform.localScale.x * 0.5f);
    }
#endif
}
