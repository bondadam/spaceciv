using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Spaceship : MonoBehaviour
{
    private List<Spaceship> battling;
    private Team team;
    private SpriteRenderer m_SpriteRenderer;

    private float speed;

    //private int frozen;

    private TextMeshPro population_display;

    private Structure target;

    private Planet origin;

    private Vector2 target_position;

    private Vector2 current_position;

    private bool moving;

    private int population;

    public bool destroyable;
    private int frozen;
    private string Name;
    private Explosion_Animation_Callback explosion_callback;
    public delegate void Explosion_Animation_Callback(Vector2 pos);
    public IEnumerator unfreeze(float seconds_to_wait)
    {
        yield return new WaitForSeconds(seconds_to_wait);
       // this.frozen--;
    }

    public void Initialize(Team team, int population, Planet origin, Structure target, float speed, string name, Explosion_Animation_Callback explosion_callback)
    {

        this.m_SpriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        this.population_display = this.GetComponentInChildren<TextMeshPro>();

        this.moving = true;
        this.destroyable = false;
        this.speed = speed;
        this.Name = name;
        this.explosion_callback = explosion_callback;
        this.frozen = 0;

        this.battling = new List<Spaceship>();

        // Set positions

        this.origin = origin;
        this.target = target;
        this.target_position = this.target.transform.position;
        this.current_position = new Vector2(this.transform.position.x, this.transform.position.y);

        // set rotation
        this.face(this.target_position);


        this.team = team;
        this.population = population;
        this.population_display.text = this.population.ToString();
        this.m_SpriteRenderer.color = Constants.team_colors[this.team];

        if (this.team == Team.Player)
        {
            LevelStatsKeeper.sent_spaceship();
        }
    }

    public void face(Vector3 target_position)
    {
        // Takes the object.transform.position as input
        // Because too lazy to cast to gameobject everytime
        target_position.z = 0f;
        target_position.x = target_position.x - this.transform.position.x;
        target_position.y = target_position.y - this.transform.position.y;
        float angle = Mathf.Atan2(-target_position.x, target_position.y) * Mathf.Rad2Deg;
        this.m_SpriteRenderer.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void add_battling(Spaceship spaceship)
    {
        this.battling.Add(spaceship);
    }

    public void remove_battling(Spaceship spaceship)
    {
        this.battling.Remove(spaceship);
    }

    public void freeze()
    {
        //this.frozen++;
    }

    public bool is_battling()
    {
        return this.battling.Count > 0;
    }

    public bool ungrow_one()
    {
        // returns TRUE if alive,
        // returns FALSE if dead
        if (this.population > 0)
        {
            this.set_population(this.population - 1);
        }
        if (this.population < 1)
        {
            this.die(true);
            return false;
        }
        return true;
    }

    public void update_display()
    {
        this.population_display.text = this.population.ToString();
        this.m_SpriteRenderer.color = Constants.team_colors[this.team];
    }

    public void set_population(int new_pop)
    {
        this.population = new_pop;
        this.update_display();
    }

    public int get_population()
    {
        return this.population;
    }


    public void die(bool dead_from_battle = true)
    {
        if (dead_from_battle)
        {
            explosion_callback(this.transform.position);
        }
        this.set_population(0);
        this.destroyable = true;
        this.gameObject.SetActive(false);
        // GameObject.Destroy(this.gameObject);
    }

    public Team get_team()
    {
        return this.team;
    }
    public void set_team(Team team)
    {
        this.team = team;
        this.update_display();
    }

    public Structure get_target()
    {
        return this.target;
    }

    public Planet get_source()
    {
        return this.origin;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "FrozenVoid")
        {
            this.frozen = 2;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
       if (other.gameObject.tag == "Sun")
        {
            this.die(true);
        
        }else if (other.gameObject.tag == "Planet" || other.gameObject.tag == "Turret" || other.gameObject.tag == "Spacegun")
        {
            //Debug.Log("Fleet touched a " + other.gameObject.tag);
            Structure collided_planet = other.gameObject.GetComponent<Structure>();
            switch (collided_planet.get_structure_type())
            {
                case Structure_Type.Planet:
                    collided_planet = (Planet)collided_planet;
                    break;
                case Structure_Type.Turret:
                    collided_planet = (Turret)collided_planet;
                    break;
                case Structure_Type.Spacegun:
                    collided_planet = (Spacegun)collided_planet;
                    break;
            }
            if (collided_planet == this.target)
            {
                this.moving = false;
                if (collided_planet.get_team() == this.team)
                {
                    // Transfer of units between planets
                    collided_planet.grow(this.population);
                    this.die(false);
                }
                else
                {
                    int defending_units = collided_planet.get_population();
                    if (this.population > defending_units)
                    {
                        // invasion success

                        // Update Stats

                        if (this.team == Team.Player)
                        {
                            LevelStatsKeeper.planet_conquered();
                        }
                        else if (collided_planet.team == Team.Player)
                        {
                            LevelStatsKeeper.planet_lost();
                        }

                        collided_planet.set_population(this.population - defending_units);
                        collided_planet.change_team(this.team);
                        this.die(false);
                    }
                    else
                    {
                        // invasion defeat
                        collided_planet.ungrow(this.population);
                        this.die(false);
                    }
                }
            }
        }
        else if (other.gameObject.tag == "Spaceship")
        {
            Spaceship collided_spaceship = other.gameObject.GetComponent<Spaceship>();
            if (this.team != collided_spaceship.team)
            {
                // Yay space battle
                // Only one spaceship survives unless both
                // have exactly the same number of units
                this.face(collided_spaceship.transform.position);
                this.add_battling(collided_spaceship);
            }
        }
    }

    public void custom_update(float delta)
    {
        this.frozen --;
        if (this.is_battling())
        {
            for (int i = 0; i < this.battling.Count; i++)
            {
                if (this.battling[i] == null)
                {
                    // Spaceship has already been destroyed by something else
                    this.battling.RemoveAt(i);
                }
                else
                {
                    bool other_spaceship_alive = this.battling[i].ungrow_one();
                    if (!other_spaceship_alive)
                    {
                        LevelStatsKeeper.spaceship_destroyed();
                        this.remove_battling(this.battling[i]);
                    }
                }
            }
            if (!this.is_battling())
            {
                // We have battled the last battle,
                // Let's go on our merry way
                this.face(this.target_position);
            }
        }
        else
        {
            this.move(delta);
        }
    }

    public void move(float delta)
    {
        if (this.frozen>0)
        {
            this.current_position = Vector2.MoveTowards(this.current_position, this.target_position, this.speed * delta * Constants.FROZEN_SPACESHIP_RELATIVE_SPEED);
        }
        else
        {
            this.current_position = Vector2.MoveTowards(this.current_position, this.target_position, this.speed * delta);
        }
        this.transform.position = new Vector3(this.current_position.x, this.current_position.y, this.transform.position.z);
    }

    // Update is called once per frame  
    void Update()
    {
    }
    public string get_Name()
    {
        return this.Name;
    }
}
