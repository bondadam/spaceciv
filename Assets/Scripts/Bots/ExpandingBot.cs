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

            // remove neutral planets to which I have already sent a colonizing force
            List<Planet> planets_to_remove_from_canidates = new List<Planet>();
            foreach(Planet pl in candidate_planets)
            {
                //bool can_be_skipped = false; // can be skipped if enough forces are underway
                int incoming_army = 0;
                foreach(Spaceship p in spaceships.my_spaceships){
                    if(p.get_target().Equals(pl)){
                        incoming_army += p.get_population();
                    }
                //  candidate_planets = candidate_planets.Where(planet => !planet.Equals(p.get_target())).ToList();
                }
               // if(incoming_army > pl.get_population()) can_be_skipped = true;
                if(incoming_army > pl.get_population())
                {
                    planets_to_remove_from_canidates.Add(pl);
                }
            }
            candidate_planets = candidate_planets.Except(planets_to_remove_from_canidates).ToList();
            //candidate_planets = candidate_planets.Where(planet => !planet.Equals(pl)).ToList();
            List<Vector2> enemy_planet_positions = (from p in planets.enemy_planets select p.get_position()).ToList();
            List<float> enemy_planet_sizes = (from p in planets.enemy_planets select p.get_planet_size()).ToList();
            Vector2 enemy_center_point = Utils.find_center_of_weighted_points(enemy_planet_positions, enemy_planet_sizes);
            
            List<Vector2> my_planet_positions = (from p in planets.my_planets select p.get_position()).ToList();
            List<float> my_planet_sizes = (from p in planets.my_planets select p.get_planet_size()).ToList();
            Vector2 my_center_point = Utils.find_center_of_weighted_points(my_planet_positions, my_planet_sizes);
        
            if(candidate_planets.Count > 0 )
            {
                List<(Planet, float)> my_planet_dists_from_target = new List<(Planet, float)>();
                Planet target_planet = candidate_planets[0];
                double min_dist =  double.MaxValue; 

                foreach(Planet p in candidate_planets){
                    float my_x = my_center_point[0];
                    float my_y = my_center_point[1];
                    double score = 0;
                    double dist = Mathf.Sqrt(Mathf.Pow(my_y - p.transform.position.y,2) + Mathf.Pow(my_x - p.transform.position.x,2)) + p.get_population()/15;
                    score -= dist;
                    float enemy_x = enemy_center_point[0];
                    float enemy_y = enemy_center_point[1];
                    double dist_from_enemy = Mathf.Sqrt(Mathf.Pow(enemy_y - p.transform.position.y,2) + Mathf.Pow(enemy_x - p.transform.position.x,2)) + p.get_population()/15;
                    score += dist;
                    if(dist < min_dist){ min_dist = dist; target_planet = p;}
                }
                foreach(Planet my_planet in planets.my_planets)
                {
                    float my_x = my_planet.transform.position.x;
                    float my_y = my_planet.transform.position.y;
                    float dist = Mathf.Sqrt(Mathf.Pow(my_y - target_planet.transform.position.y,2) + Mathf.Pow(my_x - target_planet.transform.position.x,2));
                    my_planet_dists_from_target.Add((my_planet,dist));
                }
                my_planet_dists_from_target.Sort((t1, t2) => t1.Item2.CompareTo(t2.Item2)); // sort planets based on distance from target
                int army_count = 0;
                foreach(Spaceship sp in spaceships.my_spaceships)
                {
                    if(sp.get_target().Equals(target_planet))
                    {
                        army_count += sp.get_population();
                    }
                }
                Planet sending_planet = null;
                if(army_count <= target_planet.get_population())
                {
                    bool loop_has_ended = false;
                    foreach((Planet,float) limit_planet in my_planet_dists_from_target)
                    {
                        if(loop_has_ended) break;
                        int army_count_from_limit_planet = army_count;
                        foreach((Planet,float) my_planet in my_planet_dists_from_target)
                        {
                            double extra_distance = limit_planet.Item2 - my_planet.Item2;
                            double population_growth_rate = my_planet.Item1.get_growth_factor()*100;
                            int extra_population = (int) Mathf.Floor(((float) extra_distance/((float) Game_Settings.BASE_SPACESHIP_SPEED) * (float) population_growth_rate));
                            army_count_from_limit_planet += my_planet.Item1.get_population() + extra_population;
                            if(army_count_from_limit_planet > target_planet.get_population())
                            {
                                bool already_sent = false;
                                foreach(Spaceship sp in spaceships.my_spaceships)
                                {
                                    if(sp.get_target().Equals(target_planet) && sp.get_source().Equals(limit_planet.Item1))
                                    {
                                        already_sent = true;
                                    }
                                }
                                if(!already_sent){
                                    sending_planet = limit_planet.Item1;
                                    loop_has_ended = true;
                                }
                                break;
                            }
                            if(my_planet.Equals(limit_planet)) break; // we don't want to look past the limit planet in this inner loop
                        }
                    }
                }
                
                if(sending_planet != null){
                    // this next line is not needed anymore, because only one target neutral planet is considered per turn
                    candidate_planets = candidate_planets.Where(planet => !planet.Equals(target_planet)).ToList();
                    this.level_Manager.send_spaceship_to_planet_bot(sending_planet, target_planet, sending_planet.get_population());
                }
            }                 

        }else{
            foreach (Planet p in planets.my_planets){
                foreach (Planet ep in planets.enemy_planets){
                    if (!move_chosen && (p.get_population() > ep.get_population() || p.get_population()==99)){
                        move_chosen = true;
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
