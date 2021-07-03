using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_Editor_Menu : MonoBehaviour
{
    public void StartGame(){
        this.gameObject.SetActive(false);
        //SceneManager.LoadScene("DemoLevel");
    }
    
    public void StartEditor(){
		PlayerPrefs.SetString(Constants.EDITOR_CURRENT_LEVEL_NAME_PLAYER_PREF,"");
        SceneManager.LoadScene("Level_Editor");
    }
    
    public void StartLoadSelection(){
        SceneManager.LoadScene("Level_Editor_Load_Selection");
    }
    public void back(){
        SceneManager.LoadScene("Menu_Main");
    }

}
