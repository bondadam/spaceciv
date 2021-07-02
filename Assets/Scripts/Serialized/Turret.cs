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
    public float radius = Game_Settings.DEFAULT_TURRET_RADIUS;

    public int population_max;

}