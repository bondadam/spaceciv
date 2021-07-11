using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Main : MonoBehaviour
{
    public void Start()
    {
        if(Utils.levels_completed.Count == 0)
        {
            foreach ((string, Level_Difficulty) level in Constants.level_paths)
            {
                int level_completed = PlayerPrefs.GetInt(level.Item1,0);
                Utils.levels_completed.Add(level_completed);
            }
        }
    }

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
