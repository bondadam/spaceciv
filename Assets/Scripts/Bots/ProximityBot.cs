using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ProximityBot : Bot
{
    public override void make_Move(Game_State game_State){
        (List<Planet> my_planets, List<Planet> enemy_planets) planets = this.separate_planets(game_State.planets);
        bool move_chosen = false;
        List<Planet> my_planets = (from p in planets.my_planets orderby p.get_population() descending select p).ToList();
        foreach (Planet p in my_planets){
            List<Planet> enemy_planets = (from ep in planets.enemy_planets orderby ep.distance_from(p.transform.position) select ep).ToList();
            //enemy_planets.Shuffle();
            foreach (Planet ep in enemy_planets){
                if (!move_chosen && (p.get_population() > ep.get_population() || p.get_population()>=p.population_max)){
                    move_chosen = true;
                    this.level_Manager.send_spaceship_to_planet_bot(p, ep, p.get_population());
                }
            }
            if (p.can_upgrade()){
                p.upgrade();
            }
        }

    }

    public override void CustomInitialize(){
        //this.set_update_frequency(5.0f);
    }
}
