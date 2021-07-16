using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Planet : Structure
{
    public AudioSource conquestSound;
    public AudioSource selectSound;
    public AudioSource selectTwiceSound;
    public AudioSource unselectSound;

    private int level;
    private int max_level;

    private float planet_scale;
    private GameObject protected_symbol;

    public GameObject selectedcircle;
    public GameObject selectedcircle2;

    public Text upgrades_display;

    public Button upgrades_button;

    public float planet_size = 1f;

    public Sprite[] team_sprites;

    public MeshRenderer sphere;

    public float growth_factor = 50f;

    private float growth_queue = 0.0f;

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
    public void Initialize(SerializedPlanet serializedPlanet, string name, Level_Manager.Lose_Game_Callback lose_game_callback)
    {
        this.name = name;
        this.team = serializedPlanet.team;
        this.transform.position = new Vector3(serializedPlanet.position_x, serializedPlanet.position_y, 0);
        this.initial_population = serializedPlanet.initial_population;
        this.population_max = serializedPlanet.population_max;
        this.planet_size = serializedPlanet.planet_size;
        this.is_protected = serializedPlanet.is_protected;
        this.lose_game = lose_game_callback;
        this.tag = "Planet";
        this.structure_type = Structure_Type.Planet;

        this.m_SpriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        this.population_display = this.GetComponentInChildren<TextMeshPro>();
        //this.upgrades
        protected_symbol = new GameObject();
        if (this.is_protected)
        {
            SpriteRenderer NewImage = protected_symbol.AddComponent<SpriteRenderer>();
            NewImage.sprite = Resources.Load<Sprite>("star");
            NewImage.sortingLayerName = "Spaceshipground";
            protected_symbol.transform.SetParent(this.transform);
            protected_symbol.SetActive(true);
            protected_symbol.transform.localScale = new Vector3((float)0.07, (float)0.07, (float)0.07);
            protected_symbol.transform.position = new Vector3(serializedPlanet.position_x, serializedPlanet.position_y, 0);

        }
        this.population = initial_population;

        //this.m_SpriteRenderer.sprite = this.team_sprites[(int)this.team];
        Debug.Log(this.sphere);
        //this.sphere.material.SetColor("_TextureColor", Constants.team_colors[this.team]);
        this.sphere.material.SetColor("_Maincolor", Constants.team_colors[this.team]);
        this.sphere.material.SetColor("_Edgecolor", Constants.team_colors[this.team]);

        this.level = 0;
        this.max_level = 3;

        this.upgrades_button.gameObject.SetActive(this.can_upgrade());
        this.set_growth_factor();
        this.update_upgrades_display();
        this.planet_scale = 0.5f + planet_size * 0.75f;
        this.transform.localScale = new Vector3(planet_scale, planet_scale, planet_scale);

        //this.m_SpriteRenderer.transform.Rotate(0, 0, UnityEngine.Random.Range(-15, 45));

        // Generate Slightly Randomized Sound
        float base_pitch = 1f;
        float randomPitch =  Utils.randomlyAlterPitchPlanet(base_pitch, this.planet_size);
        //this.conquestSound.pitch = randomPitch;
        this.selectSound.pitch = randomPitch;
        this.selectTwiceSound.pitch = randomPitch;
        this.unselectSound.pitch = randomPitch;

    }

    private void selection_circles(Selected_State state){
        switch(state){
            case Selected_State.Full:
                this.selectedcircle.SetActive(true);
                this.selectedcircle2.SetActive(true);
                float circle1time = this.selectedcircle.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime;
                this.selectedcircle2.GetComponent<Animator>().Play("selectedcircle2",0, circle1time);
                break;
            case Selected_State.Half:
                this.selectedcircle2.SetActive(false);
                this.selectedcircle.SetActive(true);
                break;
            case Selected_State.Unselected:
            default:
                this.selectedcircle.SetActive(false);
                this.selectedcircle2.SetActive(false);
                break;
        }
    }
    override public void select()
    {
        if (this.team == Team.Player)
        {
            this.state = (Selected_State)(((int)this.state + 1) % Constants.states_num);
            this.selection_circles(this.state);
            this.playSelectSound(this.state);
        }
    }

    public void playSelectSound(Selected_State state){
        AudioSource sound_to_play;
        switch(state){
            case Selected_State.Full:
                sound_to_play = this.selectTwiceSound;
                break;
            case Selected_State.Half:
                sound_to_play = this.selectSound;
                break;
            case Selected_State.Unselected:
            default:
                sound_to_play = this.unselectSound;
                break;
        }
        sound_to_play.Play();
    }
    override public void unselect()
    {
        if (this.team == Team.Player)
        {
            this.state = Selected_State.Unselected;
            this.selection_circles(this.state);
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
        this.sphere.transform.Rotate(new Vector3(0,0.1f,0));
        if (this.growth_queue > 1)
        {
            this.grow(1);
            this.growth_queue = 0.0f;
        }
        else
        {
            this.growth_queue += this.growth_factor;
        }
        if (this.team == Team.Player)
        {
            this.upgrades_button.gameObject.SetActive(this.can_upgrade());
        }

        this.update_population_display();

    }

    public bool can_upgrade()
    {
        return false;
        if (this.get_team().Equals(Team.Neutral)) { return false; }
        return this.population == this.population_max && this.level < this.max_level;
    }

    public void Update()
    {

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

    override public void update_identity()
    {
        //this.m_SpriteRenderer.sprite = this.team_sprites[(int)this.team];
        //this.sphere.material.SetColor("_TextureColor", Constants.team_colors[this.team]);
        this.sphere.material.SetColor("_Maincolor", Constants.team_colors[this.team]);
        this.sphere.material.SetColor("_Edgecolor", Constants.team_colors[this.team]);
        this.protected_symbol.SetActive(this.is_protected);
        this.update_population_display();
    }

    override public void change_team(Team new_team)
    {
        this.unselect();
        this.set_team(new_team);
        this.update_identity();
        if (new_team == Team.Player)
        {
            this.conquestSound.Play();
        }
    }
 
    public void set_growth_factor()
    {
        this.growth_factor = this.planet_size * Game_Settings.BASE_PLANET_GROWTH_RATE * (this.level + 1) / 100;
    }
    public float get_growth_factor()
    {
        return this.growth_factor;
    }
    public float get_planet_scale()
    {
        return this.planet_scale;
    }
}
