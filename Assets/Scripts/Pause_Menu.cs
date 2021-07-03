using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause_Menu : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject pause_panel;
    void Start()
    {
        pause_panel = this.gameObject;
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
        SceneManager.LoadScene("DemoLevel");
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

    public void return_to_menu(){
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu_Main");
    }

    public void back(){
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu_Level");
    }
}
