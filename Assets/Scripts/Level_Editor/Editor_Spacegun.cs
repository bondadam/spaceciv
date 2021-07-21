using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class Editor_Spacegun : Editor_Structure
{
    public TMP_Text initPopDynamic;
    private SerializedSpacegun data;
    private GameObject radius_object;
    public SpriteRenderer m_SpriteRenderer;
    private LineRenderer lineRenderer;
    public Slider radiusSlider;
    public TextMeshProUGUI radiusValue;

    override public void Initialize(Vector2 coords, On_Destroy_Callback on_destroy)
    {

        this.data = new SerializedSpacegun();
        this.data.team = Team.Neutral;
        this.data.position_x = coords.x;
        this.data.position_y = coords.y;
        this.data.initial_population = Constants.PLANET_DEFAULT_INITIAL_POPULATION;
        this.data.population_max = Constants.PLANET_DEFAULT_MAX_POPULATION;
        this.data.radius = Game_Settings.DEFAULT_SPACEGUN_RADIUS;
        
        this.team_dropdown.value = (int) this.data.team;

        this.initPopSlider.maxValue = Constants.PLANET_DEFAULT_MAX_POPULATION;
        this.initPopSlider.value = Constants.PLANET_DEFAULT_INITIAL_POPULATION;
        this.initialPopValue.text = Constants.PLANET_DEFAULT_INITIAL_POPULATION.ToString();

        this.initPopDynamic.text = this.initialPopValue.text;

        this.maxPopSlider.maxValue = Constants.PLANET_ABSOLUTE_MAX_POPULATION;
        this.maxPopSlider.value = Constants.PLANET_DEFAULT_MAX_POPULATION;
        this.maxPopValue.text = Constants.PLANET_DEFAULT_MAX_POPULATION.ToString();

        this.radiusSlider.maxValue = Game_Settings.MAX_SPACEGUN_RADIUS;
        this.radiusSlider.value = Game_Settings.DEFAULT_SPACEGUN_RADIUS;
        this.radiusSlider.minValue = Game_Settings.MIN_SPACEGUN_RADIUS;
        this.radiusValue.text = Game_Settings.DEFAULT_SPACEGUN_RADIUS.ToString();

        this.notify_destroy = on_destroy;

        //this.open_databox();
        this.update_identity();
    }

    public void Initialize_Load(SerializedSpacegun data){
        this.data = data;

        this.team_dropdown.value = (int) this.data.team;

        this.initPopSlider.maxValue = Constants.PLANET_DEFAULT_MAX_POPULATION;
        this.initPopSlider.value = this.data.initial_population;
        this.initialPopValue.text = this.initPopSlider.value.ToString();

        this.initPopDynamic.text = this.initialPopValue.text;

        this.maxPopSlider.maxValue = Constants.PLANET_ABSOLUTE_MAX_POPULATION;
        this.maxPopSlider.value = this.data.population_max;
        this.maxPopValue.text = this.maxPopSlider.value.ToString();

        this.radiusSlider.maxValue = Game_Settings.MAX_SPACEGUN_RADIUS;
        this.radiusSlider.value = this.data.radius;
        this.radiusSlider.minValue = Game_Settings.MIN_SPACEGUN_RADIUS;
        this.radiusValue.text = this.data.radius.ToString();


        this.close_databox();
        this.update_position();
        this.update_identity();
    }

    public void remove_radius(){
        GameObject.Destroy(this.radius_object);
    }

    public void draw_radius(float new_radius){
        this.radius_object = new GameObject("perimeter");
        this.radius_object.transform.SetParent(this.transform);
        float theta_scale = 0.01f;
        float sizeValue = (2.0f * Mathf.PI) / theta_scale;
        int number_of_points = (int)Mathf.Floor(sizeValue)+1;
        lineRenderer = this.radius_object.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("UI/Default"));
        lineRenderer.startColor = Color.gray;
        lineRenderer.endColor = Color.gray;
        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.02f;
        lineRenderer.positionCount = number_of_points;
        lineRenderer.sortingLayerID = m_SpriteRenderer.sortingLayerID;
        lineRenderer.sortingOrder = m_SpriteRenderer.sortingOrder;
        float theta = 0f;
        for (int i = 0; i < number_of_points; i++)
        {
            theta += (2.0f * Mathf.PI * theta_scale);
            float x = new_radius * Mathf.Cos(theta) + this.transform.position.x;
            float y = new_radius * Mathf.Sin(theta) + this.transform.position.y;
            lineRenderer.SetPosition(i, new Vector2(x, y));
        }
    }

    public void changeRadius(){
        this.data.radius = this.radiusSlider.value;
        this.radiusValue.text =  this.data.radius.ToString();
        this.update_identity();
        this.update_radius();
    }
    override public void update_identity()
    {
        this.m_SpriteRenderer.color = Constants.team_colors[this.data.team];
        
        this.update_position();
    }

    public void update_radius(){
        this.remove_radius();
        this.draw_radius(this.data.radius);
    }

    override public void update_position()
    {
        this.data.position_x = this.transform.position.x;
        this.data.position_y = this.transform.position.y;
        this.positionValue.text = "X : " + this.data.position_x + "\nY : " + this.data.position_y;
        this.update_radius();
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

    public SerializedSpacegun get_data()
    {
        return this.data;
    }
   
}
