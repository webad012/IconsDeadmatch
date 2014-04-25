using UnityEngine;
using System.Collections;

public class PlayMenuController : MonoBehaviour 
{
	private Rect headerArea;
	private int headerHeight = 40;
	
	private Rect contentArea;
	private float menuOptionAreaWight;
	private float menuOptionAreaHeight;
	private float menuOptionWidth = 100;
	private float menuOptionHeight = 30;

	private int selectedMenuOption = 0;

	// Use this for initialization
	void Start () 
	{
		headerArea = new Rect (10, 0, Screen.width-10, headerHeight);
		contentArea = new Rect (10, headerHeight, Screen.width-10, Screen.height - headerHeight);

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
		GUI.Label (headerArea, "<size=30>"+StaticTexts.Instance.PlayMenu_Header()+"</size>");
		// content
		for (int i=0; i < StaticTexts.Instance.PlayMenu_OptionsLength(); i++) 
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
			           StaticTexts.Instance.PlayMenu_Options(i));
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.DownArrow)
		    || Input.GetKeyDown (KeyCode.RightArrow)) 
		{
			selectedMenuOption++;
			if(selectedMenuOption == StaticTexts.Instance.PlayMenu_OptionsLength())
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
				selectedMenuOption = StaticTexts.Instance.PlayMenu_OptionsLength() - 1;
			}
		}
	}
}
