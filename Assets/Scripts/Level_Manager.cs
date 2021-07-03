using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level_Manager : MonoBehaviour
{
    public Planet planet_prefab;

    public Turret turret_prefab;
    public Spacegun spacegun_prefab;

    public Spaceship spaceship_prefab;
    // TODO: Change List<Planet> to Array for perf gains
    // see https://blogs.unity3d.com/2015/12/23/1k-update-calls/

    private float time_taken;
    private List<Spaceship> spaceships;
    private List<Planet> planets;
    private List<Turret> turrets;
    private List<Spacegun> spaceguns;
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


    public Game_State get_state_copy()
    {
        return new Game_State(this.planets, this.spaceships);
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
                    
                    double posx = from_planet.transform.position.x + Math.Cos(angle)*from_planet.get_planet_size()*0.5;
                    double posy = from_planet.transform.position.y + Math.Sin(angle)*from_planet.get_planet_size()*0.5;
                    Vector3 spaceship_launch_pos = new Vector3((float) posx, (float) posy, 0);
                    
                    Spaceship spaceship = Instantiate(spaceship_prefab, spaceship_launch_pos, Quaternion.identity);
                    this.spaceship_count ++;
                    spaceship.Initialize(from_planet.get_team(), incoming_units, from_planet, target_planet, spaceship_speed, "spaceship"+this.spaceship_count.ToString());
                    this.spaceships.Add(spaceship);
                }
                from_planet.unselect();
            }
        }
    }

    public void send_spaceship_to_planet_bot(Planet from_planet, Structure target_planet, int incoming_units)
    {
        from_planet.ungrow(incoming_units);
        Spaceship spaceship = Instantiate(spaceship_prefab, from_planet.transform.position, Quaternion.identity);
        this.spaceship_count ++;
        spaceship.Initialize(from_planet.get_team(), incoming_units, from_planet, target_planet, spaceship_speed, "spaceship"+this.spaceship_count.ToString());
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
        this.spaceship_speed = Game_Settings.BASE_SPACESHIP_SPEED;
        this.time_taken = 0; 
        this.game_over = false;
        String level_json;
        Debug.Log(Utils.selected_level);
        if (Utils.selected_level == Constants.USER_LEVEL_CODE)
        {
            // User made level
            string level_path = Save_File_Manager.getFullPath(Utils.selected_custom_level);
            if(System.IO.File.Exists(level_path))
            {
                level_json = System.IO.File.ReadAllText(level_path);
            }
            else
            {
                // TODO: show panel saying "Level could not be loaded"? (or save file could not be found)
                Debug.Log("Save file could not be found");
                TextAsset myTextAsset = Resources.Load(Constants.level_paths[1]) as TextAsset;
                level_json = myTextAsset.text;
            }
        }
        else
        {
            // Level from game's binaries
            TextAsset myTextAsset = Resources.Load(Constants.level_paths[Utils.selected_level]) as TextAsset;
            level_json = myTextAsset.text;
            
        }
        this.level_indicator.text = "level " + Utils.selected_level.ToString();
        Level level = JsonUtility.FromJson<Level>(level_json);
        int structure_counter = 0;

        foreach (SerializedSpacegun ssg in level.spaceguns)
        {
            Spacegun spacegun = Instantiate(spacegun_prefab, new Vector3(ssg.position_x, ssg.position_y, 0), Quaternion.identity);
            Get_Nearest_Planet_Callback callback = new Get_Nearest_Planet_Callback(get_nearest_planet_within_radius);
            spacegun.Initialize(ssg, "spacegun"+structure_counter.ToString(), callback);
            this.spaceguns.Add(spacegun.GetComponent<Spacegun>());
            structure_counter += 1;
        }
        foreach (SerializedPlanet sp in level.planets)
        {
            Planet planet = Instantiate(planet_prefab, new Vector3(sp.position_x, sp.position_y, 0), Quaternion.identity);
            planet.Initialize(sp, "planet"+structure_counter.ToString());
            this.planets.Add(planet.GetComponent<Planet>());
            structure_counter += 1;
        }
        foreach (SerializedTurret st in level.turrets)
        {
            Turret turret = Instantiate(turret_prefab, new Vector3(st.position_x, st.position_y, 0), Quaternion.identity);
            Get_Nearest_Spaceship_Callback callback = new Get_Nearest_Spaceship_Callback(get_nearest_spaceship_within_radius);
            turret.Initialize(st, "turret"+structure_counter.ToString(), callback);
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
                default:
                    new_bot = gameObject.AddComponent<BlitzBot>() as BlitzBot;
                    break;
            }
            new_bot.Initialize(this, sb.team, sb.decision_interval);
            this.bots.Add(new_bot);
        }
        var random = new System.Random();
        int next_color_index = random.Next(Constants.space_colors.Count);
        SpaceLoad.switchColors(Constants.space_colors[next_color_index]);
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
                        this.game_Over_Menu.end_game(player_alive); // player_alive == true --> we won
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

    public Game_State(List<Planet> planets, List<Spaceship> spaceships)
    {
        this.planets = planets;
        this.spaceships = spaceships;
    }

}