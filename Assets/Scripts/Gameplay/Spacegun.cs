using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Spacegun : Structure
{
    private float reload_speed = 0.006f;

    private float reload_queue = 0f;

    private Planet target_planet;

    private float firepower = 3f;
    private float radius;

    private GameObject radius_object;
    
    private Level_Manager.Get_Nearest_Planet_Callback get_target;
    
    LineRenderer lineRenderer;
    LineRenderer laser_beam;
    
    Vector2 laser_beam_start;

    Vector2 laser_beam_end;

    float laser_beam_alpha;

    private void update_population_display()
    {
        this.population_display.text = this.population.ToString();
    }

    void Start()
    {

    }


    public void Initialize(SerializedSpacegun serializedSpacegun, string name, Level_Manager.Lose_Game_Callback lose_game_callback, Level_Manager.Get_Nearest_Planet_Callback get_nearest_planet_callback)
    {

        this.name = name;
        this.team = serializedSpacegun.team;
        this.transform.position = new Vector3(serializedSpacegun.position_x, serializedSpacegun.position_y, 0);
        this.initial_population = serializedSpacegun.initial_population;
        this.is_protected = serializedSpacegun.is_protected;
        this.population_max = serializedSpacegun.population_max;
        this.get_target = get_nearest_planet_callback;
        this.lose_game = lose_game_callback;
        this.radius = serializedSpacegun.radius;
        this.tag = "Spacegun";
        this.structure_type = Structure_Type.Spacegun;
        this.selectable = false;

        this.m_SpriteRenderer = this.GetComponentInChildren<SpriteRenderer>();

        this.population = initial_population;

        this.m_SpriteRenderer.color = Constants.team_colors[this.team];

        this.transform.localScale = new Vector3(1, 1, 1);

        this.draw_radius();
        
        laser_beam = gameObject.AddComponent<LineRenderer>();
        laser_beam.material = new Material(Shader.Find("UI/Default"));
        laser_beam.startColor = Color.red;
        laser_beam.endColor = Color.red;
        laser_beam.startWidth = 0.1f;
        laser_beam.endWidth = 0.1f;
        laser_beam.positionCount = 2;
        laser_beam.sortingLayerID = m_SpriteRenderer.sortingLayerID;
        laser_beam.sortingOrder = m_SpriteRenderer.sortingOrder;
    }

    public void draw_radius(){
        this.radius_object = new GameObject("perimeter");
        this.radius_object.transform.SetParent(this.transform);
        float theta_scale = 0.01f;
        float sizeValue = (2.0f * Mathf.PI) / theta_scale;
        int number_of_points = (int)Mathf.Floor(sizeValue)+1;
        lineRenderer = this.radius_object.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
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
            float x = radius * Mathf.Cos(theta) + this.transform.position.x;
            float y = radius * Mathf.Sin(theta) + this.transform.position.y;
            lineRenderer.SetPosition(i, new Vector2(x, y));
        }
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
        this.update_population_display();
        Color color = Color.yellow;
        laser_beam_alpha -= 0.2f;
        color.a = laser_beam_alpha;
        color = Color.red;
        color.a = laser_beam_alpha;
        laser_beam.startColor = color;
        laser_beam.endColor = color;
        laser_beam.SetPosition(0, laser_beam_start);
        laser_beam.SetPosition(1, laser_beam_end);
        if(this.team.Equals(Team.Neutral))
        {
            return;
        }
        // todo: only call get_target if there's not already a non-null target_spaceship in range
        this.target_planet = get_target(this.transform.position, this.radius, this.team);
        if(this.reload_queue > 1)
        {
            if(this.target_planet != null && !this.target_planet.team.Equals(Team.Neutral))
            {   

                laser_beam_alpha = 3.7f;
                color = Color.red;
                color.a = laser_beam_alpha;
                laser_beam.startColor = color;
                laser_beam.endColor = color;
                laser_beam_start = this.transform.position;
                laser_beam_end = target_planet.transform.position;
                laser_beam.SetPosition(0, laser_beam_start);
                laser_beam.SetPosition(1, laser_beam_end);
                if(this.target_planet.get_population() <= this.firepower)
                {
                    this.target_planet.set_population(1);
                    this.target_planet.set_team(Team.Neutral);
                    this.target_planet.update_identity();
                    this.target_planet.unselect();
                }
                else
                {
                    this.target_planet.set_population(this.target_planet.get_population()- (int) Mathf.Floor(this.firepower));
                }
                this.reload_queue = 0.0f;
            }
        }
        else
        {
            this.reload_queue += this.reload_speed;
        }

    }

    public void Update()
    {

    }


}
