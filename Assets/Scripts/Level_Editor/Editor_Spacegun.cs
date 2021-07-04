using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Editor_Spacegun : Editor_Structure
{
    private SerializedSpacegun data;
    public SpriteRenderer m_SpriteRenderer;

    override public void Initialize(Vector2 coords, On_Destroy_Callback on_destroy)
    {

        this.data = new SerializedSpacegun();
        this.data.team = Team.Neutral;
        this.data.position_x = coords.x;
        this.data.position_y = coords.y;
        this.data.initial_population = Constants.PLANET_DEFAULT_INITIAL_POPULATION;
        this.data.population_max = Constants.PLANET_DEFAULT_MAX_POPULATION;
        
        this.team_dropdown.value = (int) this.data.team;

        this.initPopSlider.maxValue = Constants.PLANET_DEFAULT_MAX_POPULATION;
        this.initPopSlider.value = Constants.PLANET_DEFAULT_INITIAL_POPULATION;
        this.initialPopValue.text = Constants.PLANET_DEFAULT_INITIAL_POPULATION.ToString();

        this.maxPopSlider.maxValue = Constants.PLANET_ABSOLUTE_MAX_POPULATION;
        this.maxPopSlider.value = Constants.PLANET_DEFAULT_MAX_POPULATION;
        this.maxPopValue.text = Constants.PLANET_DEFAULT_MAX_POPULATION.ToString();

        this.notify_destroy = on_destroy;

        this.open_databox();
        this.update_identity();
    }

    public void Initialize_Load(SerializedSpacegun data){
        this.data = data;

        this.team_dropdown.value = (int) this.data.team;

        this.initPopSlider.maxValue = Constants.PLANET_DEFAULT_MAX_POPULATION;
        this.initPopSlider.value = this.data.initial_population;
        this.initialPopValue.text = this.initPopSlider.value.ToString();

        this.maxPopSlider.maxValue = Constants.PLANET_ABSOLUTE_MAX_POPULATION;
        this.maxPopSlider.value = this.data.population_max;
        this.maxPopValue.text = this.maxPopSlider.value.ToString();


        this.close_databox();
        this.update_position();
        this.update_identity();
    }
    override public void update_identity()
    {
        this.m_SpriteRenderer.color = Constants.team_colors[this.data.team];
        this.update_position();
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
        this.update_identity();
    }
    override public void change_maxPop()
    {
        this.data.population_max = Mathf.FloorToInt(this.maxPopSlider.value);
        this.maxPopValue.text = this.data.population_max.ToString();
        this.initPopSlider.maxValue = this.data.population_max;
        this.update_identity();
    }

    public SerializedSpacegun get_data()
    {
        return this.data;
    }
   
}
