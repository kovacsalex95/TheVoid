using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAgent : Agent
{
    [System.NonSerialized]
    public GameController controller = null;

    PlayerInput _input = null;
    public PlayerInput input
    {
        get
        {
            if (_input == null && controller != null)
                _input = controller.input;

            return _input;
        }
    }

    Camera playerCamera;

    float normalDistance;
    public float overViewDistance = -80f;
    float cameraDistanceTarget;

    float normalFov;
    public float overViewFov = 80f;
    float cameraFovTarget;

    float normalDegree;
    public float overViewCameraDegree = 90f;
    float cameraDegreeTarget;

    bool overviewMode = false;

    public override void AgentStart()
    {
        playerCamera = Camera.main;

        controller = Util.gameController();
        normalDistance = playerCamera.transform.localPosition.z;
        normalFov = playerCamera.fieldOfView;
        normalDegree = playerCamera.transform.parent.localEulerAngles.x;
    }
    public override void AgentBeforeUpdate()
    {
        Vector2 moveVector = input.actions["Move"].ReadValue<Vector2>();
        Move(moveVector);
    }

    public override void AgentUpdate()
    {
        overviewMode = input.actions["Map"].ReadValue<float>() > 0.1f;

        cameraDistanceTarget = overviewMode ? overViewDistance : normalDistance;
        cameraFovTarget = overviewMode ? overViewFov : normalFov;
        cameraDegreeTarget = overviewMode ? overViewCameraDegree : normalDegree;

        float velocity = 0;

        Vector3 cameraLocalPos = playerCamera.transform.localPosition;
        cameraLocalPos.z = Mathf.SmoothDamp(cameraLocalPos.z, cameraDistanceTarget, ref velocity, 0.2f);
        playerCamera.transform.localPosition = cameraLocalPos;

        velocity = 0;

        playerCamera.fieldOfView = Mathf.SmoothDamp(playerCamera.fieldOfView, cameraFovTarget, ref velocity, 0.2f);

        velocity = 0;
        Vector3 cameraLocalDir = playerCamera.transform.parent.localEulerAngles;
        cameraLocalDir.x = Mathf.SmoothDamp(cameraLocalDir.x, cameraDegreeTarget, ref velocity, 0.2f);
        playerCamera.transform.parent.localEulerAngles = cameraLocalDir;
    }
}
