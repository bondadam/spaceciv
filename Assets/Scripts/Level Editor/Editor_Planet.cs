using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Editor_Planet : MonoBehaviour
{
    private SerializedPlanet data;
    public GameObject panel;
    private RectTransform panel_rect;
    // Start is called before the first frame update
    void Start()
    {
        this.panel_rect = panel.GetComponent<RectTransform>();
    }

    public void Initialize(Vector2 coords){
        this.data.team = Team.Neutral;
        this.data.position_x = coords.x;
        this.data.position_y = coords.y;
        this.data.initial_population = Constants.PLANET_INITIAL_POPULATION;
        this.data.population_max = Constants.PLANET_MAX_POPULATION;
        this.data.planet_size = 1;
        this.panel_rect.position = Camera.main.WorldToScreenPoint(new Vector3(coords.x, coords.y, 0));
    }

    public void show_data_panel(){
        panel.gameObject.SetActive(true);
    }

    public void hide_data_panel(){
        panel.gameObject.SetActive(false);
    }

    public void update_identity(){

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
