using System;
using UnityEngine;

[Serializable]
public class Level
{
    public SerializedPlanet[] planets;
    public SerializedBot[] bots;
    
}


[Serializable]
public class SerializedPlanet {
    public Team team;
    public float position_x;
    public float position_y;
    public int initial_population;

    public int population_max;

    public float planet_size;
}

[Serializable]
public class SerializedBot{
    public Team team;
    public String type;
    public float decision_interval;
}