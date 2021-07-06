using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using lxkvcs.PlanetGen;

public class World : MonoBehaviour
{
    Planet planet = null;
    PlanetAtmosphereScaler atmosphere = null;

    public Planet getPlanet()
    {
        if (planet == null)
            planet = gameObject.GetComponentInChildren<Planet>();

        return planet;
    }

    public PlanetAtmosphereScaler getAtmosphere()
    {
        if (atmosphere == null)
            atmosphere = gameObject.GetComponentInChildren<PlanetAtmosphereScaler>();

        return atmosphere;
    }

    public float getAtmosphereRadius()
    {
        PlanetAtmosphereScaler atmo = getAtmosphere();
        if (atmo == null)
            return 0;

        return atmo.transform.localScale.x * 0.5f;
    }
}
