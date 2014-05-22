using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelSelectorController : MonoBehaviour 
{
	public GUIStyle levelGuiStyle;
	public GUIText statusText;
	public List<GUIContent> levels;

	private Rect contentArea;
	private float menuLevelsAreaWight;
	private float menuLevelsAreaHeight;
	private float menuLevelWidth = 200;
	private float menuLevelHeight = 100;

	// Use this for initialization
	void Start () 
	{
		contentArea = new Rect (10, 10, Screen.width - 20, Screen.height - 20);

		menuLevelsAreaWight = levels.Count * menuLevelWidth;
		if (menuLevelsAreaWight > contentArea.width) 
		{
			menuLevelsAreaWight = contentArea.width;
		}
		menuLevelsAreaHeight = levels.Count * menuLevelHeight;
		if (menuLevelsAreaHeight > contentArea.height) 
		{
			menuLevelsAreaHeight = contentArea.height;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnGUI()
	{
		for (int i=0; i < levels.Count; i++) 
		{
			float menuLevelX = contentArea.width/2 - menuLevelsAreaWight/2 + (menuLevelWidth * i);
			float menuLevelY = contentArea.height/2 - menuLevelsAreaHeight/2 + (menuLevelHeight * i);
			if(GUI.Button(new Rect(menuLevelX,
			                       menuLevelY,
			                       menuLevelWidth,
			                       menuLevelHeight), 
			              levels[i], levelGuiStyle))
			{
				PlayerPrefs.SetInt("SelectedLevel", i);
				Application.LoadLevel("Default");
			}
		}

		/*int y = 10;
		for(int i = 0; i < levels.Count; i++)
		{
			if(GUI.Button(new Rect(10, y, 200, 50), levels[i].levelTexture, levelGuiStyle))
			{
				//Application.LoadLevel(lvlNames[i]);
			}
			y += 60;
		}*/
	}

	void AnimateLevelSwitch()
	{

	}
}
