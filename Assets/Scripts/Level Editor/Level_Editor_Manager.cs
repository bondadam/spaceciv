using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Editor_Manager : MonoBehaviour
{
    private Object_Type selected_object;
    private List<Editor_Planet> planets;
    public Camera camera;

    public Editor_Planet editor_planet_prefab;

    // Start is called before the first frame update
    void Start()
    {
        this.planets = new List<Editor_Planet>();
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

    public void unselect(){
        this.selected_object = Object_Type.None;
    }

    public void place_selected(Vector2 coords){
        switch (this.selected_object){
            case Object_Type.Planet:
                Editor_Planet planet = Instantiate(editor_planet_prefab, new Vector3(coords.x, coords.y, 0), Quaternion.identity);
                planet.Initialize(coords);
                break;
            default: // case null
                break;
        }
        this.unselect();
    }
}
