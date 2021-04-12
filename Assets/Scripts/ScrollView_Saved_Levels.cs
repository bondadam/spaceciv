using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ScrollView_Saved_Levels : MonoBehaviour {

	public GameObject butt;
	private List<string> NameList = new List<string>();


	// Use this for initialization
	void Start () {
        List<string> NameList = Save_File_Manager.getSimpleFileNames();
		int i = 0;
		foreach(string myStr in NameList)
		{
			GameObject game_object = Instantiate(butt) as GameObject;
			game_object.AddComponent<Button_Saved_Level>();
			game_object.SetActive(true);
		    Button_Saved_Level TB = game_object.GetComponent<Button_Saved_Level>();
			TB.SetName(myStr);
			game_object.transform.SetParent(butt.transform.parent);
			Vector3 pos = butt.transform.parent.position;
			pos.y -= (40*i);
 			game_object.transform.position = pos;
			i += 1;
		}


	}
	
	public void ButtonClicked(string str)
	{
		Debug.Log(str + " button clicked.");
		PlayerPrefs.SetString(Constants.EDITOR_CURRENT_LEVEL_NAME_PLAYER_PREF,str);
        SceneManager.LoadScene("Level_Editor");


	}
}