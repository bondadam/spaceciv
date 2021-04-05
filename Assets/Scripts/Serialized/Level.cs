using System;
using UnityEngine;

[Serializable]
public class Level
{
    public Level(){

    }

    public Level(SerializedPlanet[] planets, SerializedBot[] bots){
        this.planets = planets;
        this.bots = bots;
    }

    public SerializedPlanet[] planets;
    public SerializedBot[] bots;

}

