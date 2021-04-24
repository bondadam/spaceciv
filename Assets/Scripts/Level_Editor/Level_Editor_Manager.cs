using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_Editor_Manager : MonoBehaviour
{
    private Object_Type selected_object;
    private List<Editor_Planet> planets;
    private List<Bot_Config> bots;
    private Editor_Planet chosen_planet;
    public Editor_Planet editor_planet_prefab;

    public Bot_Config bot_config_prefab;

    public GameObject bots_panel;

    public GameObject bot_config_list;


    private string chosen_level;

    // Start is called before the first frame update
    void Start()
    {
        this.show_bots_panel(false);
        this.planets = new List<Editor_Planet>();
        this.bots = new List<Bot_Config>();
        this.chosen_planet = null;
        chosen_level = PlayerPrefs.GetString(Constants.EDITOR_CURRENT_LEVEL_NAME_PLAYER_PREF);
        if (chosen_level != null && chosen_level != "")
        {
            load(chosen_level);
        }
    }

    public void save()
    {
        GameObject[] planets_as_objects = GameObject.FindGameObjectsWithTag("Planet");
        SerializedPlanet[] serializedPlanets = new SerializedPlanet[planets_as_objects.Length];
        for (int i = 0; i < planets_as_objects.Length; i++)
        {
            Editor_Planet intermediary = (Editor_Planet)planets_as_objects[i].GetComponent<Editor_Planet>();
            serializedPlanets[i] = intermediary.get_data();
        }

        update_bots();
        SerializedBot[] save_bots = new SerializedBot[this.bots.Count];
        for (int i = 0; i < this.bots.Count; i++)
        {
            save_bots[i] = this.bots[i].get_data();
        }

        SerializedTurret[] turrets = new SerializedTurret[0];
        Level new_level = new Level(serializedPlanets, save_bots, turrets);
        string serialized_level = JsonUtility.ToJson(new_level);
        System.IO.FileInfo file;
        if (chosen_level == null || chosen_level == "")
        {
            List<string> paths_list = new List<string>(System.IO.Directory.GetFiles(Constants.USER_LEVEL_DIRECTORY_PATH + "/levels/"));
            int postfix = 1;
            string file_path = Constants.USER_LEVEL_DIRECTORY_PATH + "/levels/saved_level" + postfix.ToString() + ".json";
            while (paths_list.Contains(file_path))
            {
                postfix += 1;
                file_path = Constants.USER_LEVEL_DIRECTORY_PATH + "/levels/saved_level" + postfix.ToString() + ".json";
            }

            file = new System.IO.FileInfo(file_path);
        }
        else
        {
            file = new System.IO.FileInfo(Constants.USER_LEVEL_DIRECTORY_PATH + "/levels/" + chosen_level + ".json");
        }
        file.Directory.Create();
        System.IO.File.WriteAllText(file.FullName, serialized_level);
    }

    public void load(string level_filename)
    {
        string level_path = Save_File_Manager.getFullPath(level_filename);
        string level_json = System.IO.File.ReadAllText(level_path);
        Level level = JsonUtility.FromJson<Level>(level_json);

        foreach (Editor_Planet ep in this.planets)
        {
            ep.destroy();
        }
        this.planets = new List<Editor_Planet>();

        foreach (SerializedPlanet sp in level.planets)
        {
            Editor_Planet planet = Instantiate(editor_planet_prefab, new Vector3(sp.position_x, sp.position_y, 0), Quaternion.identity);
            Editor_Planet actual_planet = planet.GetComponent<Editor_Planet>();
            actual_planet.Initialize_Load(sp);
            this.planets.Add(actual_planet);
        }

        foreach (SerializedBot sb in level.bots)
        {
            Bot_Config bc = Instantiate(bot_config_prefab);
            Bot_Config actual_bot = bc.GetComponent<Bot_Config>();
            actual_bot.Initialize(sb);
            this.bots.Add(actual_bot);
            bc.transform.SetParent(this.bot_config_list.transform, false);
        }
    }

    public void exit()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void update_bots()
    {
        List<Team> accounted_for = new List<Team>();
        List<Bot_Config> bots_to_destroy = new List<Bot_Config>();
        List<Team> present_teams = get_present_teams();
        foreach (Bot_Config bc in this.bots)
        {
            Team bc_team = bc.get_data().team;
            if (!present_teams.Contains(bc_team))
            {
                bots_to_destroy.Add(bc);
            }
            else
            {
                accounted_for.Add(bc_team);
            }
        }
        foreach (Bot_Config bc_destroy in bots_to_destroy)
        {
            this.bots.Remove(bc_destroy);
            GameObject.Destroy(bc_destroy.gameObject);
        }
        if (this.bots.Count < present_teams.Count)
        {
            foreach (Team pt in present_teams)
            {
                if (!accounted_for.Contains(pt))
                {
                    Bot_Config bc = Instantiate(bot_config_prefab);
                    Bot_Config actual_bot = bc.GetComponent<Bot_Config>();
                    actual_bot.Initialize_New(pt);
                    this.bots.Add(actual_bot);
                    bc.transform.SetParent(this.bot_config_list.transform, false);
                }
            }
        }
    }

    public void show_bots_panel(bool show)
    {
        if (show)
        {
            update_bots();
            this.close_all_databoxes();
        }
        this.bots_panel.gameObject.SetActive(show);
    }

    public void center_camera()
    {
        Camera.main.transform.position = new Vector3(0, 0, Camera.main.transform.position.z);
    }

    public void select(Object_Type selection)
    {
        this.selected_object = selection;
    }

    public void select_planet()
    {
        this.select(Object_Type.Planet);
    }

    public void choose_planet(Editor_Planet planet)
    {
        this.chosen_planet = planet;
        this.selected_object = Object_Type.None;
    }

    public void move_chosen_planet(Vector2 new_coords)
    {
        this.chosen_planet.move(new_coords);
    }

    public void unselect()
    {
        this.selected_object = Object_Type.None;
    }

    public void close_all_databoxes()
    {
        foreach (Editor_Planet ep in this.planets)
        {
            if (ep != null)
            {
                ep.close_databox();
            }
        }
    }


    public void place_selected(Vector2 coords)
    {
        switch (this.selected_object)
        {
            case Object_Type.Planet:
                this.close_all_databoxes();
                Editor_Planet planet = Instantiate(editor_planet_prefab, new Vector3(coords.x, coords.y, 0), Quaternion.identity);
                Editor_Planet.On_Destroy_Callback on_destroy = new Editor_Planet.On_Destroy_Callback(remove_planet_from_list);
                planet.Initialize(coords, on_destroy);
                this.planets.Add(planet);
                break;
            default: // case null
                break;
        }
        this.unselect();
    }

    public List<Team> get_present_teams()
    {
        List<Team> present_teams = new List<Team>();
        foreach (Editor_Planet ep in this.planets)
        {
            Team ep_team = ep.get_data().team;
            if (ep_team != Team.Neutral && ep_team != Team.Player && !present_teams.Contains(ep_team))
            {
                present_teams.Add(ep_team);
            }
        }
        return present_teams;
    }
    public void remove_planet_from_list(Editor_Planet planet_to_remove)
    {
        planets.Remove(planet_to_remove);
    }

}
