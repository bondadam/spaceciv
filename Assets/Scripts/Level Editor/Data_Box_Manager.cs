using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Data_Box_Manager : MonoBehaviour
{
    private SerializedPlanet temp_data;

    public TMP_Dropdown team_dropdown;
    public TextMeshProUGUI title;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initialize(SerializedPlanet data){
        this.temp_data = new SerializedPlanet();
        this.temp_data.team = data.team;
        this.temp_data.position_x = data.position_x;
        this.temp_data.position_y = data.position_y;
        this.temp_data.initial_population = data.initial_population;
        this.temp_data.population_max = data.population_max;
        this.temp_data.planet_size = data.planet_size;


        this.team_dropdown.value = (int) this.temp_data.team;
    }

    public void change_team(int value){
        this.temp_data.team = (Team) this.team_dropdown.value;
    }

    public SerializedPlanet get_data(){
        return this.temp_data;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
