using System;
using UnityEngine;

[Serializable]
public class SerializedSpacegun
{

    public SerializedSpacegun()
    {

    }
    public Team team;
    public float position_x;
    public float position_y;
    public int initial_population;
    public float radius = Game_Settings.DEFAULT_SPACEGUN_RADIUS;

    public int population_max;

}