using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Agent : MonoBehaviour
{
    GameController controller = null;

    public bool snapOnStart = true;
    public bool snapOnUpdate = true;

    public float rootYOffset = 0f;

    public float movementSpeedMS = 9f;

    [System.NonSerialized]
    public World assignedWorld = null;

    [System.NonSerialized]
    public Transform rootTransform = null;

    [System.NonSerialized]
    public Transform rotationTransform = null;

    [System.NonSerialized]
    public Transform yOffsetTransform = null;

    Transform rayCasterTransform = null;

    Vector3 oldPosition = Vector3.zero;

    LayerMask floorMask;

    void Awake()
    {
        controller = Util.gameController();
        floorMask = Util.agentFloorMask();

        AssignClosestWorld();

        if (assignedWorld == null) {
            Debug.LogError("World not found!");
            return;
        }

        CreateTransformHierarchy();

        if (snapOnStart)
            SnapToWorld();

        AgentStart();
    }

    void AssignClosestWorld()
    {
        assignedWorld = Util.getClosestWorld(transform.position);
    }

    void CreateTransformHierarchy()
    {
        GameObject rootObject = new GameObject(string.Format("AgentRoot_{0}", gameObject.name));
        rootObject.transform.position = transform.position;
        rootObject.transform.parent = assignedWorld.transform;
        rootObject.transform.LookAt(assignedWorld.transform.position);

        GameObject orientationObject = new GameObject(string.Format("AgentOrientation_{0}", gameObject.name));
        orientationObject.transform.parent = rootObject.transform;
        orientationObject.transform.localEulerAngles = Vector3.left * 90f;
        orientationObject.transform.localPosition = Vector3.back * rootYOffset;
        orientationObject.transform.localScale = Vector3.one;

        GameObject rotationObject = new GameObject(string.Format("AgentRotation_{0}", gameObject.name));
        rotationObject.transform.parent = orientationObject.transform;
        rotationObject.transform.localPosition = Vector3.zero;
        rotationObject.transform.localEulerAngles = Vector3.zero;
        rotationObject.transform.localScale = Vector3.one;

        GameObject yOffsetObject = new GameObject(string.Format("AgentYOffset_{0}", gameObject.name));
        yOffsetObject.transform.parent = rotationObject.transform;
        yOffsetObject.transform.localPosition = Vector3.zero;
        yOffsetObject.transform.localEulerAngles = Vector3.zero;
        yOffsetObject.transform.localScale = Vector3.one;

        transform.parent = yOffsetObject.transform;
        transform.localEulerAngles = Vector3.zero;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;

        gameObject.layer = LayerMask.NameToLayer("Agents");


        GameObject rayCasterObject = new GameObject(string.Format("AgentRaycaster_{0}", gameObject.name));
        rayCasterObject.transform.parent = orientationObject.transform;
        rayCasterObject.transform.localEulerAngles = Vector3.zero;
        rayCasterObject.transform.localPosition = new Vector3(0, assignedWorld.getPlanet().surfaceSettings.radius, 0);
        rayCasterTransform = rayCasterObject.transform;


        rootTransform = rootObject.transform;
        rotationTransform = rotationObject.transform;
        yOffsetTransform = yOffsetObject.transform;
    }

    void SnapToWorld()
    {
        RaycastHit groundHit;
        if (Physics.Raycast(rayCasterTransform.position, assignedWorld.transform.position - rayCasterTransform.position, out groundHit, assignedWorld.getPlanet().surfaceSettings.radius * 2, floorMask))
        {
            Vector3 difference = rootTransform.InverseTransformPoint(groundHit.point);
            rootTransform.Translate(difference);
        }
        oldPosition = transform.position;
    }

    void LookAtWorld()
    {
        rootTransform.LookAt(assignedWorld.transform.position);
    }

    void Update()
    {
        if (assignedWorld == null)
            return;

        LookAtWorld();

        AgentBeforeUpdate();

        if (snapOnUpdate && oldPosition != transform.position)
            SnapToWorld();

        AgentUpdate();
    }

    public void Move(Vector2 moveVector)
    {
        Vector3 moveVector3 = new Vector3(moveVector.x, 0, moveVector.y);

        moveVector3 *= Time.deltaTime * 10f * movementSpeedMS;

        Vector3 playerMoveVector = rayCasterTransform.TransformVector(moveVector3);
        Vector3 rootVector3 = rootTransform.InverseTransformVector(playerMoveVector);

        rootTransform.Translate(rootVector3);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (assignedWorld == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(rayCasterTransform.position, assignedWorld.transform.position);
    }
#endif

    public abstract void AgentStart();

    public abstract void AgentBeforeUpdate();

    public abstract void AgentUpdate();
}
