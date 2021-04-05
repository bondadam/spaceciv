using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Editor_Planet : MonoBehaviour
{
    private SerializedPlanet data;
    public Data_Box_Manager data_Box;
    private SpriteRenderer m_SpriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void Initialize(Vector2 coords)
    {
        this.m_SpriteRenderer = this.GetComponentInChildren<SpriteRenderer>();

        this.data = new SerializedPlanet();
        this.data.team = Team.Neutral;
        this.data.position_x = coords.x;
        this.data.position_y = coords.y;
        this.data.initial_population = Constants.PLANET_INITIAL_POPULATION;
        this.data.population_max = Constants.PLANET_MAX_POPULATION;
        this.data.planet_size = 1;

        this.data_Box.Initialize(this.data);
        this.open_databox();

        this.update_identity();
    }
    public void update_identity()
    {
        this.m_SpriteRenderer.color = Constants.team_colors[this.data.team];
    }
    
    public SerializedPlanet get_data(){
        return this.data;
    }

    public void save_data(){
        SerializedPlanet temp_data = this.data_Box.get_data();
        this.data.team = temp_data.team;
        this.data.position_x = temp_data.position_x;
        this.data.position_y = temp_data.position_y;
        this.data.initial_population = temp_data.initial_population;
        this.data.population_max = temp_data.population_max;
        this.data.planet_size = temp_data.planet_size;
        this.update_identity();
    }

    public void open_databox()
    {
        this.data_Box.gameObject.SetActive(true);
    }

    public void close_databox()
    {
        this.data_Box.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
