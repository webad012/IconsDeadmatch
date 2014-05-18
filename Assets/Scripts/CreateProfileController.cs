using UnityEngine;
using System.Collections;

public class CreateProfileController : MonoBehaviour 
{
	public GUIText statusText;
	
	private string inputUsername = "";
	private string inputPassword = "";
	private string inputRepeatPassword = "";
	private string inputEmail = "";
	private float statusTextStartTime ;
	private float statusTextTimeout = 5f;

	// Use this for initialization
	void Start () 
	{
		statusText.text = "";
	}

	void OnGUI()
	{
		GUI.skin.label.alignment = TextAnchor.MiddleRight;
		GUI.Label(new Rect(Screen.width/2 - 200, Screen.height/2 - 100, 200, 20), "Username:");
		inputUsername = GUI.TextField (new Rect(Screen.width/2, Screen.height/2 - 100, 200, 20), inputUsername);
		
		GUI.Label(new Rect(Screen.width/2 - 200, Screen.height/2 - 80, 200, 20), "Password:");
		inputPassword = GUI.PasswordField (new Rect(Screen.width/2, Screen.height/2 - 80, 200, 20), inputPassword, "*"[0], 25);
		
		GUI.Label(new Rect(Screen.width/2 - 200, Screen.height/2 - 60, 200, 20), "Repeat password:");
		inputRepeatPassword = GUI.PasswordField (new Rect(Screen.width/2, Screen.height/2 - 60, 200, 20), inputRepeatPassword, "*"[0], 25);
		
		GUI.Label(new Rect(Screen.width/2 - 200, Screen.height/2 - 40, 200, 20), "Email:");
		inputEmail = GUI.TextField (new Rect(Screen.width/2, Screen.height/2 - 40, 200, 20), inputEmail);
		
		if(GUI.Button(new Rect(Screen.width/2 - 50, Screen.height/2, 100, 20), "Create"))
		{
			if(inputUsername == ""
			   || inputPassword == ""
			   || inputRepeatPassword == ""
			   || inputEmail == "")
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
		
		if(GUI.Button(new Rect(Screen.width/2 - 50, Screen.height/2+20, 100, 20), "Back"))
		{
			Application.LoadLevel("MainMenu");
		}
		
		float guiTime = Time.time - statusTextStartTime;
		int restSeconds = Mathf.CeilToInt (statusTextTimeout - guiTime);
		if (restSeconds == 0) 
		{
			statusText.text = "";
		}
	}

	static bool VerifyEmailAddress(string address)
	{
		string[] atCharacter;
		string[] dotCharacter;
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

	IEnumerator CreateProfile()
	{
		statusText.text = "Creating profile";
		statusTextStartTime = Time.time;
		
		WWW hs_get = new WWW(StaticTexts.Instance.web_api_location + "?action=CreateProfile&Username="+inputUsername+"&Password="+inputPassword+"&Email="+inputEmail);
		yield return hs_get;
		
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
}
