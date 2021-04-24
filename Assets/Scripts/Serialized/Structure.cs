using System;
using UnityEngine;

[Serializable]
public class SerializedStructure
{

    public SerializedStructure()
    {

    }
    public Team team;
    public float position_x;
    public float position_y;
    public int initial_population;

    public int population_max;

    public string type;
}