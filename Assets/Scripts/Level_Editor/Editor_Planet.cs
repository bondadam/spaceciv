using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Editor_Planet : Editor_Structure
{
    public TMP_Text initPopDynamic;
    private SerializedPlanet data;
    public SpriteRenderer m_SpriteRenderer;
    public Slider sizeSlider;
    public TextMeshProUGUI sizeValue;

    override public void Initialize(Vector2 coords, On_Destroy_Callback on_destroy)
    {

        this.data = new SerializedPlanet();
        this.data.team = Team.Neutral;
        this.data.position_x = coords.x;
        this.data.position_y = coords.y;
        this.data.initial_population = Constants.PLANET_DEFAULT_INITIAL_POPULATION;
        this.data.population_max = Constants.PLANET_DEFAULT_MAX_POPULATION;
        this.data.planet_size = 1;
        
        this.team_dropdown.value = (int) this.data.team;

        this.initPopSlider.maxValue = Constants.PLANET_DEFAULT_MAX_POPULATION;
        this.initPopSlider.value = Constants.PLANET_DEFAULT_INITIAL_POPULATION;
        this.initialPopValue.text = Constants.PLANET_DEFAULT_INITIAL_POPULATION.ToString();

        this.initPopDynamic.text = this.initialPopValue.text;

        this.maxPopSlider.maxValue = Constants.PLANET_ABSOLUTE_MAX_POPULATION;
        this.maxPopSlider.value = Constants.PLANET_DEFAULT_MAX_POPULATION;
        this.maxPopValue.text = Constants.PLANET_DEFAULT_MAX_POPULATION.ToString();

        this.sizeSlider.maxValue = Constants.PLANET_MAX_SIZE * 2.0f;
        this.sizeSlider.value = Constants.PLANET_DEFAULT_SIZE * 2.0f;
        this.sizeSlider.minValue = Constants.PLANET_MIN_SIZE * 2.0f;
        this.sizeValue.text = Constants.PLANET_DEFAULT_SIZE.ToString();

        this.notify_destroy = on_destroy;

        this.open_databox();
        this.update_identity();
    }

    public void Initialize_Load(SerializedPlanet data){
        this.data = data;

        this.team_dropdown.value = (int) this.data.team;

        this.initPopSlider.maxValue = Constants.PLANET_DEFAULT_MAX_POPULATION;
        this.initPopSlider.value = this.data.initial_population;
        this.initialPopValue.text = this.initPopSlider.value.ToString();

        this.initPopDynamic.text = this.initialPopValue.text;

        this.maxPopSlider.maxValue = Constants.PLANET_ABSOLUTE_MAX_POPULATION;
        this.maxPopSlider.value = this.data.population_max;
        this.maxPopValue.text = this.maxPopSlider.value.ToString();

        this.sizeSlider.maxValue = Constants.PLANET_MAX_SIZE * 2.0f;
        this.sizeSlider.value = this.data.planet_size * 2.0f;
        this.sizeSlider.minValue = Constants.PLANET_MIN_SIZE * 2.0f;
        this.sizeValue.text = this.sizeSlider.value.ToString();

        this.close_databox();
        this.update_position();
        this.update_identity();
    }
    override public void update_identity()
    {
        this.m_SpriteRenderer.transform.localScale = new Vector3(this.data.planet_size * 0.1f, this.data.planet_size * 0.1f, this.data.planet_size * 0.1f);
        this.m_SpriteRenderer.color = Constants.team_colors[this.data.team];
        this.update_position();
    }

    public void changeSize(){
        this.data.planet_size = this.sizeSlider.value / 2.0f;
        this.sizeValue.text =  this.data.planet_size.ToString();
        this.update_identity();
    }

    override public void update_position()
    {
        this.data.position_x = this.transform.position.x;
        this.data.position_y = this.transform.position.y;
        this.positionValue.text = "X : " + this.data.position_x + "\nY : " + this.data.position_y;
    }
    override public void change_team(int new_team)
    {
        this.data.team = (Team)new_team;
        this.update_identity();
    }

    override public void change_initPop()
    {
        this.data.initial_population = Mathf.FloorToInt(this.initPopSlider.value);
        this.initialPopValue.text = this.data.initial_population.ToString();
        this.initPopDynamic.text = this.initialPopValue.text;
        this.update_identity();
    }
    override public void change_maxPop()
    {
        this.data.population_max = Mathf.FloorToInt(this.maxPopSlider.value);
        this.maxPopValue.text = this.data.population_max.ToString();
        this.initPopSlider.maxValue = this.data.population_max;
        this.update_identity();
    }

    public SerializedPlanet get_data()
    {
        return this.data;
    }
    
}
