using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour 
{
	public GUIText statusText;
	public Texture startButtonTexture;
	public Texture scoresButtonTexture;
	public Texture suggestButtonTexture;
	public GUIStyle buttonStyle;
	
	private ComboBox versusesComboBox;
	private ComboBox teamsComboBox;
	
	private GUIContent[] versusesList;
	private GUIContent[] teamsList;
	private GUIStyle listStyle;
	
	private Rect versusesComboBoxRect;
	private Rect teamsComboBoxRect;
	private Rect startButtonRect;
	private Rect scoresButtonRect;
	private Rect suggestButtonRect;
	private Rect loginUsernameRect;
	private Rect loginPasswordRect;
	private Rect loginoutButtonRect;
	private Rect createProfiletButtonRect;
	private Rect recoverPasswordButtonRect;
	private Rect selectedUsernameRect;
	
	private int selectedVersuses = 0;
	private int oldSelectedVersuses  = -1;
	private int selectedTeam = 0;
	
	private string inputPassword = "";
	private string inputUsername = "";
	
	private float statusTextStartTime;
	private float statusTextTimeout = 5f;
	
	private bool versusesListLodaded = false;

	// Use this for initialization
	void Start () 
	{
		StartCoroutine(LoadVersuses());
		
		startButtonRect = new Rect(Screen.width/2 - 100, Screen.height/2 - 200, 200, 200);
		scoresButtonRect = new Rect(Screen.width/2 - 200, Screen.height/2 - 100, 100, 100);
		suggestButtonRect = new Rect(Screen.width/2 + 100, Screen.height/2 - 100, 100, 100);
		versusesComboBoxRect = new Rect(Screen.width/2 - 200, Screen.height/2, 200, 20);
		teamsComboBoxRect = new Rect(Screen.width/2, Screen.height/2, 200, 20);
		loginoutButtonRect = new Rect(Screen.width - 100, 0, 100, 20);
		loginPasswordRect = new Rect(Screen.width - 200, 0, 100, 20);
		loginUsernameRect = new Rect(Screen.width - 300, 0, 100, 20);
		createProfiletButtonRect = new Rect(Screen.width - 100, 20, 100, 20);
		recoverPasswordButtonRect = new Rect(Screen.width - 250, 20, 150, 20);
		selectedUsernameRect = new Rect(Screen.width - 300, 0, 200, 20);
		
		// Make a GUIStyle that has a solid white hover/onHover background to indicate highlighted items
		listStyle = new GUIStyle();
		listStyle.normal.textColor = Color.white;
		var tex = new Texture2D(2, 2);
		var colors = new Color[4] {Color.white, Color.white, Color.white, Color.white};
		tex.SetPixels(colors);
		tex.Apply();
		listStyle.hover.background = tex;
		listStyle.onHover.background = tex;
		listStyle.padding.left = listStyle.padding.right = listStyle.padding.top = listStyle.padding.bottom = 4;
		
		statusText.text = "";
	}

	void OnGUI()
	{
		if(PlayerPrefs.GetInt ("LoggedIn", 0) == 0)
		{
			inputUsername = GUI.TextField (loginUsernameRect, inputUsername);
			inputPassword = GUI.PasswordField (loginPasswordRect, inputPassword, "*"[0], 25);
			if(GUI.Button(loginoutButtonRect, "Login"))
			{
				StartCoroutine(PerformLogin());
			}
			if(GUI.Button(createProfiletButtonRect, "Create Profile"))
			{
				Application.LoadLevel("CreateProfile");
			}
			if(GUI.Button(recoverPasswordButtonRect, "Recover Password"))
			{
				if(inputUsername == "")
				{
					statusText.text = "Please enter username";
					statusTextStartTime = Time.time;
				}
				else
				{
					StartCoroutine(PerformPasswordRecovery());
				}
			}
		}
		else
		{
			GUI.Label(selectedUsernameRect, PlayerPrefs.GetString("SelectedProfileUsername", "NaN"));
			if(GUI.Button(loginoutButtonRect, "Logout"))
			{
				PerformLogout();
			}
		}
		
		if(versusesListLodaded == true)
		{
			selectedVersuses = versusesComboBox.Show();
			
			if(selectedVersuses != oldSelectedVersuses)
			{
				oldSelectedVersuses = selectedVersuses;
				//selectedTeam = 0;
				
				string[] split = versusesList[selectedVersuses].text.Split("-"[0]);
				teamsList = new GUIContent[2];
				teamsList[0] = new GUIContent(split[0].Trim());
				teamsList[1] = new GUIContent(split[1].Trim());
				
				teamsComboBox = new ComboBox(teamsComboBoxRect, teamsList[selectedTeam], teamsList, listStyle);
			}
			
			selectedTeam = teamsComboBox.Show();
		}
		
		if(GUI.Button(startButtonRect, startButtonTexture, buttonStyle))
		{
			if(PlayerPrefs.GetInt ("LoggedIn", 0) == 0 || versusesListLodaded == false)
			{
				statusText.text = "Need to login first";
				statusTextStartTime = Time.time;
			}
			else
			{
				int enemyTeam = (selectedTeam+1)%2;
				PlayerPrefs.SetInt("SelectedTeamint", selectedTeam);
				PlayerPrefs.SetString("SelectedUsername", inputUsername);
				PlayerPrefs.SetString("SelectedTeam", teamsList[selectedTeam].text);
				PlayerPrefs.SetString("EnemyTeam", teamsList[enemyTeam].text);
				Application.LoadLevel("Default");
			}
		}
		
		if(GUI.Button(scoresButtonRect, scoresButtonTexture, buttonStyle))
		{
			Application.LoadLevel("Scores");
		}
		
		if(GUI.Button(suggestButtonRect, suggestButtonTexture, buttonStyle))
		{
			Application.LoadLevel("SuggestVS");
		}
		
		float guiTime = Time.time - statusTextStartTime;
		int restSeconds = Mathf.CeilToInt (statusTextTimeout - guiTime);
		if (restSeconds == 0) 
		{
			statusText.text = "";
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	IEnumerator LoadVersuses()
	{
		statusText.text = "Loading versuses";
		statusTextStartTime = Time.time;

		WWW hs_get = new WWW(StaticTexts.Instance.web_api_location + "?action=GetVersuses");
		yield return hs_get;
		
		if (hs_get.error != null)
		{
			statusText.guiText.text = "There was an error loading in: " + hs_get.error;
		}
		else
		{
			string[] split = hs_get.text.Split("|"[0]);
			versusesList = new GUIContent[split.Length/2];
			for (var i = 0; i < split.Length; i+=2) 
			{
				versusesList[i/2] = new GUIContent(split[i] + " - " + split[i+1]);
			}
			
			versusesComboBox = new ComboBox(versusesComboBoxRect, versusesList[selectedVersuses], versusesList, listStyle);
			versusesListLodaded = true;
		}
	}

	IEnumerator PerformLogin()
	{
		statusText.text = "Loging in";
		statusTextStartTime = Time.time;
		
		WWW hs_get = new WWW(StaticTexts.Instance.web_api_location + "?action=ValidateProfile&Username="+inputUsername+"&Password="+inputPassword);
		yield return hs_get;
		
		if (hs_get.error != null)
		{
			statusText.guiText.text = "There was an error loging in: " + hs_get.error;
		}
		else
		{
			if(hs_get.text == "OK")
			{
				PlayerPrefs.SetString("SelectedProfileUsername", inputUsername);
				PlayerPrefs.SetInt ("LoggedIn", 1);
				statusText.text = "";
			}
			else
			{
				statusText.text = "Error loging in: " + hs_get.text;
				statusTextStartTime = Time.time;
			}
		}
	}
	
	void PerformLogout()
	{
		PlayerPrefs.SetInt ("LoggedIn", 0);
	}
	
	IEnumerator PerformPasswordRecovery()
	{
		statusText.text = "Sending recovery request";
		statusTextStartTime = Time.time;
		
		WWW hs_get = new WWW(StaticTexts.Instance.web_api_location + "?action=RecoverPassword&Username="+inputUsername);
		yield return hs_get;
		
		if (hs_get.error != null)
		{
			statusText.guiText.text = "There was an error sending recovery request: " + hs_get.error;
		}
		else
		{
			if(hs_get.text == "OK")
			{
				statusText.text = "Password recovery successful, please check your email.";
				statusTextStartTime = Time.time;
			}
			else
			{
				statusText.text = "Error sending recovery request: " + hs_get.text;
				statusTextStartTime = Time.time;
			}
		}
	}
}
