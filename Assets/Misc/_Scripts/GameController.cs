using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class GameController : MonoBehaviour
{
    public PlayerInput input { get; private set; }

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
    }
}
