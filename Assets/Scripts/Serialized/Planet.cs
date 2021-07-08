using System;
using UnityEngine;

[Serializable]
public class SerializedPlanet
{

    public SerializedPlanet()
    {
        //this.is_protected = false;  // by default not protected planet
    }

    public float planet_size;
    public Team team;
    public float position_x;
    public float position_y;
    public int initial_population;
    public int population_max;
    public bool is_protected;
}