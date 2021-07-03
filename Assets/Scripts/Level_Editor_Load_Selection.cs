using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_Editor_Load_Selection : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartGame(){
        this.gameObject.SetActive(false);
        //SceneManager.LoadScene("DemoLevel");
    }
    
    public void StartEditor(){
        SceneManager.LoadScene("Level_Editor");
    }
    public void StartLoadSelection(){
    }
    /*public void ShowLevels(){
        //SceneManager.LoadScene("Level_Editor_Load_Selection");
        Debug.Log("Show Levels called");
        string[] level_names = System.IO.Directory.GetFiles(Constants.USER_LEVEL_DIRECTORY_PATH + "/levels/");
        Debug.Log("list length is : "+level_names.Length.ToString());
        foreach(string level_name in level_names)
        {
            Debug.Log("level: "+level_name);
        }
        //Level level = JsonUtility.FromJson<Level>(level_json);


    }*/

}
