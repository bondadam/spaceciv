using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Structure : MonoBehaviour
{
    public Team team;

    public const int min_selected = 1;

    public SpriteRenderer m_SpriteRenderer;

    public TMP_Text population_display;

    public int initial_population;

    public int population;

    public int population_max;
    public const int population_min = 0;

    public Selected_State state;

    public String name;
    
    
   /* public void Initialize(SerializedPlanet serializedPlanet, string name)
    {
        this.name = name;
        this.team = serializedPlanet.team;
        this.transform.position = new Vector3(serializedPlanet.position_x, serializedPlanet.position_y, 0);
        this.initial_population = serializedPlanet.initial_population;
        this.population_max = serializedPlanet.population_max;

        this.population = initial_population;


    }
*/

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
            this.set_population(this.population-num_sent);
        }
        else
        {
            // Otherwise, send everyone while accounting for minimum pop
            num_sent = this.population - Planet.population_min;
            this.set_population(this.population-num_sent);
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
        string my_pop = this.population.ToString();
        this.population_display.text = my_pop;
    }

    void Start()
    {

    }

    virtual public void update_identity()
    {
        this.m_SpriteRenderer.color = Constants.team_colors[this.team];
        this.update_population_display();
    }




    public int get_population()
    {
        return this.population;
    }

    public Vector2 get_position()
    {
        return this.transform.position;
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
    public string get_name()
    {
        return this.name;
    }
    public float get_planet_size()
    {
        return (float)0;
    }
    public float get_planet_scale()
    {
        return (float)0;
    }
}
