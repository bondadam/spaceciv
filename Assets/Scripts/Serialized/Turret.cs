using System;
using UnityEngine;

[Serializable]
public class SerializedTurret
{

    public SerializedTurret()
    {

    }
    public Team team;
    public float position_x;
    public float position_y;
    public int initial_population;

    public int population_max;

}