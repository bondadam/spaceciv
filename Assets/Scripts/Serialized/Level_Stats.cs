using System;
using UnityEngine;

[Serializable]
public class Level_Stats
{
    public float time_taken;
    public int end_population;
    public int sent_spaceships;
    public int destroyed_enemy_spaceships;
    public int planets_conquered;
    public int planets_lost;

    public Level_Stats(){
        this.time_taken = 0;
        this.end_population = 0;
        this.sent_spaceships = 0;
        this.destroyed_enemy_spaceships = 0;
        this.planets_conquered = 0;
        this.planets_lost = 0;
    }
}
