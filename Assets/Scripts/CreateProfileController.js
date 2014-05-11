#pragma strict

public var statusText : GUIText;

private var inputUsername : String = "";
private var inputPassword : String = "";
private var inputRepeatPassword : String = "";
private var inputEmail : String = "";
private var statusTextStartTime : float;
private var statusTextTimeout : float = 5f;

function Start () 
{
	statusText.text = "";
}

function OnGUI()
{
	GUI.skin.label.alignment = TextAnchor.MiddleRight;
	GUI.Label(Rect(Screen.width/2 - 200, Screen.height/2 - 100, 200, 20), "Username:");
	inputUsername = GUI.TextField (Rect(Screen.width/2, Screen.height/2 - 100, 200, 20), inputUsername);
	
	GUI.Label(Rect(Screen.width/2 - 200, Screen.height/2 - 80, 200, 20), "Password:");
	inputPassword = GUI.PasswordField (Rect(Screen.width/2, Screen.height/2 - 80, 200, 20), inputPassword, "*"[0], 25);
	
	GUI.Label(Rect(Screen.width/2 - 200, Screen.height/2 - 60, 200, 20), "Repeat password:");
	inputRepeatPassword = GUI.PasswordField (Rect(Screen.width/2, Screen.height/2 - 60, 200, 20), inputRepeatPassword, "*"[0], 25);
	
	GUI.Label(Rect(Screen.width/2 - 200, Screen.height/2 - 40, 200, 20), "Email:");
	inputEmail = GUI.TextField (Rect(Screen.width/2, Screen.height/2 - 40, 200, 20), inputEmail);
	
	if(GUI.Button(Rect(Screen.width/2 - 50, Screen.height/2, 100, 20), "Create"))
	{
		if(inputUsername === ""
			|| inputPassword === ""
			|| inputRepeatPassword === ""
			|| inputEmail === "")
		{
			statusText.text = "All fields required";
			statusTextStartTime = Time.time;
		}
		else
		{
			if(inputPassword != inputRepeatPassword)
			{
				statusText.text = "Repeat password does not match";
				statusTextStartTime = Time.time;
			}
			else
			{
				if(!VerifyEmailAddress(inputEmail))
				{
					statusText.text = "Invalid email format";
					statusTextStartTime = Time.time;
				}
				else
				{
					StartCoroutine(CreateProfile());
				}
			}
		}
	}
	
	if(GUI.Button(Rect(Screen.width/2 - 50, Screen.height/2+20, 100, 20), "Back"))
	{
		Application.LoadLevel("MainMenu");
	}
	
	var guiTime : float = Time.time - statusTextStartTime;
	var restSeconds : int = Mathf.CeilToInt (statusTextTimeout - guiTime);
	if (restSeconds == 0) 
	{
		statusText.text = "";
	}
}

function Update () {

}

static function VerifyEmailAddress(address:String)
{
	var atCharacter:String[];
	var dotCharacter:String[];
	atCharacter = address.Split("@"[0]);

	if(atCharacter.Length == 2)
	{
		dotCharacter = atCharacter[1].Split("."[0]);
		
		if(dotCharacter.Length >= 2)
		{
		    if(dotCharacter[dotCharacter.Length - 1].Length == 0)
		    {
		        return false;
		    }
		    else
		    {
		        return true;
		    }
		}
		else
		{
		    return false;
		}
	}
	else
	{
	return false;
	}
}

function CreateProfile()
{
	statusText.text = "Creating profile";
	statusTextStartTime = Time.time;
	
	var hs_get : WWW = new WWW("http://alas.matf.bg.ac.rs/~mi08204/projekti/IconsDeathmatch/iconsdeathmatchapi.php?action=CreateProfile&Username="+inputUsername+"&Password="+inputPassword+"&Email="+inputEmail);
	yield hs_get;
	
	if (hs_get.error != null)
	{
		statusText.guiText.text = "There was an error loging in: " + hs_get.error;
	}
	else
	{
		statusText.text = hs_get.text;
		if(hs_get.text == "OK")
		{
			statusText.text = "Account successfully created";
		}
		else
		{
			statusText.text = "Error: " + hs_get.text;
		}
	}
}