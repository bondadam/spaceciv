using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlitzBot : Bot
{
    public override void make_Move(Game_State game_State){
        (List<Planet> my_planets, List<Planet> enemy_planets) planets = this.separate_planets(game_State.planets);
        bool move_chosen = false;
        planets.my_planets.Shuffle();
        planets.enemy_planets.Shuffle();
        foreach (Planet p in planets.my_planets){
            foreach (Planet ep in planets.enemy_planets){
                if (!move_chosen && p.get_population() > ep.get_population()){
                    move_chosen = true;
                    //Debug.Log("Move chosen. From planet " + p.ToString() + " to planet " + ep.ToString());
                    this.level_Manager.send_spaceship_to_planet_bot(p, ep, p.get_population());
                }
            }
        }
    }

    public override void CustomInitialize(){
        //this.set_update_frequency(5.0f);
    }
}
