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
    private float update_frequency = 0.016f; // 60 times/s
    private float timer = 0.0f;
    private List<Bot> bots;

    private float spaceship_speed;

    private int game_over_delay_check = 6;

    private int game_over_delay_check_counter = 0;

    private bool game_over = false;

    public Game_Over_Menu game_Over_Menu;

    public Game_State get_state_copy()
    {
        return new Game_State(this.planets, this.spaceships);
    }


    public void send_spaceship_to_planet(Planet target_planet)
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
                    spaceship.Initialize(from_planet.get_team(), incoming_units, from_planet, target_planet, spaceship_speed);
                    this.spaceships.Add(spaceship);
                }
                from_planet.unselect();
            }
        }
    }

    public void send_spaceship_to_planet_bot(Planet from_planet, Planet target_planet, int incoming_units)
    {
        from_planet.ungrow(incoming_units);
        Spaceship spaceship = Instantiate(spaceship_prefab, from_planet.transform.position, Quaternion.identity);
        spaceship.Initialize(from_planet.get_team(), incoming_units, from_planet, target_planet, spaceship_speed);
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
        this.spaceships = new List<Spaceship>();
        this.spaceship_speed = Game_Settings.BASE_SPACESHIP_SPEED;

        this.game_over = false;
        String level_json;
        if (Utils.selected_level == Constants.USER_LEVEL_CODE && System.IO.File.Exists(Constants.USER_LEVEL_DEFAULT_COMPLETE_PATH))
        {
            // User made level
            string level_path = Save_File_Manager.getFullPath(Utils.selected_custom_level);
            level_json = System.IO.File.ReadAllText(level_path);
        }
        else
        {
            TextAsset myTextAsset = Resources.Load(Constants.level_paths[Utils.selected_level]) as TextAsset;
            level_json = myTextAsset.text;
        }
        Level level = JsonUtility.FromJson<Level>(level_json);
        int planet_counter = 0;
        foreach (SerializedPlanet sp in level.planets)
        {
            Planet planet = Instantiate(planet_prefab, new Vector3(sp.position_x, sp.position_y, 0), Quaternion.identity);
            planet.Initialize(sp, "planet"+planet_counter.ToString());
            this.planets.Add(planet.GetComponent<Planet>());
            planet_counter += 1;
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
                        this.game_Over_Menu.end_game(player_alive, timer); // player_alive == true --> we won
                    }
                    this.game_over = game_over;
                }

                for (int i = 0; i < this.planets.Count; i++)
                {
                    this.planets[i].Update_Custom();
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