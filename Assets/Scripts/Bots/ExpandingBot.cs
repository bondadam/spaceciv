using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ExpandingBot : Bot
{
    // Start is called before the first frame update
    public override void make_Move(Game_State game_State){
        (List<Planet> my_planets2, List<Planet> enemy_planets2) planets = this.separate_planets(game_State.planets);
        List<Structure> turrets = game_State.turrets.ConvertAll(x => (Structure) x);
        List<Planet> my_planets = planets.my_planets2;
        List<Structure> enemy_planets = (planets.enemy_planets2.ConvertAll(x => (Structure)x));
        //enemy_planets.AddRange(turrets);
        foreach(Turret turret in turrets)
        {
            if(turret.team != this.team)
            {
                enemy_planets.Add(turret);
            }
        }

        (List<Spaceship> my_spaceships, List<Spaceship> enemy_spaceships) spaceships = this.separate_spaceships(game_State.spaceships);

        bool move_chosen = false;

        // ExpandingBot will only attack enemy planets if there are no more neutral planets left to conquer

        List<Structure> neutral_planets = (from p in enemy_planets where p.team == Team.Neutral select p).ToList();

        enemy_planets = (from p in enemy_planets where (p.team != this.team && p.team != Team.Neutral) select p).ToList();
        foreach(Structure s in enemy_planets)
        {
            Debug.Log("enemy planet at "+(s.get_position().x).ToString() + ", "+(s.get_position().y).ToString());
        }
        if(neutral_planets.Count > 0){

            List<Structure> candidate_planets = (from p in neutral_planets select p).ToList();

            // remove neutral planets to which I have already sent a colonizing force
            List<Structure> planets_to_remove_from_canidates = new List<Structure>();
            foreach(Structure pl in candidate_planets)
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
            List<Vector2> enemy_planet_positions = (from p in enemy_planets select p.get_position()).ToList();
            List<float> enemy_planet_sizes = (from p in enemy_planets select p.get_planet_size()).ToList();
            Vector2 enemy_center_point = Utils.find_center_of_weighted_points(enemy_planet_positions, enemy_planet_sizes);
            
            List<Vector2> my_planet_positions = (from p in my_planets select p.get_position()).ToList();
            List<float> my_planet_sizes = (from p in my_planets select p.get_planet_size()).ToList();
            Vector2 my_center_point = Utils.find_center_of_weighted_points(my_planet_positions, my_planet_sizes);
            Debug.Log("enemy center is ");
            Debug.Log(enemy_center_point);
            Debug.Log("my center is ");
            Debug.Log(my_center_point);
            //Debug.Log("enemy center is "+(enemy_center_point.x).ToString() + ", "+(enemy_center_point.y).ToString());
            if(candidate_planets.Count > 0 )
            {
                Structure target_planet = candidate_planets[0];
                double max_score =  double.MinValue; 

                foreach(Structure p in candidate_planets){
                    float my_x = my_center_point[0];
                    float my_y = my_center_point[1];
                    double score = 0;
                    double dist = Mathf.Sqrt(Mathf.Pow(my_y - p.transform.position.y,2) + Mathf.Pow(my_x - p.transform.position.x,2)) /*+ p.get_population()/15*/;
                    score -= dist;
                    float enemy_x = enemy_center_point[0];
                    float enemy_y = enemy_center_point[1];
                    double dist_from_enemy = Mathf.Sqrt(Mathf.Pow(enemy_y - p.transform.position.y,2) + Mathf.Pow(enemy_x - p.transform.position.x,2));
                    score += dist_from_enemy;
                    if(score > max_score){ max_score = score; target_planet = p;}
                }



                List<(Planet, float)> my_planet_dists_from_target = new List<(Planet, float)>();
                foreach(Planet my_planet in my_planets)
                {
                    float my_x = my_planet.transform.position.x;
                    float my_y = my_planet.transform.position.y;
                    float dist = Mathf.Sqrt(Mathf.Pow(my_y - target_planet.transform.position.y,2) + Mathf.Pow(my_x - target_planet.transform.position.x,2));
                    dist -= my_planet.get_planet_scale()/2 + target_planet.get_planet_scale()/2;
                    my_planet_dists_from_target.Add((my_planet,dist));
                }
                my_planet_dists_from_target.Sort((t1, t2) => t1.Item2.CompareTo(t2.Item2)); // sort planets based on distance from target
                int army_count = 0;
                int army_to_send = 0;
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
                            if(army_count_from_limit_planet > target_planet.get_population()  && army_count_from_limit_planet-limit_planet.Item1.get_population() <= target_planet.get_population())
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
                                    army_to_send = target_planet.get_population() - (army_count-my_planet.Item1.get_population()) + 1;
                                    //army_to_send = my_planet.Item1.get_population();
                                    if (army_to_send<0){ army_to_send=0;}
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
                    this.level_Manager.send_spaceship_to_planet_bot(sending_planet, target_planet, army_to_send);
                }
            }                 

        }else{
            foreach (Planet p in my_planets){
                foreach (Structure ep in enemy_planets){
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
