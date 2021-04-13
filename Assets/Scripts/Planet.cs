using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Planet : MonoBehaviour
{
    private int level;

    private int max_level;

    public Team team;

    public const int min_selected = 1;

    private SpriteRenderer m_SpriteRenderer;

    public TextMeshPro population_display;

    public Text upgrades_display;

    public Button upgrades_button;

    public float planet_size = 1f;

    public int initial_population;

    private int population;

    private int population_max;
    public const int population_min = 1;

    public float growth_factor = 50f;

    private float growth_queue = 0.0f;
    private Selected_State state;

    private String name;
    
    public void grow(int num_pop)
    {
        if (this.team != Team.Neutral)
        {
            this.population = Math.Min(this.population_max, this.population + num_pop);
        }
    }
    public int ungrow(int num_pop)
    {
        int num_sent = 0; // people we end up sending from this planet
        if ((this.population - num_pop) >= Planet.population_min)
        {
            // If this planet has enough people on it
            num_sent = num_pop;
            this.population -= num_sent;
        }
        else
        {
            // Otherwise, send everyone while accounting for minimum pop
            num_sent = this.population - Planet.population_min;
            this.population -= num_sent;
        }
        return num_sent;
    }

    public void select()
    {
        if (this.team == Team.Player)
        {
            this.state = (Selected_State)(((int)this.state + 1) % Constants.states_num);
            this.m_SpriteRenderer.color = Constants.selected_color[this.state];
        }
    }

    public void unselect()
    {
        if (this.team == Team.Player)
        {
            this.state = Selected_State.Unselected;
            this.m_SpriteRenderer.color = Constants.selected_color[this.state];
        }
    }

    private void update_population_display()
    {
        this.population_display.text = this.population.ToString();
    }

    private void update_upgrades_display()
    {
        this.upgrades_display.text = new String('*', this.level);
    }

    void Start()
    {

    }

    public void update_identity()
    {
        this.m_SpriteRenderer.color = Constants.team_colors[this.team];
        this.update_population_display();
    }

    public void Initialize(SerializedPlanet serializedPlanet)
    {

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

        this.m_SpriteRenderer.color = Constants.team_colors[this.team];

        this.level = 0;
        this.max_level = 3;

        this.upgrades_button.gameObject.SetActive(this.can_upgrade());
        this.set_growth_factor();
        this.update_upgrades_display();
        this.transform.localScale = new Vector3(this.planet_size, this.planet_size, this.planet_size);

    }

    public int take_selected_units()
    {
        int selected_units = (int)Math.Floor(Constants.selected_value[this.state] * this.population);
        this.ungrow(selected_units);
        return selected_units;
    }

    // Update is called once per frame
    public void Update_Custom()
    {
        if (this.growth_queue > 1)
        {
            this.grow(1);
            this.growth_queue = 0.0f;
        }
        else
        {
            this.growth_queue += this.growth_factor / 100;
        }
        if (this.team == Team.Player)
        {
            this.upgrades_button.gameObject.SetActive(this.can_upgrade());
        }

        this.update_population_display();

    }

    public bool can_upgrade()
    {
        return this.population == this.population_max && this.level < this.max_level;
    }

    public void Update()
    {

    }


    public int get_population()
    {
        return this.population;
    }

    public void set_population(int new_pop)
    {
        this.population = new_pop;
        this.update_population_display();
    }

    public Team get_team()
    {
        return this.team;
    }

    public void set_team(Team team)
    {
        this.team = team;
    }

    public int get_level()
    {
        return this.level;
    }
    public float get_planet_size()
    {
        return this.planet_size;
    }
    public void set_level(int level)
    {
        this.level = Math.Min(level, this.max_level);
    }

    public void upgrade()
    {
        if (this.can_upgrade())
        {
            this.level++;
            this.set_population(Constants.PLANET_DEFAULT_INITIAL_POPULATION);
            this.set_growth_factor();
            this.update_upgrades_display();
        }
    }

    public void set_growth_factor(){
        this.growth_factor = this.planet_size * Constants.PLANET_BASE_GROWTH * (this.level+1) ;
    }
}
