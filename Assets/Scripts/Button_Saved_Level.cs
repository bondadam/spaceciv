using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Button_Saved_Level :  Button {

	private string Name;
	public Text ButtonText;
	public ScrollView_Saved_Levels ScrollView;


	public void SetName(string name)
	{
		Name = name;
		ButtonText.text = name;
	}
	public void Button_Click()
	{
		ScrollView.ButtonClicked(Name);

	}
}