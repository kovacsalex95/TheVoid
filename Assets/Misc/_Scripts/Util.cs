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

    public static World getClosestWorld(Vector3 position)
    {
        World result = null;
        float closestDistance = Mathf.Infinity;

        World[] worlds = FindObjectsOfType<World>();
        foreach (World world in worlds)
        {
            float distanceToWorld = Vector3.Distance(position, world.transform.position);
            if (distanceToWorld < closestDistance)
            {
                closestDistance = distanceToWorld;
                result = world;
            }
        }

        return result;
    }

    public static lxkvcs.PlanetGen.Planet getClosestPlanet(Vector3 position)
    {
        World closestWorld = getClosestWorld(position);
        if (closestWorld == null)
            return null;

        return closestWorld.getPlanet();
    }
}
