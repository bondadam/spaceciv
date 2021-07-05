using System;
using UnityEngine;

[Serializable]
public class SerializedSpacegun {

    public SerializedSpacegun()
    {
        this.radius = Game_Settings.DEFAULT_SPACEGUN_RADIUS;;
    }
    public float radius;
    public Team team;
    public float position_x;
    public float position_y;
    public int initial_population;
    public int population_max;

}