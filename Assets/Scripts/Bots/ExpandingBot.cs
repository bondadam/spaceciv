using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ExpandingBot : Bot
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

        // ExpandingBot will only attack enemy planets if there are no more neutral planets left to conquer
        List<Planet> neutral_planets = (from p in planets.enemy_planets where p.team == Team.Neutral select p).ToList();
        if(neutral_planets.Count > 0){
            List<Planet> candidate_planets = (from p in neutral_planets select p).ToList();
            foreach(Spaceship p in spaceships.my_spaceships){
                candidate_planets = candidate_planets.Where(planet => planet != p.get_target()).ToList();
            }
            foreach(Planet pl in planets.my_planets){
                Planet target_planet = neutral_planets[0];
                double min_dist =  double.MaxValue; // find closest neutral planet
                foreach(Planet p in neutral_planets){
                    double dist = Mathf.Sqrt(Mathf.Pow(pl.transform.position.y - p.transform.position.y,2) + Mathf.Pow(pl.transform.position.x - p.transform.position.x,2)) + p.get_population()/15;

                    if(dist < min_dist){ min_dist = dist; target_planet = p;}
                }
                if(pl.get_population() > target_planet.get_population()){
                   // move_chosen = true;
                    candidate_planets = candidate_planets.Where(planet => planet != target_planet).ToList();
                    this.level_Manager.send_spaceship_to_planet_bot(pl, target_planet, pl.get_population());
                }
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
