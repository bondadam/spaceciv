using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Planet : MonoBehaviour
{
    public Level_Manager level_manager;
    private bool selected;

    private int units_taken_from_planet;

    private Team team;

    public const int min_selected = 1;

    private SpriteRenderer m_SpriteRenderer;

    private TextMeshPro population_display;

    private bool over_planet;
    private float planet_size = 1f;

    private int population = 1;
 
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

    void Start()
    {   
    
    }

    public void update_identity(){
        this.m_SpriteRenderer.color = Constants.team_colors[this.team];
        this.update_population_display();
    }
    public void Initialize(Level_Manager level_manager, Team team){
        this.tag = "Planet";

        this.m_SpriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        this.population_display = this.GetComponentInChildren<TextMeshPro>();

        this.team = team;
        this.units_taken_from_planet = 0;

        this.selected = false;

        this.m_SpriteRenderer.color = Constants.team_colors[this.team];
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
        
        this.update_population_display();

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
    }

    public Team get_team(){
        return this.team;
    }

    public void set_team(Team team){
        this.team = team;
    }
}
