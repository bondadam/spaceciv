using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause_Menu : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject pause_panel;
    public GameObject next_level_button;
    void Start()
    {
        pause_panel = this.gameObject;
        if(Utils.selected_level != Constants.USER_LEVEL_CODE && Utils.levels_completed[Utils.selected_level]==0)
        {
            next_level_button.SetActive(false);
        }
        pause_panel.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static bool get_paused(){
        return Time.timeScale == 0;
    }

    public void restart_game()
    {
        this.continue_game();
        SceneManager.LoadScene("Level");
    }

    public void pause_game()
    {
        Time.timeScale = 0;
        pause_panel.SetActive(true);
        Debug.Log("Game Paused");
        //Disable scripts that still work while timescale is set to 0
    } 
    public void continue_game()
    {
        Time.timeScale = 1;
        pause_panel.SetActive(false);
        //enable the scripts again
    }

    public void play_next_level()
    {
        continue_game();
        if(Utils.selected_level >= Constants.level_paths.Count-1)
        {
            return_to_menu();
        }else{
            Utils.selected_level = Utils.selected_level + 1;
            SceneManager.LoadScene("Level");
        }
    }
    public void return_to_menu(){
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu_Level");
    }

    public void back(){
        Time.timeScale = 1;
        if (Utils.selected_level == Constants.USER_LEVEL_CODE){
            SceneManager.LoadScene("Menu_Level_Custom");
        } else {
            SceneManager.LoadScene("Menu_Level");
        }
    }
}
