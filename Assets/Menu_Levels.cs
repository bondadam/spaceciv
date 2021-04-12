using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class Menu_Levels : MonoBehaviour
{
    public GameObject content;
    public GameObject prefabButton;
    public RectTransform ParentPanel;



    // Start is called before the first frame update
    void Start()
    {
        foreach(KeyValuePair<int, string> entry in Constants.level_paths)
        {
            GameObject goButton = (GameObject)Instantiate(prefabButton);
            goButton.transform.SetParent(ParentPanel, false);
            goButton.transform.localScale = new Vector3(1, 1, 1);
            Button tempButton = goButton.GetComponent<Button>();
            goButton.GetComponentInChildren<TextMeshProUGUI>().text = "Level " + entry.Key.ToString();
            tempButton.onClick.AddListener(() => ButtonClicked(entry.Key, ""));
        }
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
     
     }

     void ButtonClicked(int buttonNo, string level_name)
     {
         Debug.Log ("Button clicked = " + buttonNo);
         start_level(buttonNo, level_name);
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

    public void enable(){
        this.gameObject.SetActive(true);
    }

    public void disable(){
        this.gameObject.SetActive(false);
    }
}
