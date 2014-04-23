using UnityEngine;
using System.Collections;

public class StaticTexts
{
	private string[] _GameController_EndGame = {"Kraj igre!", "Game over!"};
	private string[] _GameController_PickUpsLeft = {"Preostalo: ", "Remaining: "};
	private string[] _GameController_Time = {"Vreme: ", "Time: "};
	private string[] _GameController_Win = {"Pobeda!", "Win!"};
	private static string[] _MainMenu_Option1 = {"Igraj oflajn", "Play offline"};
	private static string[] _MainMenu_Option2 = {"Igraj onlajn", "Play online"};
	private static string[] _MainMenu_Option3 = {"Rankiranja", "Rankings"};
	private static string[] _MainMenu_Option4 = {"Opcije", "Options"};
	private static string[] _MainMenu_Option5 = {"Izadji", "Exit"};
	private string[][] _MainMenu_Options = new string[][]{_MainMenu_Option1,
															_MainMenu_Option2,
															_MainMenu_Option3,
															_MainMenu_Option4,
															_MainMenu_Option5};
	private string[] _MainMenu_Header = {"Glavni meni", "Main menu"};
	private string[] _MainMenu_NotYourProfile = {"Nije vas profil?", "Not your profile?"};

	public string MainMenu_NotYourProfile()
	{
		return _MainMenu_NotYourProfile[PlayerPrefs.GetInt ("SelectedLanguage", 0)];
	}

	public string MainMenu_Header()
	{
		return _MainMenu_Header[PlayerPrefs.GetInt ("SelectedLanguage", 0)];
	}

	public string MainMenu_Option(int opt_num)
	{
		return _MainMenu_Options[opt_num][PlayerPrefs.GetInt ("SelectedLanguage", 0)];
	}

	public int MainMenu_OptionLength()
	{
		return _MainMenu_Options.Length;
	}

	public string GameController_Win()
	{
		return _GameController_Win[PlayerPrefs.GetInt ("SelectedLanguage", 0)];
	}

	public string GameController_Time()
	{
		return _GameController_Time[PlayerPrefs.GetInt ("SelectedLanguage", 0)];
	}

	public string GameController_PickUpsLeft()
	{
		return _GameController_PickUpsLeft[PlayerPrefs.GetInt ("SelectedLanguage", 0)];
	}

	public string GameController_EndGame()
	{
		return _GameController_EndGame[PlayerPrefs.GetInt ("SelectedLanguage", 0)];
	}

	protected StaticTexts(){}
	private static StaticTexts _instance = null;
	public static StaticTexts Instance
	{
		get
		{
			return StaticTexts._instance == null ? new StaticTexts() : StaticTexts._instance;
		}
	}
}
