using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System;

class PlayerProfile
{
	private int _id;
	private string _name;

	public string GetName()
	{
		return _name;
	}

	public int GetId()
	{
		return _id;
	}

	public PlayerProfile(int id, string name)
	{
		_id = id;
		_name = name;
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
	private float scrollWidth = 300;
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

		newNameInput = StaticTexts.Instance.PlayerProfiles_NewNameDefault ();
		playerProfiles = new List<PlayerProfile> ();
		StartCoroutine (SetupPlayersList());
	}

	IEnumerator SetupPlayersList()
    {
		string sqlCommand;

		sqlCommand = "SELECT COUNT(*) FROM PlayerProfiles";
		int profilesCount = DBController.Instance.ExecuteSqlForIntScalar (sqlCommand);

		for (int i=0; i<profilesCount; i++) 
		{
			sqlCommand = "SELECT id, name FROM PlayerProfiles LIMIT 1 OFFSET " + i.ToString() + ";";
			object[] sqlReader = new object[2];
			DBController.Instance.ExecuteSqlForReader (sqlCommand, sqlReader);

			playerProfiles.Add(new PlayerProfile(Convert.ToInt32(sqlReader[0].ToString()),
			                                     sqlReader[1].ToString()));
		}

		yield return new WaitForSeconds (0);
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
			string command = "INSERT INTO PlayerProfiles (name, money, selected_vehicle_id, selected_weapon_id)"
								+" VALUES ('"+newNameInput+"', 2000, 1, 1);";
			DBController.Instance.ExecuteSqlForNonQuery(command);
			playerProfiles.Clear();
			StartCoroutine (SetupPlayersList());
		}
	}

	void DeleteProfile(int profileId, string profileName)
	{
		if (EditorUtility.DisplayDialog (StaticTexts.Instance.PlayerProfiles_DeleteDialogTitle(),
		                                 StaticTexts.Instance.PlayerProfiles_DeleteDialogMessage() + profileName,
		                                 StaticTexts.Instance.Yes(),
		                                 StaticTexts.Instance.No())) 
		{
			string sqlCommand;

			sqlCommand= "DELETE FROM PlayerProfiles WHERE id=" + profileId.ToString() + ";";
			DBController.Instance.ExecuteSqlForNonQuery (sqlCommand);

			playerProfiles.Clear ();
			StartCoroutine (SetupPlayersList ());
		}
	}

	void SelectProfile(int profile_list_index)
	{
		PlayerPrefs.SetInt ("ChoosenProfileId", playerProfiles[profile_list_index].GetId());
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
			if(GUI.Button(new Rect(scrollRectPosition.x, scrollRectPosition.y + (40 * i), 200, 20), playerProfiles[i].GetName()))
			{
				SelectProfile(i);
			}
			if(GUI.Button(new Rect(scrollRectPosition.x + 200, scrollRectPosition.y + (40 * i), 25, 20), "X"))
			{
				DeleteProfile(playerProfiles[i].GetId(), playerProfiles[i].GetName());
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
