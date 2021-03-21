using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveBot : Bot
{
    // Start is called before the first frame update
    public override void make_Move(Game_State game_State){
        (List<Planet> my_planets, List<Planet> enemy_planets) planets = this.separate_planets(game_State.planets);
        planets.my_planets.Shuffle();
        planets.enemy_planets.Shuffle();

        (List<Spaceship> my_spaceships, List<Spaceship> enemy_spaceships) spaceships = this.separate_spaceships(game_State.spaceships);
        planets.my_planets.Shuffle();
        planets.enemy_planets.Shuffle();

        bool move_chosen = false;

        bool defend = Random.value > 0.25; // 75% chance de défendre au lieu d'attaquer
        if (defend){
            foreach (Spaceship es in spaceships.enemy_spaceships){
                if (!move_chosen){
                    foreach (Planet p in planets.my_planets){
                        if (!move_chosen){
                            if (es.get_target() == p){
                                move_chosen = true;
                                this.level_Manager.send_spaceship_to_planet_bot(p, es.get_source(), p.get_population());
                            }
                        } else {
                            int reinforcements = Mathf.FloorToInt(p.get_population() / 2.0f);
                            this.level_Manager.send_spaceship_to_planet_bot(p, es.get_target(), reinforcements);
                        }
                    }
                }
            }
        } else {
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
    }
}
