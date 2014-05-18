using UnityEngine;
using System.Collections;

public class StaticTexts 
{
	public string web_api_location = "http://milosjankovic.site11.com/games_data/iconsdeathmatch/iconsdeathmatchapi.php";

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
