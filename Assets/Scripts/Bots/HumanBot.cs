using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HumanBot : Bot
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

        bool defend = Random.value > 0.95; 
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
                        if (p.can_upgrade()){
                            p.upgrade();
                        }
                    }
                }
            }
        } else {
            // ExpandingBot will only attack enemy planets if there are no more neutral planets left to conquer
            List<Planet> neutral_planets = (from p in planets.enemy_planets where p.team == Team.Neutral select p).ToList();
            if(neutral_planets.Count > 0){
                Planet safest_planet = planets.my_planets[0];
                Planet sender_planet = planets.my_planets[0];
                double max_dist = 0; // find my planet which is farthest from enemies
                foreach(Planet p in planets.my_planets){
                    if(p.get_population() > 80){ sender_planet = p; break;}
                    double cum_dist = 0;
                    foreach(Planet ep in planets.enemy_planets){
                        cum_dist += Mathf.Sqrt(Mathf.Pow(ep.transform.position.y - p.transform.position.y,2)+ Mathf.Pow(ep.transform.position.x - p.transform.position.x,2));
                    }

                    if(cum_dist > max_dist){ max_dist = cum_dist; safest_planet = p;}
                }
                Planet target_planet = neutral_planets[0];
                double min_dist =  double.MaxValue; // find closest neutral planet
                foreach(Planet p in neutral_planets){
                    double dist = Mathf.Sqrt(Mathf.Pow(safest_planet.transform.position.y - p.transform.position.y,2) + Mathf.Pow(safest_planet.transform.position.x - p.transform.position.x,2)) + p.get_population()/15;

                    if(dist < min_dist){ min_dist = dist; target_planet = p;}
                }
                if(safest_planet.get_population() > target_planet.get_population()){
                    move_chosen = true;
                    this.level_Manager.send_spaceship_to_planet_bot(safest_planet, target_planet, safest_planet.get_population());
                }else if(sender_planet.get_population() > target_planet.get_population()){
                    move_chosen = true;
                    this.level_Manager.send_spaceship_to_planet_bot(sender_planet, target_planet, target_planet.get_population());
                }
            }else{
                foreach (Planet p in planets.my_planets){
                    foreach (Planet ep in planets.enemy_planets){
                        if (!move_chosen && (p.get_population() > ep.get_population() || p.get_population()==99)){
                            move_chosen = true;
                            //Debug.Log("Move chosen. From planet " + p.ToString() + " to planet " + ep.ToString());
                            this.level_Manager.send_spaceship_to_planet_bot(p, ep, p.get_population());
                        }
                    }
                    if (p.can_upgrade()){
                        p.upgrade();
                    }
                }
            }
        }
    }
}
