using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Spaceship : MonoBehaviour
{
    private Team team;
    private SpriteRenderer m_SpriteRenderer;

    private float speed = 1f;

    private TextMeshPro population_display;

    private Planet target;

    private Vector2 target_position;

    private Vector2 current_position;

    private bool moving;

    private int population;

    public bool destroyable;

     public void Initialize(Team team, int population, Planet target){

        this.m_SpriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        this.population_display = this.GetComponentInChildren<TextMeshPro>();

        this.moving = true;
        this.destroyable = false;

        // Set positions

        this.target = target;
        this.target_position = this.target.transform.position;
        this.current_position = new Vector2(this.transform.position.x, this.transform.position.y);

        // set angle
        Vector3 targ = this.target.transform.position;
        targ.z = 0f;
        targ.x = targ.x - this.transform.position.x;
        targ.y = targ.y - this.transform.position.y;
        float angle = Mathf.Atan2(-targ.x, targ.y) * Mathf.Rad2Deg;
        this.m_SpriteRenderer.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));


        this.team = team;
        this.population = population;
        this.population_display.text = this.population.ToString();
        this.m_SpriteRenderer.color = Constants.team_colors[this.team];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void custom_update(float delta){
        if ( this.moving){
            this.move(delta);
        }
        //if ()
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Planet"){
            Planet collided_planet = other.gameObject.GetComponent<Planet>();
            if (collided_planet == this.target){
                this.moving = false;
                if (collided_planet.get_team() == this.team){
                    // Transfer of units between planets
                    collided_planet.grow(this.population);
                } else {
                    int defending_units = collided_planet.get_population();
                    if (this.population > defending_units){
                        // invasion success
                        collided_planet.set_population(this.population - defending_units);
                        collided_planet.set_team(this.team);
                        collided_planet.update_identity();
                        Debug.Log("invasion success");
                    } else{
                        // invasion defeat
                        collided_planet.ungrow(this.population);
                        Debug.Log("defeat!");
                    }
                }
                this.population = 0;
                this.destroyable = true;
                this.gameObject.SetActive(false);
            }
        }
    }

    public void move(float delta){
        this.current_position = Vector2.MoveTowards(this.current_position, this.target_position, this.speed*delta);
        this.transform.position = new Vector3(this.current_position.x, this.current_position.y, this.transform.position.z);
    }

    // Update is called once per frame  
    void Update()
    {
        //this.move(Time.deltaTime);
    }
}
