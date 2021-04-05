using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    public Menu_Levels menu_Levels;

    // Start is called before the first frame update
    public void StartGame(){
        this.menu_Levels.enable();
        this.gameObject.SetActive(false);
        //SceneManager.LoadScene("DemoLevel");
    }
    
    public void StartEditor(){
        SceneManager.LoadScene("Level_Editor");
    }

    public void ExitGame(){
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

}
