using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Planet : MonoBehaviour
{
    private bool selected;

    private int units_taken_from_planet;

    private int level;

    public Team team;

    public const int min_selected = 1;

    private SpriteRenderer m_SpriteRenderer;

    public TextMeshPro population_display;

    public Text upgrades_display;

    public Button upgrades_button;

    private bool over_planet;
    public float planet_size = 1f;

    public int initial_population;

    private int population;
 
    private int population_max = 99;
    public const int population_min = 1;

    public float growth_factor = 50f;

    private float growth_queue = 0.0f;
    public void grow(int num_pop){
        if (this.team != Team.Neutral){
            int total_population = this.population + this.units_taken_from_planet;
            this.population += Math.Min(this.population_max - total_population, num_pop);
        }
    }


    public void grow_back(int num_pop){
        this.population = Math.Min(this.population_max, this.population + num_pop);
    }

    public void cancelled_selection(){
        int units_taken = this.units_taken_from_planet;
        this.units_taken_from_planet = 0;
        this.grow(units_taken);
    }

    private bool ungrow_one(){
        // remove one person from population if possible
        // return 1/0 depending on success
        if (this.population > Planet.population_min){
            this.population--;
            return true;
        }
        return false;
    }

    public int ungrow(int num_pop){
        int num_sent = 0; // people we end up sending from this planet
        if ((this.population - num_pop) >= Planet.population_min){
            // If this planet has enough people on it
            num_sent = num_pop;
            this.population -= num_sent;
        } else{
            // Otherwise, send everyone while accounting for minimum pop
            num_sent = this.population - Planet.population_min;
            this.population -= num_sent;
        }
        return num_sent;
    }

    private void update_population_display(){
        this.population_display.text = this.population.ToString();
    }

    private void update_upgrades_display(){
        this.upgrades_display.text = new String('*', this.level);
    }

    void Start()
    {   
    
    }

    public void update_identity(){
        this.m_SpriteRenderer.color = Constants.team_colors[this.team];
        this.update_population_display();
    }
    /*
    public void Awake(){
        this.tag = "Planet";

        this.m_SpriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        this.population_display = this.GetComponentInChildren<TextMeshPro>();

        this.population = initial_population;

        this.units_taken_from_planet = 0;

        this.selected = false;

        this.m_SpriteRenderer.color = Constants.team_colors[this.team];

        //planet size grows proportionally to planet growth rate
        this.growth_factor = (this.planet_size * 10);


        this.transform.localScale = new Vector3(this.planet_size,this.planet_size,this.planet_size);
    }*/

    public void Initialize(SerializedPlanet serializedPlanet){

        this.team = serializedPlanet.team;
        this.transform.position = new Vector3(serializedPlanet.position_x, serializedPlanet.position_y, 0);
        this.initial_population = serializedPlanet.initial_population;
        this.population_max = serializedPlanet.population_max;
        this.planet_size = serializedPlanet.planet_size;

        this.tag = "Planet";

        this.m_SpriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        this.population_display = this.GetComponentInChildren<TextMeshPro>();
        //this.upgrades

        this.population = initial_population;

        this.units_taken_from_planet = 0;

        this.selected = false;

        this.m_SpriteRenderer.color = Constants.team_colors[this.team];

        this.level = 0;

        this.upgrades_button.gameObject.SetActive(this.can_upgrade());
        this.set_growth_factor();
        this.update_upgrades_display();
        this.transform.localScale = new Vector3(this.planet_size,this.planet_size,this.planet_size);

    }

    public int take_available_units(){
        return (this.selected && ungrow_one()) ? 1 : 0;

    }
    // Update is called once per frame
    public void Update_Custom(int units_taken_from_planet)
    {
        this.units_taken_from_planet = units_taken_from_planet;
        if (this.growth_queue > 1){
                this.grow(1);
                this.growth_queue = 0.0f;
        }
        else {
                this.growth_queue += this.growth_factor/100;
        }

        this.upgrades_button.gameObject.SetActive(this.can_upgrade());
        
        this.update_population_display();

    }

    public bool can_upgrade(){
        return this.population == this.population_max;
    }

    public void Update(){

    }

    public bool get_selected(){
        return this.selected;
    }

    public void set_selected(bool selection){
        this.selected = selection;
    }

    public int get_population(){
        return this.population;
    }

    public void set_population(int new_pop){
        this.population = new_pop;
        this.update_population_display();
    }

    public Team get_team(){
        return this.team;
    }

    public void set_team(Team team){
        this.team = team;
    }

    public int get_level(){
        return this.level;
    }

    public void set_level(int level){
        this.level = level;
    }

    public void upgrade(){
        if(this.can_upgrade()){
            this.level++;
            this.set_population(0);
            this.set_growth_factor();
            this.update_upgrades_display();
        }
    }

    public void set_growth_factor(){
        this.growth_factor = this.planet_size * 10 * (this.level+1);
    }
}
