using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStatsKeeper : MonoBehaviour
{
    private static Level_Stats level_stats;

    public void Awake()
    {
        LevelStatsKeeper.level_stats = new Level_Stats();
    }

    public static void planet_conquered()
    {
        LevelStatsKeeper.level_stats.planets_conquered++;
    }
    public static void planet_lost()
    {
        LevelStatsKeeper.level_stats.planets_lost++;
    }

    public void set_end_population(int end_population){
        LevelStatsKeeper.level_stats.end_population = end_population;
    }

    public int get_end_population(){
        return LevelStatsKeeper.level_stats.end_population;
    }

    public static void sent_spaceship()
    {
        LevelStatsKeeper.level_stats.sent_spaceships++;
    }
    public static void spaceship_destroyed()
    {
        LevelStatsKeeper.level_stats.destroyed_enemy_spaceships++;
    }
    public static void set_timer(float time_taken)
    {
        LevelStatsKeeper.level_stats.time_taken = time_taken;
    }

    public static float get_timer()
    {
        Debug.Log("Destroyed enemy spaceships : " + LevelStatsKeeper.level_stats.destroyed_enemy_spaceships);
        Debug.Log("Sent Spaceships : " + LevelStatsKeeper.level_stats.sent_spaceships);
        Debug.Log("End Population : " + LevelStatsKeeper.level_stats.end_population);
        Debug.Log("Planets Conquered : " + LevelStatsKeeper.level_stats.planets_conquered);
        Debug.Log("Planets Lost : " + LevelStatsKeeper.level_stats.planets_lost);
        return LevelStatsKeeper.level_stats.time_taken;
    }
    public static void get_planet_conquered()
    {
        LevelStatsKeeper.level_stats.planets_conquered++;
    }

    public static void get_planet_lost()
    {
        LevelStatsKeeper.level_stats.planets_lost++;
    }

}
