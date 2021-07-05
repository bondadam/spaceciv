using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Main : MonoBehaviour
{

    // Start is called before the first frame update
    public void StartGame(){
        this.gameObject.SetActive(false);
        SceneManager.LoadScene("Menu_Level");
    }
    
    public void StartEditor(){
        SceneManager.LoadScene("Level_Editor_Menu");
    }

    public void ExitGame(){
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

}
