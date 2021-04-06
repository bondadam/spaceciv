using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Editor_Planet : MonoBehaviour
{
    private SerializedPlanet data;
    public Canvas data_Box;

    public SpriteRenderer m_SpriteRenderer;
    public TMP_Dropdown team_dropdown;

    public Slider initPopSlider;

    public TextMeshProUGUI initialPopValue;
    public Slider maxPopSlider;

    public TextMeshProUGUI maxPopValue;
    public Slider sizeSlider;

    public TextMeshProUGUI sizeValue;

    public TextMeshProUGUI positionValue;
    // Start is called before the first frame update
    void Start()
    {
    }

    void Awake(){
    }

    public void Initialize(Vector2 coords)
    {

        this.data = new SerializedPlanet();
        this.data.team = Team.Neutral;
        this.data.position_x = coords.x;
        this.data.position_y = coords.y;
        this.data.initial_population = Constants.PLANET_DEFAULT_INITIAL_POPULATION;
        this.data.population_max = Constants.PLANET_DEFAULT_MAX_POPULATION;
        this.data.planet_size = 1;

        this.team_dropdown.value = (int) this.data.team;

        this.initPopSlider.maxValue = Constants.PLANET_DEFAULT_MAX_POPULATION;
        this.initPopSlider.value = Constants.PLANET_DEFAULT_INITIAL_POPULATION;
        this.initialPopValue.text = Constants.PLANET_DEFAULT_INITIAL_POPULATION.ToString();

        this.maxPopSlider.maxValue = Constants.PLANET_ABSOLUTE_MAX_POPULATION;
        this.maxPopSlider.value = Constants.PLANET_DEFAULT_MAX_POPULATION;
        this.maxPopValue.text = Constants.PLANET_DEFAULT_MAX_POPULATION.ToString();

        this.sizeSlider.maxValue = Constants.PLANET_MAX_SIZE * 2.0f;
        this.sizeSlider.value = Constants.PLANET_DEFAULT_SIZE * 2.0f;
        this.sizeSlider.minValue = Constants.PLANET_MIN_SIZE * 2.0f;
        this.sizeValue.text = Constants.PLANET_DEFAULT_SIZE.ToString();

        this.open_databox();
        this.update_identity();
    }

    public void Initialize_Load(SerializedPlanet data){
        this.data = data;

        this.team_dropdown.value = (int) this.data.team;

        this.initPopSlider.maxValue = Constants.PLANET_DEFAULT_MAX_POPULATION;
        this.initPopSlider.value = this.data.initial_population;
        this.initialPopValue.text = this.initPopSlider.value.ToString();

        this.maxPopSlider.maxValue = Constants.PLANET_ABSOLUTE_MAX_POPULATION;
        this.maxPopSlider.value = this.data.population_max;
        this.maxPopValue.text = this.maxPopSlider.value.ToString();

        this.sizeSlider.maxValue = Constants.PLANET_MAX_SIZE * 2.0f;
        this.sizeSlider.value = this.data.planet_size * 2.0f;
        this.sizeSlider.minValue = Constants.PLANET_MIN_SIZE * 2.0f;
        this.sizeValue.text = this.sizeSlider.value.ToString();

        this.close_databox();
        this.update_position();
        this.update_identity();
    }
    public void update_identity()
    {
        this.m_SpriteRenderer.transform.localScale = new Vector3(this.data.planet_size * 0.1f, this.data.planet_size * 0.1f, this.data.planet_size * 0.1f);
        this.m_SpriteRenderer.color = Constants.team_colors[this.data.team];
        this.update_position();
    }

    public void update_position(){
        this.data.position_x = this.transform.position.x;
        this.data.position_y = this.transform.position.y;
        this.positionValue.text = "X : " + this.data.position_x + "\nY : " + this.data.position_y;
    }

    public void move(Vector2 new_position){
        this.transform.position = new Vector3(new_position.x, new_position.y, this.transform.position.z);
        this.update_position();
    }

    public void change_team(int unused_value){
        this.data.team = (Team) this.team_dropdown.value;
        this.update_identity();
    }

    public void change_initPop(){
        this.data.initial_population = Mathf.FloorToInt(this.initPopSlider.value);
        this.initialPopValue.text = this.data.initial_population.ToString();
        this.update_identity();
    }

    public void change_maxPop(){
        this.data.population_max = Mathf.FloorToInt(this.maxPopSlider.value);
        this.maxPopValue.text = this.data.population_max.ToString();
        this.initPopSlider.maxValue = this.data.population_max;
        this.update_identity();
    }

    public void changeSize(){
        this.data.planet_size = this.sizeSlider.value / 2.0f;
        this.sizeValue.text =  this.data.planet_size.ToString();
        this.update_identity();
    }

    
    public SerializedPlanet get_data(){
        return this.data;
    }
    public void open_databox()
    {
        this.data_Box.gameObject.SetActive(true);
    }

    public void close_databox()
    {
        this.data_Box.gameObject.SetActive(false);
    }

    public void destroy(){
        GameObject.Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
