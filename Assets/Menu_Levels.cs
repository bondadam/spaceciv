using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class Menu_Levels : MonoBehaviour
{
    public GameObject prefabButton;
    public Transform grid;



    // Start is called before the first frame update
    void Start()
    {
        /*
        List<string> save_file_names = Save_File_Manager.getSimpleFileNames();
        foreach(string file_name in save_file_names)
        {
            GameObject userLevelButton = (GameObject)Instantiate(prefabButton);
            userLevelButton.transform.SetParent(ParentPanel, false);
            userLevelButton.transform.localScale = new Vector3(1, 1, 1);
            Button userLevelTempButton = userLevelButton.GetComponent<Button>();
            userLevelButton.GetComponentInChildren<TextMeshProUGUI>().text = file_name;
            userLevelTempButton.onClick.AddListener(() => ButtonClicked(Constants.USER_LEVEL_CODE, file_name));
        }
        this.disable();          
        */

        foreach(KeyValuePair<int, string> entry in Constants.level_paths)
        {
            var custom = false;

            GameObject button_object = Instantiate(this.prefabButton) as GameObject;
			button_object.AddComponent<level_button>();
			button_object.SetActive(true);
		    level_button lb = button_object.GetComponent<level_button>();
			lb.initialize(custom, entry.Key, Constants.level_difficulties[entry.Key]);
			button_object.transform.SetParent(grid);
            Button button = button_object.GetComponent<Button>();
            button.onClick.AddListener(() => level_clicked(entry.Key, custom));
        }
     }

    public void level_clicked(int number, bool custom){
        this.start_level(number, "");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void start_level(int level, string level_name){
        Utils.selected_level = level;
        Utils.selected_custom_level = level_name;
        SceneManager.LoadScene("DemoLevel");
    }

    public void back(){
        SceneManager.LoadScene("Menu_Main");
    }
}
