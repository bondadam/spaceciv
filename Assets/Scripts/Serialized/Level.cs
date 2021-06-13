using System;
using UnityEngine;

[Serializable]
public class Level
{
    public Level(){

    }

    public Level(SerializedPlanet[] planets, SerializedBot[] bots, SerializedTurret[] turrets){
        this.planets = planets;
        this.bots = bots;
        this.turrets = turrets;
    }

    public SerializedPlanet[] planets;
    public SerializedSpacegun[] spaceguns;    
    public SerializedTurret[] turrets;
    public SerializedBot[] bots;

}

