﻿using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level_Manager : MonoBehaviour
{
    public Planet planet_prefab;
    public Sun sun_prefab;
    public GameObject explosion_prefab;

    public GameObject UI;
    public Turret turret_prefab;
    public Spacegun spacegun_prefab;

    public Spaceship spaceship_prefab;
    // TODO: Change List<Planet> to Array for perf gains
    // see https://blogs.unity3d.com/2015/12/23/1k-update-calls/

    private float time_taken;
    private Level level;
    private List<Spaceship> spaceships;
    private List<Planet> planets;
    private List<Turret> turrets;
    private List<Spacegun> spaceguns;
    private List<Sun> suns;
    private float update_frequency = 0.016f; // 60 times/s
    private float timer = 0.0f;
    private List<Bot> bots;
    private float spaceship_speed;
    private int spaceship_count = 0;
    private int game_over_delay_check = 6;
    private int game_over_delay_check_counter = 0;
    private bool game_over = false;
    public TMP_Text level_indicator;
    public Game_Over_Menu game_Over_Menu;
    public delegate Spaceship Get_Nearest_Spaceship_Callback(Vector2 pos, float radius, Team except_team); 
    public delegate Planet Get_Nearest_Planet_Callback(Vector2 pos, float radius, Team except_team);   
    public delegate void Lose_Game_Callback(Team team);


    public Game_State get_state_copy()
    {
        return new Game_State(this.planets, this.spaceships, this.turrets, this.spaceguns);
    }

    public void create_spaceship_explosion_animation(Vector2 pos){
        GameObject explosion = Instantiate(this.explosion_prefab, new Vector3(pos.x, pos.y, 0), Quaternion.identity) as GameObject;
        explosion.transform.SetParent(this.UI.transform);
        explosion.transform.localScale = new Vector3(1,1,1);
    }


    public void load_tutorial(int tutorial_num){
        GameObject tutorialPartPrefab = Resources.Load("Tutorial/" + tutorial_num) as GameObject;
        Time.timeScale = 0;
        GameObject tutorialPart = Instantiate(tutorialPartPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        tutorialPart.transform.SetParent(this.UI.transform);
        tutorialPart.transform.localScale = new Vector3(1,1,1);
    }

    public void send_spaceship_to_planet(Structure target_planet)
    {

        foreach (Planet from_planet in this.planets)
        {
            if (from_planet.get_team() == Team.Player && target_planet != from_planet)
            {
                int incoming_units = from_planet.take_selected_units();
                if (incoming_units != 0)
                {
                    double diffx = from_planet.transform.position.x - target_planet.transform.position.x;
                    double diffy = from_planet.transform.position.y - target_planet.transform.position.y;
                    double angle = Math.Atan2(diffy, diffx) + Math.PI;
                    
                    double posx = from_planet.transform.position.x + Math.Cos(angle)*from_planet.get_planet_scale()*0.5;
                    double posy = from_planet.transform.position.y + Math.Sin(angle)*from_planet.get_planet_scale()*0.5;
                    Vector3 spaceship_launch_pos = new Vector3((float) posx, (float) posy, 0);
                    
                    Spaceship spaceship = Instantiate(spaceship_prefab, spaceship_launch_pos, Quaternion.identity);
                    this.spaceship_count ++;
                    Spaceship.Explosion_Animation_Callback explosion_callback = new Spaceship.Explosion_Animation_Callback(create_spaceship_explosion_animation);
                    spaceship.Initialize(from_planet.get_team(), incoming_units, from_planet, target_planet, spaceship_speed, "spaceship"+this.spaceship_count.ToString(), explosion_callback);
                    this.spaceships.Add(spaceship);
                }
                from_planet.unselect();
            }
        }
    }

    public void send_spaceship_to_planet_bot(Planet from_planet, Structure target_planet, int incoming_units)
    {
        if(incoming_units > from_planet.get_population()){ incoming_units = from_planet.get_population();} 
        if(incoming_units <= 0){ return;}
        double diffx = from_planet.transform.position.x - target_planet.transform.position.x;
        double diffy = from_planet.transform.position.y - target_planet.transform.position.y;
        double angle = Math.Atan2(diffy, diffx) + Math.PI;
        
        double posx = from_planet.transform.position.x + Math.Cos(angle)*from_planet.get_planet_scale()*0.5;
        double posy = from_planet.transform.position.y + Math.Sin(angle)*from_planet.get_planet_scale()*0.5;
        Vector3 spaceship_launch_pos = new Vector3((float) posx, (float) posy, 0);
        
        from_planet.ungrow(incoming_units);
        Spaceship spaceship = Instantiate(spaceship_prefab, spaceship_launch_pos, Quaternion.identity);
        this.spaceship_count ++;
        Spaceship.Explosion_Animation_Callback explosion_callback = new Spaceship.Explosion_Animation_Callback(create_spaceship_explosion_animation);
        spaceship.Initialize(from_planet.get_team(), incoming_units, from_planet, target_planet, spaceship_speed, "spaceship"+this.spaceship_count.ToString(), explosion_callback);
        this.spaceships.Add(spaceship);
    }


    // Start is called before the first frame update
    void Start()
    {
        this.Initialize();
    }

    public void Initialize()
    {
        this.planets = new List<Planet>();
        this.turrets = new List<Turret>();
        this.spaceguns = new List<Spacegun>();
        this.spaceships = new List<Spaceship>();
        this.suns = new List<Sun>();
        this.spaceship_speed = Game_Settings.BASE_SPACESHIP_SPEED;
        this.time_taken = 0; 
        this.game_over = false;
        String level_json;
        String level_indicator_text = String.Empty;
        if (Utils.selected_level == Constants.USER_LEVEL_CODE)
        {
            // User made level
            string level_path = Save_File_Manager.getFullPath(Utils.selected_custom_level);
            level_indicator_text = Utils.selected_custom_level;
            if(System.IO.File.Exists(level_path))
            {
                level_json = System.IO.File.ReadAllText(level_path);
            }
            else
            {
                // TODO: show panel saying "Level could not be loaded"? (or save file could not be found)
                Debug.Log("Save file could not be found");
                TextAsset myTextAsset = Resources.Load(Constants.level_paths[1].Item1) as TextAsset;
                level_json = myTextAsset.text;
            }
        }
        else
        {
            // Level from game's binaries
            TextAsset myTextAsset = Resources.Load(Constants.level_paths[Utils.selected_level].Item1) as TextAsset;
            level_json = myTextAsset.text;
            level_indicator_text = Utils.selected_level.ToString();
            
        }
        this.level_indicator.text = "level " + level_indicator_text;
        level = JsonUtility.FromJson<Level>(level_json);
        int structure_counter = 0;
        int spaceentity_counter = 0;
        if(level.suns != null){
            foreach (SerializedSpaceEntity sse in level.suns)
            {
                Sun sun = Instantiate(sun_prefab, new Vector3(sse.position_x, sse.position_y, 0), Quaternion.identity);
                sun.Initialize(sse, "sun"+spaceentity_counter.ToString());
                this.suns.Add(sun.GetComponent<Sun>());

                spaceentity_counter += 1;
            }
        }
        foreach (SerializedSpacegun ssg in level.spaceguns)
        {
            Spacegun spacegun = Instantiate(spacegun_prefab, new Vector3(ssg.position_x, ssg.position_y, 0), Quaternion.identity);
            Get_Nearest_Planet_Callback get_nearest_planet_callback = new Get_Nearest_Planet_Callback(get_nearest_planet_within_radius);
            Lose_Game_Callback lose_game_callback = new Lose_Game_Callback(lose_game);
            spacegun.Initialize(ssg, "spacegun"+structure_counter.ToString(), lose_game_callback, get_nearest_planet_callback);
            this.spaceguns.Add(spacegun.GetComponent<Spacegun>());
            structure_counter += 1;
        }
        foreach (SerializedPlanet sp in level.planets)
        {
            Planet planet = Instantiate(planet_prefab, new Vector3(sp.position_x, sp.position_y, 0), Quaternion.identity);
            Lose_Game_Callback lose_game_callback = new Lose_Game_Callback(lose_game);
            planet.Initialize(sp, "planet"+structure_counter.ToString(), lose_game_callback);
            this.planets.Add(planet.GetComponent<Planet>());
            structure_counter += 1;
        }
        foreach (SerializedTurret st in level.turrets)
        {
            Turret turret = Instantiate(turret_prefab, new Vector3(st.position_x, st.position_y, 0), Quaternion.identity);
            Get_Nearest_Spaceship_Callback get_nearest_spaceship_callback = new Get_Nearest_Spaceship_Callback(get_nearest_spaceship_within_radius);
            Lose_Game_Callback lose_game_callback = new Lose_Game_Callback(lose_game);
            turret.Initialize(st, "turret"+structure_counter.ToString(), lose_game_callback, get_nearest_spaceship_callback);
            this.turrets.Add(turret.GetComponent<Turret>());
            structure_counter += 1;
        }

        this.bots = new List<Bot>();

        // Handle Bot Types
        foreach (SerializedBot sb in level.bots)
        {
            Bot new_bot;
            switch (sb.type)
            {
                case Bot_Type.BlitzBot:
                    new_bot = gameObject.AddComponent<BlitzBot>() as BlitzBot;
                    break;
                case Bot_Type.DefensiveBot:
                    new_bot = gameObject.AddComponent<DefensiveBot>() as DefensiveBot;
                    break;
                case Bot_Type.ExpandingBot:
                    new_bot = gameObject.AddComponent<ExpandingBot>() as ExpandingBot;
                    break;
                case Bot_Type.JuggernautBot:
                    new_bot = gameObject.AddComponent<JuggernautBot>() as JuggernautBot;
                    break;
                case Bot_Type.EmptyBot:
                    new_bot = gameObject.AddComponent<EmptyBot>() as EmptyBot;
                    break;
                case Bot_Type.ProximityBot:
                    new_bot = gameObject.AddComponent<ProximityBot>() as ProximityBot;
                    break;
                case Bot_Type.JuggernautProximityBot:
                    new_bot = gameObject.AddComponent<JuggernautProximityBot>() as JuggernautProximityBot;
                    break;
                default:
                    new_bot = gameObject.AddComponent<BlitzBot>() as BlitzBot;
                    break;
            }
            new_bot.Initialize(this, sb.team, sb.decision_interval);
            this.bots.Add(new_bot);
        }
        SpaceLoad.switchColors((Background_Color)level.color);

        if(level.tutorial>0 && Utils.selected_level != Constants.USER_LEVEL_CODE){
            load_tutorial(level.tutorial);
        }
    }

    // Update is called once per frame
    void Update()
    {

        this.timer += Time.deltaTime;

        // Check if we have reached beyond 16ms.
        // Subtracting is more accurate over time than resetting to zero.
        // Every 6*16ms (~1 sec), check that we are not in game over state
        if (!this.game_over)
        {
            if (this.timer > this.update_frequency)
            {
                this.game_over_delay_check_counter++;
                if (this.game_over_delay_check_counter >= this.game_over_delay_check)
                {
                    this.game_over_delay_check_counter = 0;
                    (bool game_over, bool player_alive) = this.check_game_over();
                    if (game_over)
                    {
                        Debug.Log("Game over!");
                        LevelStatsKeeper.set_timer(this.time_taken);
                        float time_goal = level.record_time;
                        this.game_Over_Menu.end_game(player_alive, time_goal); // player_alive == true --> we won
                    }
                    this.game_over = game_over;
                }

                for (int i = 0; i < this.planets.Count; i++)
                {
                    this.planets[i].Update_Custom();
                }

                foreach (Turret turret in this.turrets)
                {
                    turret.Update_Custom();
                }
                foreach (Spacegun spacegun in this.spaceguns)
                {
                    spacegun.Update_Custom();
                }
                for (int i = 0; i < this.spaceships.Count; i++)
                {
                    this.spaceships[i].custom_update(this.update_frequency);
                    if (this.spaceships[i].destroyable)
                    {
                        Destroy(this.spaceships[i]);
                        this.spaceships.Remove(this.spaceships[i]);
                    }
                }

                // Remove the recorded 16ms.
                this.timer = this.timer - this.update_frequency;
                //And update level stats timer
                this.time_taken += this.update_frequency;
            }
        }
        else
        {
            //Debug.Log("Game over!");
        }
    }
    private void lose_game(Team team)
    {
        Game_State game_State = this.get_state_copy();
        List<Structure> structures = new List<Structure>();
        structures.AddRange(game_State.planets);
        structures.AddRange(game_State.turrets);
        structures.AddRange(game_State.spaceguns);

        foreach(Structure s in structures)
        {
            if(s.get_team() == team)
            {
                s.set_team(Team.Neutral);
            }
        }
        foreach(Spaceship s in game_State.spaceships)
        {
            if(s.get_team() == team)
            {
                s.set_team(Team.Neutral);
            }
        }
    }
    public (bool, bool) check_game_over()
    {
        bool game_over = true;
        Game_State game_State = this.get_state_copy();
        bool player_alive = Utils.check_alive(Team.Player, game_State);
        if (player_alive)
        {
            foreach (Bot b in this.bots)
            {
                if (Utils.check_alive(b.get_team(), game_State))
                {
                    game_over = false;
                    break;
                }
            }
        }
        return (game_over, player_alive);
    }

    public Spaceship get_nearest_spaceship_within_radius(Vector2 pos, float radius, Team except_team)
    {
        Spaceship nearest_spaceship;
        if(this.spaceships.Count == 0)
        {
            return null;
        }
        else
        {
            nearest_spaceship = this.spaceships[0];
            double min_dist = double.MaxValue;
            foreach (Spaceship sp in this.spaceships)
            {
                double dist = Mathf.Sqrt(Mathf.Pow(pos[1] - sp.transform.position.y,2) + Mathf.Pow(pos[0] - sp.transform.position.x,2));
                if(dist < min_dist && !sp.get_team().Equals(except_team))
                {
                    nearest_spaceship = sp;
                    min_dist = dist;
                }
            }
            if(min_dist >= radius)
            {
                return null;
            }
            else
            {
                return nearest_spaceship;
            }
        }
    }
     public Planet get_nearest_planet_within_radius(Vector2 pos, float radius, Team except_team)
    {
        Planet nearest_planet;
        if(this.planets.Count == 0)
        {
            return null;
        }
        else
        {
            nearest_planet = this.planets[0];
            double min_dist = double.MaxValue;
            foreach (Planet pl in this.planets)
            {
                double dist = Mathf.Sqrt(Mathf.Pow(pos[1] - pl.transform.position.y,2) + Mathf.Pow(pos[0] - pl.transform.position.x,2));
                if(dist < min_dist && !pl.get_team().Equals(except_team))
                {
                    nearest_planet = pl;
                    min_dist = dist;
                }
            }
            if(min_dist >= radius)
            {
                return null;
            }
            else
            {
                return nearest_planet;
            }
        }
    }
}

public class Game_State
{

    public List<Spaceship> spaceships;
    public List<Planet> planets;
    public List<Turret> turrets;
    public List<Spacegun> spaceguns;

    public Game_State(List<Planet> planets, List<Spaceship> spaceships, List<Turret> turrets, List<Spacegun> spaceguns)
    {
        this.planets = planets;
        this.spaceships = spaceships;
        this.turrets = turrets;
        this.spaceguns = spaceguns;

    }

}