#pragma strict

public var statusText : GUIText;
public var startButtonTexture : Texture;
public var scoresButtonTexture : Texture;
public var suggestButtonTexture : Texture;
public var buttonStyle : GUIStyle;

private var versusesComboBox : ComboBox;
private var teamsComboBox : ComboBox;

private var versusesList : GUIContent[];
private var teamsList : GUIContent[];
private var listStyle : GUIStyle;

private var versusesComboBoxRect : Rect;
private var teamsComboBoxRect : Rect;
private var startButtonRect : Rect;
private var scoresButtonRect : Rect;
private var suggestButtonRect : Rect;
private var loginUsernameRect : Rect;
private var loginPasswordRect : Rect;
private var loginoutButtonRect : Rect;
private var createProfiletButtonRect : Rect;
private var recoverPasswordButtonRect : Rect;
private var selectedUsernameRect : Rect;

private var selectedVersuses : int;
private var oldSelectedVersuses : int = -1;
private var selectedTeam : int;

private var inputPassword : String = "";
private var inputUsername : String = "";

private var statusTextStartTime : float;
private var statusTextTimeout : float = 5f;

private var versusesListLodaded : boolean = false;
private var versusesSelected : boolean = false;
private var selectedPlayerTeam : String = "";
private var selectedEnemyTeam : String = "";

function Start()
{
	LoadVersuses();
	
	startButtonRect = Rect(Screen.width/2 - 100, Screen.height/2 - 200, 200, 200);
	scoresButtonRect = Rect(Screen.width/2 - 200, Screen.height/2 - 100, 100, 100);
	suggestButtonRect = Rect(Screen.width/2 + 100, Screen.height/2 - 100, 100, 100);
	versusesComboBoxRect = Rect(Screen.width/2 - 200, Screen.height/2, 200, 20);
	teamsComboBoxRect = Rect(Screen.width/2, Screen.height/2, 200, 20);
	loginoutButtonRect = Rect(Screen.width - 100, 0, 100, 20);
	loginPasswordRect = Rect(Screen.width - 200, 0, 100, 20);
	loginUsernameRect = Rect(Screen.width - 300, 0, 100, 20);
	createProfiletButtonRect = Rect(Screen.width - 100, 20, 100, 20);
	recoverPasswordButtonRect = Rect(Screen.width - 250, 20, 150, 20);
	selectedUsernameRect = Rect(Screen.width - 300, 0, 200, 20);
	
	// Make a GUIStyle that has a solid white hover/onHover background to indicate highlighted items
	listStyle = new GUIStyle();
	listStyle.normal.textColor = Color.white;
	var tex = new Texture2D(2, 2);
	var colors = new Color[4];
	for (color in colors) color = Color.white;
	tex.SetPixels(colors);
	tex.Apply();
	listStyle.hover.background = tex;
	listStyle.onHover.background = tex;
	listStyle.padding.left = listStyle.padding.right = listStyle.padding.top = listStyle.padding.bottom = 4;
	
	statusText.text = "";
}

function OnGUI()
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
			StartCoroutine(PerformPasswordRecovery());
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
			
			var split : String[] = versusesList[selectedVersuses].text.Split("-"[0]);
			teamsList = new GUIContent[2];
			teamsList[0] = new GUIContent(split[0].Trim());
			teamsList[1] = new GUIContent(split[1].Trim());
			
			teamsComboBox = ComboBox(teamsComboBoxRect, teamsList[selectedTeam], teamsList, listStyle);
		}
		
		selectedTeam = teamsComboBox.Show();
	}
	
	if(GUI.Button(startButtonRect, startButtonTexture, buttonStyle))
	{
		if(PlayerPrefs.GetInt ("LoggedIn", 0) == 0 && versusesListLodaded == false)
		{
			statusText.text = "Need to login first";
			statusTextStartTime = Time.time;
		}
		else
		{
			var enemyTeam : int = (selectedTeam+1)%2;
			PlayerPrefs.SetInt("SelectedVSint", selectedVersuses);
			PlayerPrefs.SetInt("SelectedTeamint", selectedTeam);
			PlayerPrefs.SetString("SelectedUsername", inputUsername);
			PlayerPrefs.SetString("SelectedTeam", teamsList[selectedTeam].text);
			PlayerPrefs.SetString("EnemyTeam", teamsList[enemyTeam].text);
			Application.LoadLevel("Default");
			//Application.LoadLevel("IconsDeathmatch");
		}
	}
	
	if(GUI.Button(scoresButtonRect, scoresButtonTexture, buttonStyle))
	{
		Application.LoadLevel("Scores");
	}
	
	if(GUI.Button(suggestButtonRect, suggestButtonTexture, buttonStyle))
	{
	}
	
	var guiTime : float = Time.time - statusTextStartTime;
	var restSeconds : int = Mathf.CeilToInt (statusTextTimeout - guiTime);
	if (restSeconds == 0) 
	{
		statusText.text = "";
	}
}

function LoadVersuses()
{
	statusText.text = "Loading versuses";
	statusTextStartTime = Time.time;
	
	var hs_get : WWW = new WWW("http://alas.matf.bg.ac.rs/~mi08204/projekti/IconsDeathmatch/iconsdeathmatchapi.php?action=GetVersuses");
	yield hs_get;
	
	if (hs_get.error != null)
	{
		statusText.guiText.text = "There was an error loading in: " + hs_get.error;
	}
	else
	{
		var split : String[] = hs_get.text.Split("|"[0]);
		versusesList = new GUIContent[split.length/2];
		for (var i = 0; i < split.length; i+=2) 
		{
			versusesList[i/2] = new GUIContent(split[i] + " - " + split[i+1]);
		}
		
		selectedVersuses = PlayerPrefs.GetInt("SelectedVSint", 0);
		selectedTeam = PlayerPrefs.GetInt("SelectedTeamint", 0);
	
		versusesComboBox = ComboBox(versusesComboBoxRect, versusesList[selectedVersuses], versusesList, listStyle);
		versusesListLodaded = true;
	}
}

function PerformLogin()
{
	statusText.text = "Loging in";
	statusTextStartTime = Time.time;
	
	var hs_get : WWW = new WWW("http://alas.matf.bg.ac.rs/~mi08204/projekti/IconsDeathmatch/iconsdeathmatchapi.php?action=ValidateProfile&Username="+inputUsername+"&Password="+inputPassword);
	yield hs_get;
	
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

function PerformLogout()
{
	PlayerPrefs.SetInt ("LoggedIn", 0);
}

function PerformPasswordRecovery()
{
	yield;
}