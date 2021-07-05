using System;
using UnityEngine;

[Serializable]
public class Level
{
    public Level(){}

    public Level(SerializedBot[] bots, SerializedPlanet[] planets, SerializedTurret[] turrets, SerializedSpacegun[] spaceguns){
        
        this.bots = bots;

        this.planets = planets;
        this.turrets = turrets;
        this.spaceguns = spaceguns;
    }

    public Level_Difficulty difficulty;
    public Background_Color color;
    public SerializedPlanet[] planets;
    public SerializedSpacegun[] spaceguns;    
    public SerializedTurret[] turrets;
    public SerializedBot[] bots;

}

