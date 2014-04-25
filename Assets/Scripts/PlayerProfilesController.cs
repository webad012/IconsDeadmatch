using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;

class PlayerProfile
{
	private string _name;
	private int _level;
	//private int _health;
	//private int _move_speed;
	//private int _turn_speed;
	//private int _damage;

	public string GetName()
	{
		return _name;
	}

	public string GetDisplayName()
	{
		return _name + " - lvl" + _level.ToString();
	}

	//public PlayerProfile(string name, int level, int health, int move_speed, int turn_speed, int damage)
	public PlayerProfile(string name, int level)
	{
		_name = name;
		_level = level;
		//_health = health;
		//_move_speed = move_speed;
		//_turn_speed = turn_speed;
		//_damage = damage;
	}
}

public class PlayerProfilesController : MonoBehaviour 
{
	public GUIText statusText;

	private Rect headerArea;
	private int headerHeight = 40;
	private Rect contentArea;
	private Rect scrollRectPosition;
	private Rect scrollRectView;
	private Vector2 scrollPosition = Vector2.zero;
	//private Vector2 scrollPosition;
	private float scrollWidth = 300;
	//private float scrollHeigth;
	private float createNewTextFieldHeight = 20;
	private float createNewHeight = 40;
	private List<PlayerProfile> playerProfiles;
	private string newNameInput;
	private float statusTextStartTime;
	private float statusTextTimeout = 5;

	// Use this for initialization
	void Start () 
	{
		statusText.text = "";

		headerArea = new Rect (10, 0, Screen.width-10, headerHeight);
		contentArea = new Rect (10, headerHeight, Screen.width-10, Screen.height - headerHeight);
		float scrollHeigth = contentArea.height - createNewHeight - 50;
		scrollRectPosition = new Rect (Screen.width/2 - scrollWidth/2, contentArea.y + createNewHeight, scrollWidth, scrollHeigth);
		//scrollPosition = new Vector2 (Screen.width/2 - scrollWidth/2, contentArea.y + createNewHeight);
		//scrollRectView = new Rect (0, 0, 100, 100);
		//scrollRectView = scrollRectPosition;

		newNameInput = StaticTexts.Instance.PlayerProfiles_NewNameDefault ();
		playerProfiles = new List<PlayerProfile> ();
		StartCoroutine (SetupPlayersList());
	}

	IEnumerator SetupPlayersList()
    {
		string command = "SELECT name, level, health, move_speed, turn_speed, damage FROM PlayerProfiles;";
		SQLiteDataReader sqlReader = DBController.Instance.ExecuteSqlForReader (command);
		while (sqlReader.Read()) 
		{
			playerProfiles.Add(new PlayerProfile(sqlReader.GetString(0),
			                                     sqlReader.GetInt32(1)));//,
			                                     //sqlReader.GetInt32(2),
			                                     //sqlReader.GetInt32(3),
			                                     //sqlReader.GetInt32(4),
			                                     //sqlReader.GetInt32(5)));
		}

		yield return new WaitForSeconds (0);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void CreateNewProfile()
	{
		bool found = false;

		for (int i=0; i<playerProfiles.Count; i++) 
		{
			if(string.Compare(playerProfiles[i].GetName(), newNameInput) == 0)
			{
				statusText.text = StaticTexts.Instance.PlayerProfiles_NameTaken();
				statusTextStartTime = Time.time;
				found = true;
				return;
			}
		}

		if (found == false) 
		{
			string command = "INSERT INTO PlayerProfiles (name, level, health, move_speed, turn_speed, damage) VALUES ('"+newNameInput+"', 1, 100, 1, 10, 1);";
			DBController.Instance.ExecuteSqlForNonQuery(command);
			playerProfiles.Clear();
			StartCoroutine (SetupPlayersList());
		}
	}

	void DeleteProfile(string profileName)
	{
		if (EditorUtility.DisplayDialog (StaticTexts.Instance.PlayerProfiles_DeleteDialogTitle(),
		                                 StaticTexts.Instance.PlayerProfiles_DeleteDialogMessage() + profileName,
		                                 StaticTexts.Instance.Yes(),
		                                 StaticTexts.Instance.No())) 
		{
			string command = "DELETE FROM PlayerProfiles WHERE name='" + profileName + "';";
			DBController.Instance.ExecuteSqlForNonQuery (command);
			playerProfiles.Clear ();
			StartCoroutine (SetupPlayersList ());
		}
	}

	void SelectProfile(string profileName)
	{
		PlayerPrefs.SetString ("ChosenProfileName", profileName);
		Application.LoadLevel ("MainMenu");
	}

	void OnGUI()
	{
		GUI.Label (headerArea, "<size=30>"+StaticTexts.Instance.PlayerProfiles_Header()+"</size>");

		newNameInput = GUI.TextField (new Rect(contentArea.x, contentArea.y, 100, createNewTextFieldHeight), newNameInput);
		if (GUI.Button (new Rect(contentArea.x + 110, contentArea.y, 100, createNewTextFieldHeight), StaticTexts.Instance.PlayerProfiles_CreateNewButton())) 
		{
			if(string.Compare(newNameInput, StaticTexts.Instance.PlayerProfiles_NewNameDefault ()) != 0)
			{
				CreateNewProfile();
			}
		}

		
		scrollRectView = new Rect (scrollRectPosition.x, scrollRectPosition.y, scrollRectPosition.width, playerProfiles.Count*40);
		scrollPosition = GUI.BeginScrollView (scrollRectPosition, scrollPosition, scrollRectView, true, false);
		for (int i=0; i<playerProfiles.Count; i++) 
		{
			if(GUI.Button(new Rect(scrollRectPosition.x, scrollRectPosition.y + (40 * i), 200, 20), playerProfiles[i].GetDisplayName()))
			{
				SelectProfile(playerProfiles[i].GetName());
			}
			if(GUI.Button(new Rect(scrollRectPosition.x + 200, scrollRectPosition.y + (40 * i), 25, 20), "X"))
			{
				DeleteProfile(playerProfiles[i].GetName());
			}
			//GUI.Button(new Rect(scrollRectPosition.x + 230, scrollRectPosition.y + (40 * i), 100, 20), StaticTexts.Instance.PlayerProfiles_Rename());
		}
		GUI.EndScrollView();


		float guiTime = Time.time - statusTextStartTime;
		int restSeconds = Mathf.CeilToInt (statusTextTimeout - guiTime);
		if (restSeconds == 0) 
		{
			statusText.text = "";
		}
	}
}
