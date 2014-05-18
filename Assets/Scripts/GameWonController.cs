using UnityEngine;
using System.Collections;

public class GameWonController : MonoBehaviour 
{
	private string statusText;
	private bool canExit = false;

	// Use this for initialization
	void Start () 
	{
		Screen.showCursor = true;
		statusText = "";
		StartCoroutine (SendScore());
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (canExit) 
		{
			if (Input.GetKeyDown (KeyCode.Escape)) 
			{
				Application.LoadLevel("MainMenu");
			}
		}
	}

	void OnGUI()
	{
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.Label(new Rect(Screen.width/2 - 100, Screen.height/3, 200, 50), "You won! With score: " + PlayerPrefs.GetString("PlayerScore"));
		GUI.Label(new Rect(Screen.width/2 - 100, Screen.height/2, 200, 50), statusText);
	}

	IEnumerator SendScore()
	{
		statusText = "Sending score to server, please wait";
		string url = StaticTexts.Instance.web_api_location
						+ "?action=SendScore&Username=" + PlayerPrefs.GetString("SelectedProfileUsername") 
						+ "&Team=" + PlayerPrefs.GetString ("SelectedTeam") + "&Score=" + PlayerPrefs.GetString ("PlayerScore");
		//Debug.Log (url);
		WWW scoreSend = new WWW(url);
		yield return scoreSend;

		if (scoreSend.error != null) 
		{
			statusText = "There was an error sending score: " + scoreSend.error + "\nESC to main menu";
		} 
		else 
		{
			if(scoreSend.text != "OK")
			{
				statusText = "There was an error sending score: " + scoreSend.error + "\nESC to main menu";
			}
			else
			{
				statusText = "Scores sent\nESC to main menu";
			}
		}

		canExit = true;
	}
}
