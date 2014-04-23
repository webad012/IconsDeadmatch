using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMenuController : MonoBehaviour 
{
	private Rect headerArea;
	private int headerHeight = 40;

	private Rect contentArea;
	private float menuOptionAreaWight;
	private float menuOptionAreaHeight;
	private float menuOptionWidth = 100;
	private float menuOptionHeight = 30;

	private int selectedMenuOption = 0;

	void Start()
	{
		headerArea = new Rect (0, 0, Screen.width, headerHeight);
		contentArea = new Rect (0, headerHeight, Screen.width, Screen.height - headerHeight);

		menuOptionAreaWight = StaticTexts.Instance.MainMenu_OptionLength () * menuOptionWidth;
		if (menuOptionAreaWight > contentArea.width) 
		{
			menuOptionAreaWight = contentArea.width;
		}
		menuOptionAreaHeight = StaticTexts.Instance.MainMenu_OptionLength () * menuOptionHeight;
		if (menuOptionAreaHeight > contentArea.height) 
		{
			menuOptionAreaHeight = contentArea.height;
        }
    }
    
    void OnGUI()
	{
		// header
		GUI.Label (headerArea, "<size=30>"+StaticTexts.Instance.MainMenu_Header()+"</size>");
		GUI.Label (new Rect(headerArea.width - menuOptionWidth,
		                    headerArea.y,
		                    menuOptionWidth,
		                    menuOptionHeight),
		           "<size=15>"+PlayerPrefs.GetString("ChosenProfileName", "Unknown")+"</size>");
		GUI.Button (new Rect(headerArea.width - menuOptionWidth*5/4,
		                     headerArea.y + menuOptionHeight,
		                     menuOptionWidth,
		                     menuOptionHeight),
                    StaticTexts.Instance.MainMenu_NotYourProfile());

		// content
		for (int i=0; i < StaticTexts.Instance.MainMenu_OptionLength(); i++) 
		{
			if(selectedMenuOption == i)
			{
				GUI.color = Color.white;
			}
			else
			{
				GUI.color = Color.grey;
			}

			float menuOptionX = contentArea.width/2 - menuOptionAreaWight/2 + (menuOptionWidth * i);
			float menuOptionY = contentArea.height/2 - menuOptionAreaHeight/2 + (menuOptionHeight * i);
			GUI.Button(new Rect(menuOptionX,
			                    menuOptionY,
			                    menuOptionWidth,
			                    menuOptionHeight), 
			           StaticTexts.Instance.MainMenu_Option(i));
        }
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.DownArrow)
		    || Input.GetKeyDown (KeyCode.RightArrow)) 
		{
			selectedMenuOption++;
			if(selectedMenuOption == StaticTexts.Instance.MainMenu_OptionLength())
			{
				selectedMenuOption = 0;
			}
		}
		if (Input.GetKeyDown (KeyCode.UpArrow)
		    || Input.GetKeyDown (KeyCode.LeftArrow)) 
		{
			selectedMenuOption--;
			if(selectedMenuOption < 0)
			{
				selectedMenuOption = StaticTexts.Instance.MainMenu_OptionLength() - 1;
            }
        }
	}
}
