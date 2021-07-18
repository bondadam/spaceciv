using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Level_Editor_Manager : MonoBehaviour
{
    public TMP_InputField save_input;
    public GameObject save_overlay;
    private Level_Difficulty difficulty;
    private Background_Color background_Color;
    public TextMeshProUGUI save_placeholder;
    private Object_Type selected_object;
    private List<Editor_Planet> planets;
    private List<Editor_Turret> turrets;
    private List<Editor_Spacegun> spaceguns;
    private List<Editor_Sun> suns;
    private List<Editor_FrozenVoid> frozenVoids;
    private List<Bot_Config> bots;
    private Editor_Object chosen_object;
    public Editor_Planet editor_planet_prefab;
    public Editor_Turret editor_turret_prefab;
    public Editor_Spacegun editor_spacegun_prefab;
    public Editor_Sun editor_sun_prefab;
    public Editor_FrozenVoid editor_frozenvoid_prefab;

    public Bot_Config bot_config_prefab;

    public GameObject bots_panel;

    public GameObject bot_config_list;
    public TMP_Dropdown backgroundColor_dropdown;
    public TMP_Dropdown difficulty_dropdown;


    private string chosen_level;

    // Start is called before the first frame update
    void Start()
    {
        this.show_bots_panel(false);
        this.toggle_save_overlay();
        this.planets = new List<Editor_Planet>();
        this.turrets = new List<Editor_Turret>();
        this.spaceguns = new List<Editor_Spacegun>();
        this.suns = new List<Editor_Sun>();
        this.frozenVoids = new List<Editor_FrozenVoid>();
        this.bots = new List<Bot_Config>();
        this.chosen_object = null;
        this.difficulty = Level_Difficulty.Easy;
        this.background_Color = Background_Color.default_color;
        chosen_level = PlayerPrefs.GetString(Constants.EDITOR_CURRENT_LEVEL_NAME_PLAYER_PREF);
        if (chosen_level != null && chosen_level != "")
        {
            this.save_placeholder.text = string.Empty;
            this.save_input.text = chosen_level;
            load(chosen_level);
        }
    }

    public void toggle_save_overlay()
    {
        this.save_overlay.SetActive(!this.save_overlay.activeSelf);
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


        GameObject[] turrets_as_objects = GameObject.FindGameObjectsWithTag("Turret");
        SerializedTurret[] serializedTurrets = new SerializedTurret[turrets_as_objects.Length];
        for (int i = 0; i < turrets_as_objects.Length; i++)
        {
            Editor_Turret intermediary = (Editor_Turret)turrets_as_objects[i].GetComponent<Editor_Turret>();
            serializedTurrets[i] = intermediary.get_data();
        }

        GameObject[] spaceguns_as_objects = GameObject.FindGameObjectsWithTag("Spacegun");
        SerializedSpacegun[] serializedSpaceguns = new SerializedSpacegun[spaceguns_as_objects.Length];
        for (int i = 0; i < spaceguns_as_objects.Length; i++)
        {
            Editor_Spacegun intermediary = (Editor_Spacegun)spaceguns_as_objects[i].GetComponent<Editor_Spacegun>();
            serializedSpaceguns[i] = intermediary.get_data();
        }

        GameObject[] suns_as_objects = GameObject.FindGameObjectsWithTag("Sun");
        SerializedSpaceEntity[] serializedSuns = new SerializedSpaceEntity[suns_as_objects.Length];
        for (int i = 0; i < suns_as_objects.Length; i++)
        {
            Editor_Sun intermediary = (Editor_Sun)suns_as_objects[i].GetComponent<Editor_Sun>();
            serializedSuns[i] = intermediary.get_data();
        }

        GameObject[] frozenvoids_as_objects = GameObject.FindGameObjectsWithTag("FrozenVoid");
        SerializedSpaceEntity[] serializedFrozenVoids = new SerializedSpaceEntity[frozenvoids_as_objects.Length];
        for (int i = 0; i < frozenvoids_as_objects.Length; i++)
        {
            Editor_FrozenVoid intermediary = (Editor_FrozenVoid)frozenvoids_as_objects[i].GetComponent<Editor_FrozenVoid>();
            serializedFrozenVoids[i] = intermediary.get_data();
        }

        update_bots();
        SerializedBot[] save_bots = new SerializedBot[this.bots.Count];
        for (int i = 0; i < this.bots.Count; i++)
        {
            save_bots[i] = this.bots[i].get_data();
        }

        Level new_level = new Level(save_bots, serializedPlanets, serializedTurrets, serializedSpaceguns, serializedSuns, serializedFrozenVoids);
        new_level.difficulty = this.difficulty;
        new_level.color = this.background_Color;
        string serialized_level = JsonUtility.ToJson(new_level);
        System.IO.FileInfo file;

        if (this.save_input.text != string.Empty)
        {
            file = new System.IO.FileInfo(Constants.USER_LEVEL_DIRECTORY_PATH + "/levels/" + this.save_input.text + ".json");
        }
        else if (chosen_level == null || chosen_level == "")
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
        this.save_overlay.SetActive(false);
    }

    public void clean()
    {

        foreach (Editor_Planet ep in this.planets)
        {
            ep.destroy();
        }
        this.planets = new List<Editor_Planet>();

        foreach (Editor_Turret et in this.turrets)
        {
            et.destroy();
        }
        this.turrets = new List<Editor_Turret>();

        foreach (Editor_Spacegun es in this.spaceguns)
        {
            es.destroy();
        }
        this.spaceguns = new List<Editor_Spacegun>();

        foreach (Editor_Sun esn in this.suns)
        {
            esn.destroy();
        }
        this.suns = new List<Editor_Sun>();

        foreach (Editor_FrozenVoid ef in this.frozenVoids)
        {
            ef.destroy();
        }
        this.frozenVoids = new List<Editor_FrozenVoid>();
    }

    public void load(string level_filename)
    {

        this.clean();

        string level_path = Save_File_Manager.getFullPath(level_filename);
        string level_json = System.IO.File.ReadAllText(level_path);
        Level level = JsonUtility.FromJson<Level>(level_json);

        foreach (SerializedPlanet sp in level.planets)
        {
            Editor_Planet planet = Instantiate(editor_planet_prefab, new Vector3(sp.position_x, sp.position_y, 0), Quaternion.identity);
            Editor_Planet actual_planet = planet.GetComponent<Editor_Planet>();
            actual_planet.Initialize_Load(sp);
            this.planets.Add(actual_planet);
        }

        foreach (SerializedTurret st in level.turrets)
        {
            Editor_Turret turret = Instantiate(editor_turret_prefab, new Vector3(st.position_x, st.position_y, 0), Quaternion.identity);
            Editor_Turret actual_turret = turret.GetComponent<Editor_Turret>();
            actual_turret.Initialize_Load(st);
            this.turrets.Add(actual_turret);
        }

        foreach (SerializedSpacegun ss in level.spaceguns)
        {
            Editor_Spacegun spacegun = Instantiate(editor_spacegun_prefab, new Vector3(ss.position_x, ss.position_y, 0), Quaternion.identity);
            Editor_Spacegun actual_spacegun = spacegun.GetComponent<Editor_Spacegun>();
            actual_spacegun.Initialize_Load(ss);
            this.spaceguns.Add(actual_spacegun);
        }

        foreach (SerializedSpaceEntity sse in level.suns)
        {
            Editor_Sun sun = Instantiate(editor_sun_prefab, new Vector3(sse.position_x, sse.position_y, 0), Quaternion.identity);
            Editor_Sun actual_sun = sun.GetComponent<Editor_Sun>();
            actual_sun.Initialize_Load(sse);
            this.suns.Add(actual_sun);
        }

        foreach (SerializedSpaceEntity sse in level.frozenvoids)
        {
            Editor_FrozenVoid frozenVoid = Instantiate(editor_frozenvoid_prefab, new Vector3(sse.position_x, sse.position_y, 0), Quaternion.identity);
            Editor_FrozenVoid actual_frozenVoid = frozenVoid.GetComponent<Editor_FrozenVoid>();
            actual_frozenVoid.Initialize_Load(sse);
            this.frozenVoids.Add(actual_frozenVoid);
        }

        foreach (SerializedBot sb in level.bots)
        {
            Bot_Config bc = Instantiate(bot_config_prefab);
            Bot_Config actual_bot = bc.GetComponent<Bot_Config>();
            actual_bot.Initialize(sb);
            this.bots.Add(actual_bot);
            bc.transform.SetParent(this.bot_config_list.transform, false);
        }

        this.difficulty_dropdown.value = (int)level.difficulty;
        this.backgroundColor_dropdown.value = (int)level.color;
        SpaceLoad.switchColors(level.color);
    }

    public void exit()
    {
        SceneManager.LoadScene("Level_Editor_Menu");
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

    public void difficulty_changed(int new_difficulty_int)
    {
        Level_Difficulty new_difficulty = (Level_Difficulty)new_difficulty_int;
        this.difficulty = new_difficulty;
    }

    public void backgroundcolor_changed(int new_background_color_int)
    {
        Background_Color new_bgcolor = (Background_Color)new_background_color_int;
        SpaceLoad.switchColors(new_bgcolor);
        this.background_Color = new_bgcolor;
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
    public void select_turret()
    {
        this.select(Object_Type.Turret);
    }

    public void select_spacegun()
    {
        this.select(Object_Type.Spacegun);
    }

    public void select_sun(){
        this.select(Object_Type.Sun);
    }

    public void select_frozenvoid(){
        this.select(Object_Type.FrozenVoid);
    }

    public void choose_object(Editor_Object obj)
    {
        this.chosen_object = obj;
        this.selected_object = Object_Type.None;
    }

    public void move_chosen_object(Vector2 new_coords)
    {
        this.chosen_object.move(new_coords);
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
        foreach (Editor_Turret et in this.turrets)
        {
            if (et != null)
            {
                et.close_databox();
            }
        }
        foreach (Editor_Spacegun es in this.spaceguns)
        {
            if (es != null)
            {
                es.close_databox();
            }
        }

        foreach (Editor_Sun esn in this.suns)
        {
            if (esn != null)
            {
                esn.close_databox();
            }
        }

        foreach (Editor_FrozenVoid ef in this.frozenVoids)
        {
            if (ef != null)
            {
                ef.close_databox();
            }
        }
    }


    public void place_selected(Vector2 coords)
    {
        this.close_all_databoxes();
        switch (this.selected_object)
        {
            case Object_Type.Planet:
                Editor_Planet planet = Instantiate(editor_planet_prefab, new Vector3(coords.x, coords.y, 0), Quaternion.identity);
                Editor_Planet.On_Destroy_Callback on_destroy = new Editor_Planet.On_Destroy_Callback(remove_planet_from_list);
                planet.Initialize(coords, on_destroy);
                this.planets.Add(planet);
                break;
            case Object_Type.Turret:
                Editor_Turret turret = Instantiate(editor_turret_prefab, new Vector3(coords.x, coords.y, 0), Quaternion.identity);
                Editor_Turret.On_Destroy_Callback on_destroy_turret = new Editor_Turret.On_Destroy_Callback(remove_turret_from_list);
                turret.Initialize(coords, on_destroy_turret);
                this.turrets.Add(turret);
                break;
            case Object_Type.Spacegun:
                Editor_Spacegun spacegun = Instantiate(editor_spacegun_prefab, new Vector3(coords.x, coords.y, 0), Quaternion.identity);
                Editor_Spacegun.On_Destroy_Callback on_destroy_spacegun = new Editor_Spacegun.On_Destroy_Callback(remove_spacegun_from_list);
                spacegun.Initialize(coords, on_destroy_spacegun);
                this.spaceguns.Add(spacegun);
                break;
            case Object_Type.Sun:
                Editor_Sun sun = Instantiate(editor_sun_prefab, new Vector3(coords.x, coords.y, -5), Quaternion.identity);
                Editor_Sun.On_Destroy_Callback on_destroy_sun = new Editor_Sun.On_Destroy_Callback(remove_sun_from_list);
                sun.Initialize(coords, on_destroy_sun);
                this.suns.Add(sun);
                break;
            case Object_Type.FrozenVoid:
                Editor_FrozenVoid frozenvoid = Instantiate(editor_frozenvoid_prefab, new Vector3(coords.x, coords.y, -5), Quaternion.identity);
                Editor_FrozenVoid.On_Destroy_Callback on_destroy_frozenvoid = new Editor_FrozenVoid.On_Destroy_Callback(remove_frozenvoid_from_list);
                frozenvoid.Initialize(coords, on_destroy_frozenvoid);
                this.frozenVoids.Add(frozenvoid);
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
        foreach (Editor_Turret et in this.turrets)
        {
            Team et_team = et.get_data().team;
            if (et_team != Team.Neutral && et_team != Team.Player && !present_teams.Contains(et_team))
            {
                present_teams.Add(et_team);
            }
        }
        foreach (Editor_Spacegun es in this.spaceguns)
        {
            Team es_team = es.get_data().team;
            if (es_team != Team.Neutral && es_team != Team.Player && !present_teams.Contains(es_team))
            {
                present_teams.Add(es_team);
            }
        }
        return present_teams;
    }
    public void remove_planet_from_list(Editor_Object object_to_remove)
    {
        this.planets.Remove((Editor_Planet)object_to_remove);
    }

    public void remove_turret_from_list(Editor_Object object_to_remove)
    {
        this.turrets.Remove((Editor_Turret)object_to_remove);
    }

    public void remove_spacegun_from_list(Editor_Object structure_to_remove)
    {
        this.spaceguns.Remove((Editor_Spacegun)structure_to_remove);
    }
    public void remove_sun_from_list(Editor_Object object_to_remove)
    {
        this.suns.Remove((Editor_Sun)object_to_remove);
    }

    public void remove_frozenvoid_from_list(Editor_Object object_to_remove)
    {
        this.frozenVoids.Remove((Editor_FrozenVoid)object_to_remove);
    }
}

