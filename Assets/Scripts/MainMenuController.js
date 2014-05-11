#pragma strict

public var statusText : GUIText;

private var comboBox : ComboBox;
private var list : GUIContent[];
private var listStyle : GUIStyle;
private var comboBoxRect : Rect;
private var startButtonRect : Rect;
private var loginUsernameRect : Rect;
private var loginPasswordRect : Rect;
private var loginoutButtonRect : Rect;
private var createProfiletButtonRect : Rect;
private var recoverPasswordButtonRect : Rect;
private var selectedUsernameRect : Rect;
private var selectedComboBox : int;
private var inputPassword : String = "";
private var inputUsername : String = "";
private var statusTextStartTime : float;
private var statusTextTimeout : float = 5f;
private var versusesListLodaded : boolean = false;

function Start()
{
	//list = new GUIContent[2];
	//list[0] = new GUIContent("Zvezda vs Partizan");
	//list[1] = new GUIContent("Partizan vs Zvezda");
	LoadVersuses();
	
	startButtonRect = Rect(Screen.width/2 - 100, Screen.height/2 - 100, 200, 20);
	comboBoxRect = Rect(Screen.width/2 - 100, Screen.height/2, 200, 20);
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
		selectedComboBox = comboBox.Show();
	}
	
	if(GUI.Button(startButtonRect, "Start"))
	{
		if(PlayerPrefs.GetInt ("LoggedIn", 0) == 0)
		{
			statusText.text = "Need to login first";
			statusTextStartTime = Time.time;
		}
		else
		{
			PlayerPrefs.SetInt("SelectedOption", selectedComboBox);
			Application.LoadLevel("Default");
		}
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
		list = new GUIContent[split.length/2];
		for (var i = 0; i < split.length; i+=2) 
		{
			list[i/2] = new GUIContent(split[i] + " - " + split[i+1]);
		}
	}
	
	comboBox = ComboBox(comboBoxRect, list[PlayerPrefs.GetInt ("SelectedOption", 0)], list, listStyle);
	versusesListLodaded = true;
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