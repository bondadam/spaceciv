using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Turret : Structure
{
    private GameObject radius_object;
    private float reload_speed = 0.01f;

    private float reload_queue = 0f;
    private float radius = 2f;

    private Spaceship target_spaceship;

    private float firepower = 3f;
    
    private Level_Manager.Get_Nearest_Spaceship_Callback get_target;
    
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


    public void Initialize(SerializedTurret serializedTurret, string name, Level_Manager.Lose_Game_Callback lose_game_callback, Level_Manager.Get_Nearest_Spaceship_Callback get_nearest_spaceship_callback)
    {

        this.name = name;
        this.team = serializedTurret.team;
        this.transform.position = new Vector3(serializedTurret.position_x, serializedTurret.position_y, 0);
        this.initial_population = serializedTurret.initial_population;
        this.population_max = serializedTurret.population_max;
        this.is_protected = serializedTurret.is_protected;
        this.get_target = get_nearest_spaceship_callback;
        this.lose_game = lose_game_callback;
        this.radius = serializedTurret.radius;
        this.tag = "Turret";

        this.m_SpriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        //this.upgrades

        this.population = initial_population;

        this.m_SpriteRenderer.color = Constants.team_colors[this.team];

        this.transform.localScale = new Vector3(1, 1, 1);

        this.draw_radius();
        
        laser_beam = gameObject.AddComponent<LineRenderer>();
        laser_beam.material = new Material(Shader.Find("UI/Default"));
        laser_beam.startColor = Color.yellow;
        laser_beam.endColor = Color.yellow;
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
        color = Color.yellow;
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
        this.target_spaceship = get_target(this.transform.position, this.radius, this.team);
       
        if(this.reload_queue > 1)
        {
            if(this.target_spaceship != null)
            {   

                laser_beam_alpha = 3.7f;
                color = Color.yellow;
                color.a = laser_beam_alpha;
                laser_beam.startColor = color;
                laser_beam.endColor = color;
                laser_beam_start = this.transform.position;
                laser_beam_end = target_spaceship.transform.position;
                laser_beam.SetPosition(0, laser_beam_start);
                laser_beam.SetPosition(1, laser_beam_end);
                if(this.target_spaceship.get_population() <= this.firepower)
                {
                    this.target_spaceship.die();
                }
                else
                {
                    this.target_spaceship.set_population(this.target_spaceship.get_population()- (int) Mathf.Floor(this.firepower));
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
