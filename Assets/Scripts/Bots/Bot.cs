using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bot : MonoBehaviour
{

    protected Level_Manager level_Manager;
    private float update_frequency = 5.0f; // 60 times/s = 0.016f
    private float timer = 0.0f;
    public Team team;

    private bool alive;

    public void set_update_frequency(float update_frequency){
        this.update_frequency = update_frequency;
    }

    // Start is called before the first frame update
    public void Start()
    {
        
    }

    public Team get_team(){
        return this.team;
    }

    public abstract void make_Move(Game_State state);

    // Update is called once per frame
    public void Update()
    {
         this.timer += Time.deltaTime;

        // Check if we have reached beyond 16ms.
        // Subtracting is more accurate over time than resetting to zero.
        if (this.alive && this.timer > this.update_frequency)
        {
            Game_State game_state = this.level_Manager.get_state_copy();
            bool still_alive = Utils.check_alive(this.team, game_state);
            if (still_alive){
                this.timer = this.timer - this.update_frequency;
                this.make_Move(game_state);
            }
            else {
                this.die();
            }
        }
    }



    public void die(){
        Debug.Log("Bot" + this.ToString() + " is dead.");
        this.alive = false;
        this.enabled = false;
    }

    public (List<Planet>, List<Planet>) separate_planets(List<Planet> all_planets){
        List<Planet> my_planets = new List<Planet>();
        List<Planet> enemy_planets = new List<Planet>();
        foreach (Planet p in all_planets){
            if(p.team == this.team){
                my_planets.Add(p);
            } else {
                enemy_planets.Add(p);
            }
        }
        return (my_planets, enemy_planets);
    }

    public (List<Spaceship>, List<Spaceship>) separate_spaceships(List<Spaceship> all_spaceships){
        List<Spaceship> my_spaceships = new List<Spaceship>();
        List<Spaceship> enemy_spaceships = new List<Spaceship>();
        foreach (Spaceship s in all_spaceships){
            if(s.get_team() == this.team){
                my_spaceships.Add(s);
            } else {
                enemy_spaceships.Add(s);
            }
        }
        return (my_spaceships, enemy_spaceships);
    }

    public void Initialize(Level_Manager level_Manager, Team team){
        this.level_Manager = level_Manager;
        this.team = team;
        this.alive = true;
        this.CustomInitialize();
    }

    public void Initialize(Level_Manager level_Manager, Team team, float update_frequency){
        this.set_update_frequency(update_frequency);
        this.Initialize(level_Manager, team);
    }

    public virtual void CustomInitialize(){

    }

}
