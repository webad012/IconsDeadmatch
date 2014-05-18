using UnityEngine;
using System.Collections;

public class SuggestVSController : MonoBehaviour 
{
	public GUIText statusText;

	private Rect vsRect;
	private string inputTeam1 = "";
	private Rect team1Rect;
	private string inputTeam2 = "";
	private Rect team2Rect;
	private string inputComment = "";
	private Rect commentRect;

	private float statusTextStartTime;
	private float statusTextTimeout = 5f;

	// Use this for initialization
	void Start () 
	{
		vsRect = new Rect (Screen.width/2-25, Screen.height/3, 50, 20);
		team1Rect = new Rect (Screen.width/2-300, Screen.height/3, 200, 20);
		team2Rect = new Rect (Screen.width/2+100, Screen.height/3, 200, 20);
		commentRect = new Rect (Screen.width/2-300, Screen.height/2, 600, 20);

		statusText.text = "";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;

		GUI.Label(new Rect(vsRect.x, vsRect.y, vsRect.width, 30), "VS");

		GUI.Label(new Rect(team1Rect.x, team1Rect.y + -30, team1Rect.width, 30), "Team 1:");
		inputTeam1 = GUI.TextField (team1Rect, inputTeam1);

		GUI.Label(new Rect(team2Rect.x, team2Rect.y + -30, team2Rect.width, 30), "Team 2:");
		inputTeam2 = GUI.TextField (team2Rect, inputTeam2);

		GUI.Label(new Rect(commentRect.x, commentRect.y + -30, commentRect.width, 30), "One line comment about suggestion (not required):");
		inputComment = GUI.TextField (commentRect, inputComment);

		if(GUI.Button(new Rect(Screen.width-110, Screen.height-50, 100, 30), "Submit"))
		{
			if(inputTeam1 == "")
			{
				statusText.text = "Team 1 name required";
				statusTextStartTime = Time.time;
			}
			else if(inputTeam2 == "")
			{
				statusText.text = "Team 2 name required";
				statusTextStartTime = Time.time;
			}
			else
			{
				StartCoroutine(SubmitVsSuggestion());
			}
		}

		if(GUI.Button(new Rect(10, Screen.height-50, 50, 30), "Back"))
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

	IEnumerator SubmitVsSuggestion()
	{
		statusText.text = "Loading teams";
		WWW submitVS = new WWW(StaticTexts.Instance.web_api_location + "?action=SuggestVS&Team1="+inputTeam1+"&Team2="+inputTeam2+"&Comment="+inputComment);
		yield return submitVS;
		
		if (submitVS.error != null)
		{
			statusText.guiText.text = "There was an error sending vs suggestion: " + submitVS.error;
		}
		else
		{
			if(submitVS.text != "OK")
			{
				statusText.guiText.text = "There was an error sending vs suggestion: " + submitVS.text;
				statusTextStartTime = Time.time;
			}
			else
			{
				statusText.guiText.text = "Suggestion successfully submitted.";
				statusTextStartTime = Time.time;
			}
		}
	}
}
