using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Spaceship : MonoBehaviour
{
    private int battling;
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

    public void update_display(){
        this.population_display.text = this.population.ToString();
    }

    public void set_population(int new_pop){
        this.population = new_pop;
        this.update_display();
    }

    public int get_population(){
        return this.population;
    }

    public void update_custom(float delta){
        if (this.moving){
            this.move(delta);
        }
    }

    public void die(){
        this.set_population(0);
        this.destroyable = true;
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Planet"){
            this.planetCollision(other.gameObject.GetComponent<Planet>());
        } else if (other.gameObject.tag == "Spaceship"){
            this.spaceShipCollision(other.gameObject.GetComponent<Spaceship>());
        }
    }

    private void spaceShipCollision(Spaceship collided_spaceship){
        if (this.team != collided_spaceship.team){
            // Yay space battle
            // Only one spaceship survives unless both
            // have exactly the same number of units
            int defending_units = collided_spaceship.population;
            if (this.population == defending_units){
                // its a tie
                collided_spaceship.set_population(1);
                this.set_population(1);
            }
            else if (this.population > defending_units){
                // this spaceship wins
                this.set_population(this.population - defending_units);
                collided_spaceship.die();
            } else{
                // this one loses
                collided_spaceship.set_population(collided_spaceship.get_population() - this.population);
                this.die();
            }
        }
    }

    private void planetCollision(Planet collided_planet){
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
            this.die();
        }
    }

    public void move(float delta){
        this.current_position = Vector2.MoveTowards(this.current_position, this.target_position, this.speed*delta);
        this.transform.position = new Vector3(this.current_position.x, this.current_position.y, this.transform.position.z);
    }

}
