using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class GameController : MonoBehaviour
{
    public PlayerInput input { get; private set; }

    public GamePhase Phase { get; private set; }
    public GameMode Mode { get; private set; }

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        Phase = GamePhase.WorldSelection;
        Mode = GameMode.Management;
    }

    public enum GamePhase
    {
        WorldSelection = 0,
        StartintPoint = 1,
        Ingame = 2
    }

    public enum GameMode
    {
        Management = 0,
        Building = 1,
        Mission = 2
    }
}
