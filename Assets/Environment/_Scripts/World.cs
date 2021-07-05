using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using lxkvcs.PlanetGen;

public class World : MonoBehaviour
{
    Planet planet = null;

    public Planet getPlanet()
    {
        if (planet == null)
            planet = gameObject.GetComponentInChildren<Planet>();

        return planet;
    }
}
