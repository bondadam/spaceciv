using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class Game_Over_Menu : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject game_over_panel;
    public GameObject win_label;
    public GameObject loss_label;

    public Text time_taken_field;

    public Text ships_lost_field;

    public Text planets_conquered_field;

    public Level_Manager level_Manager;

    private float time_taken;
    void Start()
    {
        game_over_panel = this.gameObject;
        show_label(victory.hide_all);
        game_over_panel.SetActive(false);

    }

    public enum victory{
        win,
        loss,
        hide_all
    }

    private void show_label(victory which){
        switch(which){
            case victory.hide_all:
                win_label.SetActive(false);
                loss_label.SetActive(false);
                break;
            case victory.win:
                win_label.SetActive(true);
                loss_label.SetActive(false);
                break;
            case victory.loss:
                win_label.SetActive(false);
                loss_label.SetActive(true);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void end_game(bool won, float time_taken)
    {
        game_over_panel.SetActive(true);
        time_taken_field.text = "Time taken: " + Math.Round(time_taken, 2).ToString();
        if(won){
            show_label(victory.win);
        } else {
            show_label(victory.loss);
        }
    } 
    public void restart_game()
    {
        game_over_panel.SetActive(false);
        show_label(victory.hide_all);
        level_Manager.Initialize();
    }

    public void return_to_menu(){
        SceneManager.LoadScene("MainMenu");
    }
}

