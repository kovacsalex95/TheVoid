using Assets.Misc._Scripts;
using Assets.UI._Scripts.Misc;
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

    public float distanceStep = 10;

    public float minSpeed = 2f;
    public float maxSpeed = 30f;

    new Camera camera;

    float maxDistance => assignedWorldRadius * maxDistanceRatio;
    float oldDistance = 0f;

    float targetDistance => Mathf.Lerp(minDistance, maxDistance, distance * distance);
    float currentDistance;

    float targetFov => Mathf.Lerp(minFov, maxFov, distance * distance);
    float currentFov;

    bool mouseRight = false;
    Vector2 mouseDelta = Vector2.zero;
    float mouseScroll = 0;

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
        UpdateInput();
        UpdateCameraDistance();
    }

    public override void AgentUpdate()
    {
        UpdateCameraPosition();
    }

    private void UpdateCameraDistance()
    {
        distance = Mathf.Clamp01(distance + mouseScroll * (1f / distanceStep));

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
        movementSpeed = Mathf.Lerp(minSpeed, maxSpeed, distance * distance);

        if (mouseRight && mouseDelta.magnitude > 0)
            Move(-mouseDelta, true, 20);
    }

    private void UpdateInput()
    {
        if (!UIUtil.Pointers.MouseOverUI)
        {
            mouseRight = controller.input.actions["Mouse Right Button"].ReadValue<float>() != 0;
            mouseDelta = controller.input.actions["Mouse Movement"].ReadValue<Vector2>();
            mouseScroll = -CalcUtil.Normalize(controller.input.actions["Mouse Scroll"].ReadValue<float>());
        }
        else
        {
            mouseRight = false;
            mouseDelta = Vector2.zero;
            mouseScroll = 0;
        }
    }
}
