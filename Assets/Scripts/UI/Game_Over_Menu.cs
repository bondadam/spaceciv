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
    private bool game_over;
    private GameObject game_over_panel;
    public GameObject win_label;
    public GameObject loss_label;
    public TMP_Text time_taken_field;
    public Level_Manager level_Manager;
    void Start()
    {
        game_over_panel = this.gameObject;
        show_label(victory.hide_all);
        game_over_panel.SetActive(false);
        this.game_over = false;
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


    public void end_game(bool won, float target_time)
    {
        this.game_over = true;
        game_over_panel.SetActive(true);
        Debug.Log("Time Taken : " + LevelStatsKeeper.get_timer());
        time_taken_field.text = "Time taken: " + Math.Round(LevelStatsKeeper.get_timer(), 2).ToString() + "s";
        if(won){
            if(Utils.selected_level!=999)
            {
                int level_score;
                if(LevelStatsKeeper.get_timer() <= target_time)
                {
                    level_score = 3;
                }else{
                    level_score = 1;
                }
                string level_path = Constants.level_paths[Utils.selected_level].Item1;
                Utils.levels_completed[Utils.selected_level] = Math.Max(Utils.levels_completed[Utils.selected_level],level_score);
                PlayerPrefs.SetInt(level_path, Utils.levels_completed[Utils.selected_level]);
                PlayerPrefs.Save();
            }
            show_label(victory.win);
        } else {
            show_label(victory.loss);
        }
    } 
    public void restart_game()
    {
        SceneManager.LoadScene("Level");
    }
    public void play_next_level()
    {
        if(Utils.selected_level >= Constants.level_paths.Count-1)
        {
            return_to_menu();
        }else{
            Utils.selected_level = Utils.selected_level + 1;
            SceneManager.LoadScene("Level");
        }
    }

    public void return_to_menu(){
        SceneManager.LoadScene("Menu_Main");
    }
}

