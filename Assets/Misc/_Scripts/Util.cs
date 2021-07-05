using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour
{
    // HELPER CLASS
    static UtilHelper _helper = null;
    static UtilHelper helper
    {
        get
        {
            if (_helper == null)
                _helper = FindObjectOfType<UtilHelper>();

            return _helper;
        }
    }

    public static GameController gameController()
    {
        return FindObjectOfType<GameController>();
    }

    public static LayerMask agentFloorMask()
    {
        return helper.agentFloorLayers;
    }
}
