using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level_Manager : MonoBehaviour
{
    public Planet planet_prefab;

    public Spaceship spaceship_prefab;
    // TODO: Change List<Planet> to Array for perf gains
    // see https://blogs.unity3d.com/2015/12/23/1k-update-calls/

    private List<Spaceship> spaceships;
    private List<Planet> planets;

    public units_taken units_taken_prefab;
    private units_taken units_taken;

    private float update_frequency = 0.016f; // 60 times/s
    private float timer = 0.0f;

    private List<Planet> selected_planets;

    private Planet currently_selected_planet;

    private List<Bot> bots;
    
    private int game_over_delay_check = 6;

    private int game_over_delay_check_counter = 0;

    private bool game_over = false;

    public Game_Over_Menu game_Over_Menu;

    public void select(Planet planet){
        if (!this.selected_planets.Contains(planet)){
            if (this.selected_planets.Count == 0 ){

                this.units_taken.add(planet, Planet.min_selected);
                this.units_taken.show();
                this.selected_planets.Add(planet);
                this.currently_selected_planet = planet;
                planet.set_selected(true);
            }  else if (this.selected_planets[0].get_team() == planet.get_team()){
                this.selected_planets.Add(planet);
                planet.set_selected(true);
                this.currently_selected_planet = planet;
            }
        } else if (this.selected_planets.Count > 0 && this.selected_planets[0].get_team() == planet.get_team()){
            this.currently_selected_planet = planet;
        }
    }

    public Game_State get_state_copy(){
        return new Game_State(this.planets, this.spaceships);
    }

    public void unselect(Planet planet){
        planet.set_selected(false);
        this.selected_planets.Remove(planet);
        Debug.Log("Planet unselected");
    }

    public void unselect_planets(){
        if (this.selected_planets.Count > 0){
            foreach (Planet p in this.selected_planets){
                p.set_selected(false);
            }
            this.selected_planets.Clear();
            this.units_taken.selection_done();
        }
    }

    public void set_currently_selected_planet(Planet planet){
        this.currently_selected_planet = planet;
    }

    public void cancel_selected_planets(){
        foreach (Planet p in this.selected_planets){
                p.cancelled_selection();
            }
        this.unselect_planets();
    }

    public int get_units_taken(Planet planet = null){
        // null -> returns ALL units taken
        return this.units_taken.get(planet);
    }

    public void send_spaceship_to_planet(Planet target_planet){
        if (this.selected_planets.Count > 0){
            foreach (Planet from_planet in this.selected_planets){
                int incoming_units = this.get_units_taken(from_planet);
                if (from_planet == target_planet){
                    // Don't send a ship from planet A to planet A
                    // DUH
                    target_planet.grow_back(incoming_units);
                } else {
                    Spaceship spaceship = Instantiate(spaceship_prefab, from_planet.transform.position , Quaternion.identity);
                    spaceship.Initialize(from_planet.get_team(), incoming_units, from_planet, target_planet);
                    this.spaceships.Add(spaceship);
                }
            }
            this.unselect_planets();
        }
    }

    public void send_spaceship_to_planet_bot(Planet from_planet, Planet target_planet, int incoming_units){
        from_planet.ungrow(incoming_units);
        Spaceship spaceship = Instantiate(spaceship_prefab, from_planet.transform.position , Quaternion.identity);
        spaceship.Initialize(from_planet.get_team(), incoming_units, from_planet, target_planet);
        this.spaceships.Add(spaceship);
    }


    // Start is called before the first frame update
    void Start()
    {
        this.Initialize();
    }

    public void Initialize(){
        this.units_taken = Instantiate(this.units_taken_prefab, Utils.getMousePosition(), Quaternion.identity);
        this.planets = new List<Planet>();
        this.spaceships = new List<Spaceship>();
        this.selected_planets = new List<Planet>();

        this.game_over = false;

        // OLD RANDOM PLANET INSTANTIATION CODE
        /*for (int i = 0; i < 10; i++){
            Planet planet = Instantiate(planet_prefab, new Vector3(Utils.floatRange(-4.0f, 4.0f), Utils.floatRange(-4.0f, 4.0f), 0), Quaternion.identity);
            planet.Initialize(this, Utils.randomEnumValue<Team>());
            planet.growth_factor =  Mathf.FloorToInt(Utils.floatRange(1f,20f));
            this.planets.Add(planet);
        }*/

       // OLD FIND PLANETS FROM EDITOR CODE
       // GameObject[] planets_as_game_objects = GameObject.FindGameObjectsWithTag("Planet");
       /*

        for (int i = 0; i < planets_as_game_objects.Length; i++){
            this.planets.Add(planets_as_game_objects[i].GetComponent<Planet>());
        }
        */

        TextAsset myTextAsset = Resources.Load(Constants.level_paths[Utils.selected_level]) as TextAsset; 
        Level level =  JsonUtility.FromJson<Level>(myTextAsset.text);

        foreach (SerializedPlanet sp in level.planets){
            Planet planet = Instantiate(planet_prefab, new Vector3(sp.position_x, sp.position_y, 0), Quaternion.identity);
            planet.Initialize(sp);
            this.planets.Add(planet.GetComponent<Planet>());
        }

        this.bots = new List<Bot>();

        // Handle Bot Types
        foreach (SerializedBot sb in level.bots){
            Bot new_bot;
            switch(sb.type){
                case "BlitzBot":
                    new_bot = gameObject.AddComponent<BlitzBot>() as BlitzBot;
                    break;
                case "DefensiveBot":
                    new_bot = gameObject.AddComponent<DefensiveBot>() as DefensiveBot;
                    break;
                default:
                     new_bot = gameObject.AddComponent<BlitzBot>() as BlitzBot;
                     break;
            }
            new_bot.Initialize(this, sb.team, sb.decision_interval);
            this.bots.Add(new_bot);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.selected_planets.Count > 0){
            this.units_taken.set_position_to_mouse();
        }

        this.timer += Time.deltaTime;

        // Check if we have reached beyond 16ms.
        // Subtracting is more accurate over time than resetting to zero.
        // Every 6*16ms (~1 sec), check that we are not in game over state
        if (!this.game_over){ 
            if (this.timer > this.update_frequency)
            {
                this.game_over_delay_check_counter++;
                if (this.game_over_delay_check_counter >= this.game_over_delay_check){
                    this.game_over_delay_check_counter = 0;
                    (bool game_over, bool player_alive) = this.check_game_over();
                    if (game_over){
                        Debug.Log("Game over!");
                        this.game_Over_Menu.end_game(player_alive); // player_alive == true --> we won
                    }
                    this.game_over = game_over;
                }
                
                for(int i = 0; i < this.planets.Count; i++){
                    this.planets[i].Update_Custom(this.get_units_taken(this.planets[i]));
                }

                for(int i = 0; i < this.spaceships.Count; i++){
                    this.spaceships[i].custom_update(this.update_frequency);
                    if (this.spaceships[i].destroyable){
                        Destroy(this.spaceships[i]);
                        this.spaceships.Remove(this.spaceships[i]);
                    }
                }

                if (this.currently_selected_planet != null){
                    this.units_taken.add(this.currently_selected_planet, this.currently_selected_planet.take_available_units());
                }


                // Remove the recorded 16ms.
                this.timer = this.timer - this.update_frequency;
            }
        }
        else {
            //Debug.Log("Game over!");
        }
    }

    public (bool, bool) check_game_over(){
        bool game_over = true;
        Game_State game_State = this.get_state_copy();
        bool player_alive = Utils.check_alive(Team.Player, game_State);
        if (player_alive){
            foreach (Bot b in this.bots){
                if (Utils.check_alive(b.get_team(), game_State)){
                    game_over = false;
                    break;
                }
            }
        }
        return (game_over, player_alive);
    }
}

public class Game_State{

    public List<Spaceship> spaceships;
    public List<Planet> planets;

    public Game_State(List<Planet> planets, List<Spaceship> spaceships){
        this.planets = planets;
        this.spaceships = spaceships;
    }

}