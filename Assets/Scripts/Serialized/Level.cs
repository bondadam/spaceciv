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
        this.record_time = 60;
    }

    public Level_Difficulty difficulty;
    public Background_Color color;
    public SerializedPlanet[] planets;
    public SerializedSpacegun[] spaceguns;    
    public SerializedTurret[] turrets; 
    public SerializedSpaceEntity[] suns;
    public SerializedSpaceEntity[] frozenvoids;
    public SerializedBot[] bots;
    public float spaceship_speed;
    public float record_time;
    public int tutorial;

}

