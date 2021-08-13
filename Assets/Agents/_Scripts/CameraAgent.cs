using Assets.Misc._Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraAgent : Agent
{
    const float ANIMATION_TIME = 0.05f;

    public float minDistance = 10f;
    public float maxDistanceRatio = 1f;

    [Range(0f, 90f)]
    public float minFov = 65f;
    [Range(0f, 90f)]
    public float maxFov = 90f;

    [Range(0f, 1f)]
    public float distance = 0.5f;

    new Camera camera;

    float maxDistance => assignedWorldRadius * maxDistanceRatio;
    float oldDistance = 0f;

    float targetDistance => Mathf.Lerp(minDistance, maxDistance, distance * distance);
    float currentDistance;

    float targetFov => Mathf.Lerp(minFov, maxFov, distance * distance);
    float currentFov;


    public override void AgentBeforeStart()
    {
        currentDistance = targetDistance;
        currentFov = targetFov;
        rootDirection = new Vector3(90, 0, 0);
        camera = GetComponent<Camera>();
    }

    public override void AgentStart()
    {
        
    }
    public override void AgentBeforeUpdate()
    {
        UpdateCameraDistance();
    }

    public override void AgentUpdate()
    {
        UpdateCameraPosition();
    }

    private void UpdateCameraDistance()
    {
        if (distance == oldDistance && currentDistance == targetDistance && currentFov == targetFov)
            return;

        oldDistance = distance;

        currentDistance = CalcUtil.SmoothDamp(currentDistance, targetDistance, ANIMATION_TIME, 0.01f);
        SetOffsetY(currentDistance);

        currentFov = CalcUtil.SmoothDamp(currentFov, targetFov, ANIMATION_TIME, 0.01f);
        camera.fieldOfView = currentFov;
    }

    private void UpdateCameraPosition()
    {

    }
}
