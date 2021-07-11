using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class JuggernautBot : Bot
{
    public override void make_Move(Game_State game_State){
        (List<Planet> my_planets, List<Planet> enemy_planets) planets = this.separate_planets(game_State.planets);
        bool move_chosen = false;
        planets.my_planets.Shuffle();
        planets.enemy_planets.Shuffle();
        List<Planet> enemy_planets = (from p in planets.enemy_planets where p.team != Team.Neutral select p).ToList();
        List<Planet> protected_enemy_planets = (from p in planets.enemy_planets where p.is_protected == true select p).ToList();
        if(protected_enemy_planets.Count > 0){ enemy_planets = protected_enemy_planets; } // if protected planets exist, i will attack only them
        enemy_planets.Shuffle();
        //List<Structure> enemy_planets = (enemy_planets.ConvertAll(x => (Structure)x));
        foreach (Planet p in planets.my_planets){
            foreach (Planet ep in enemy_planets){
                if (!move_chosen && (p.get_population() > ep.get_population()  || p.get_population()>=p.population_max)){
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
