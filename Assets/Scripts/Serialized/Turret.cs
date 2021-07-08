using System;
using UnityEngine;

[Serializable]
public class SerializedTurret
{

    public SerializedTurret()
    {
        this.radius = Game_Settings.DEFAULT_TURRET_RADIUS;
        this.is_protected = false; // by default not protected structure
    }
    public float radius;
    public Team team;
    public float position_x;
    public float position_y;
    public int initial_population;
    public int population_max;
    public bool is_protected;

}