using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    [System.NonSerialized]
    public PlayerInput input = null;

    public TMPro.TextMeshProUGUI FpsCounter;
    public Material skyboxMaterial;

    void Awake()
    {
        input = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (FpsCounter)
        {
            FpsCounter.SetText(string.Format("{0} FPS", Mathf.RoundToInt(1f / Time.deltaTime)));
        }



        if (skyboxMaterial)
        {
            Vector3 worldPosition = Camera.main.WorldToScreenPoint(Vector3.zero);
            Vector2 worldPositionRatio;
            worldPositionRatio.x = worldPosition.x / (float)Screen.width;
            worldPositionRatio.y = worldPosition.y / (float)Screen.height;
            skyboxMaterial.SetVector("_WorldPosition", worldPositionRatio);
        }
    }

    private void OnApplicationQuit()
    {
        if (skyboxMaterial)
            skyboxMaterial.SetVector("_WorldPosition", new Vector2(0.5f, 0.5f));
    }
}
