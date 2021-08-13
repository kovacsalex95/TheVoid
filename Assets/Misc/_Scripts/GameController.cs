using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    [System.NonSerialized]
    public PlayerInput input = null;

    public TMPro.TextMeshProUGUI FpsCounter;

    World closestWorld = null;

    void Awake()
    {
        input = GetComponent<PlayerInput>();

        closestWorld = Util.getClosestWorld(Camera.main.transform.position);
    }

    private void Update()
    {
        if (FpsCounter)
            FpsCounter.SetText(string.Format("{0} FPS", Mathf.RoundToInt(1f / Time.deltaTime)));
    }
}
