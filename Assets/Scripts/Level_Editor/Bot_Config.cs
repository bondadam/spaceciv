using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Bot_Config : MonoBehaviour
{
    public Image image;

    public Sprite[] team_sprites;

    public Text team_name;

    public TMP_Dropdown type_dropdown;

    public Slider speed_slider;

    public TMP_Text speed_label;

    private SerializedBot data;
    // Start is called before the first frame update

    public void Initialize_New(Team assigned_team)
    {
        this.Initialize(new SerializedBot(assigned_team, Constants.BOT_DEFAULT_TYPE, Constants.BOT_DEFAULT_SPEED));

    }

    public void Initialize(SerializedBot serializedBot){

        this.data = serializedBot;

        // SETUP TEAM
        this.team_name.text = Constants.team_names[this.data.team];

        this.image.sprite = this.team_sprites[(int)this.data.team];
        Bot_Type[] bot_types = (Bot_Type[])Bot_Type.GetValues(typeof(Bot_Type));
        List<string> bot_options = new List<string>();
        for (int i = 0; i < bot_types.Length; i++){
            bot_options.Add(Constants.bot_names[bot_types[i]]);
        }

        this.type_dropdown.ClearOptions();
        this.type_dropdown.AddOptions(bot_options); // this is your required solutio

        // SETUP TYPE
        this.type_dropdown.value = (int)this.data.type;
        
        // SETUP SPEED
        this.speed_slider.maxValue = Constants.BOT_DEFAULT_MAX_SPEED*2.0f;
        this.speed_slider.value = this.data.decision_interval*2.0f;
        this.speed_slider.minValue = Constants.BOT_DEFAULT_MIN_SPEED*2.0f;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void change_speed(float new_speed)
    {
        float calculated_speed = new_speed/2.0f;
        this.data.decision_interval = calculated_speed;
        this.speed_label.text = calculated_speed.ToString();
    }

    public void change_type(int new_type)
    {
        this.data.type = (Bot_Type) new_type;
    }

    public void change_team(Team new_team)
    {
        this.data.team = new_team;
        this.image.color = Constants.team_colors[this.data.team];
    }

    public SerializedBot get_data()
    {
        return this.data;
    }
}
