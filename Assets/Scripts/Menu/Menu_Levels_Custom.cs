using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class Menu_Levels_Custom : MonoBehaviour
{
    public GameObject prefabButton;
    public Transform grid;



    // Start is called before the first frame update
    void Start()
    {
        
        List<string> save_file_names = Save_File_Manager.getAllSimpleFileNames();
        foreach(string file_name in save_file_names)
        {
            GameObject userLevelButton = (GameObject)Instantiate(prefabButton);
            userLevelButton.transform.SetParent(grid, false);
            userLevelButton.transform.localScale = new Vector3(1, 1, 1);
            Button userLevelTempButton = userLevelButton.GetComponent<Button>();
            userLevelButton.GetComponentInChildren<TMP_Text>().text = file_name;
            userLevelTempButton.onClick.AddListener(() => level_clicked_custom(Constants.USER_LEVEL_CODE, file_name));
        }        
     }

    public void level_clicked_custom(int level_code, string level_name){
        Debug.Log("Button clicked");
        Utils.selected_level = level_code;
        Utils.selected_custom_level = level_name;
        SceneManager.LoadScene("DemoLevel");
    }

    public void back(){
        SceneManager.LoadScene("Level_Editor_Menu");
    }
}
