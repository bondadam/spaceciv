using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartGame(){
        SceneManager.LoadScene("DemoLevel");
    }

    public void ExitGame(){
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

}
