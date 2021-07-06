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

    World closestWorld = null;
    float closestWorldRadius = 0;

    void Awake()
    {
        input = GetComponent<PlayerInput>();
        closestWorld = Util.getClosestWorld(Camera.main.transform.position);

        if (closestWorld)
        {
            closestWorldRadius = closestWorld.getAtmosphereRadius();
            if (skyboxMaterial)
                skyboxMaterial.SetFloat("_AtmoRadius", closestWorldRadius);
        }
    }

    private void Update()
    {
        if (FpsCounter)
        {
            FpsCounter.SetText(string.Format("{0} FPS", Mathf.RoundToInt(1f / Time.deltaTime)));
        }

        if (skyboxMaterial && closestWorld)
        {
            float distanceToWorld = Vector3.Distance(Camera.main.transform.position, closestWorld.transform.position);
            skyboxMaterial.SetFloat("_DistanceToWorld", distanceToWorld);
        }
    }

    private void OnApplicationQuit()
    {
        if (skyboxMaterial)
        {
            skyboxMaterial.SetFloat("_DistanceToWorld", 100);
            skyboxMaterial.SetFloat("_AtmoRadius", 1);
        }
    }
}
