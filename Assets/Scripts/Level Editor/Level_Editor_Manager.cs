using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Level_Editor_Manager : MonoBehaviour
{
    private Object_Type selected_object;
    private List<Editor_Planet> planets;
    public Camera camera;
    private Editor_Planet chosen_planet;
    public Editor_Planet editor_planet_prefab;

    // Start is called before the first frame update
    void Start()
    {
        this.planets = new List<Editor_Planet>();
        this.chosen_planet = null;
    }

    public void save(){
        GameObject[] planets_as_objects = GameObject.FindGameObjectsWithTag("Planet");
        SerializedPlanet[] serializedPlanets = new SerializedPlanet[planets_as_objects.Length];
        for(int i = 0; i < planets_as_objects.Length; i++){
            Editor_Planet intermediary = (Editor_Planet) planets_as_objects[i].GetComponent<Editor_Planet>();
            serializedPlanets[i] = intermediary.get_data();
        }

        SerializedBot[] bots = new SerializedBot[4];
        bots[0] = new SerializedBot(Team.CPU1, "BlitzBot", 10);
        bots[1] = new SerializedBot(Team.CPU2, "BlitzBot", 9);
        bots[2] = new SerializedBot(Team.CPU3, "BlitzBot", 8);
        bots[3] = new SerializedBot(Team.CPU4, "BlitzBot", 7);

        Level new_level = new Level(serializedPlanets, bots);
        string serialized_level = JsonUtility.ToJson(new_level);
        System.IO.FileInfo file = new System.IO.FileInfo(Constants.USER_LEVEL_DEFAULT_COMPLETE_PATH);
        file.Directory.Create();
        System.IO.File.WriteAllText(file.FullName, serialized_level);
    }

    public void exit(){
        SceneManager.LoadScene("MainMenu");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void center_camera(){
        this.camera.transform.position = new Vector3(0, 0, this.camera.transform.position.z);
    }

    public void select(Object_Type selection){
        this.selected_object = selection;
    }

    public void select_planet(){
        this.select(Object_Type.Planet);
    }

    public void choose_planet(Editor_Planet planet){
        this.chosen_planet = planet;
        this.selected_object = Object_Type.None;
    }

    public void move_chosen_planet(Vector2 new_coords){
        this.chosen_planet.move(new_coords);
    }

    public void unselect(){
        this.selected_object = Object_Type.None;
    }

    public void close_all_databoxes(){
        foreach (Editor_Planet ep in this.planets){
            if (ep != null){
                ep.close_databox();
            }
        }
    }

    public void place_selected(Vector2 coords){
        switch (this.selected_object){
            case Object_Type.Planet:
                this.close_all_databoxes();
                Editor_Planet planet = Instantiate(editor_planet_prefab, new Vector3(coords.x, coords.y, 0), Quaternion.identity);
                planet.Initialize(coords);
                this.planets.Add(planet);
                break;
            default: // case null
                break;
        }
        this.unselect();
    }

}
