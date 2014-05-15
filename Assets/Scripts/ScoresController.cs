using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

class PlayerScore
{
	private string _name;
	private string _score;
	
	public string GetName()
	{
		return _name;
	}
	
	public string GetScore()
	{
		return _score;
	}
	
	public PlayerScore(string name, string score)
	{
		_name = name;
		_score = score;
	}
}

public class ScoresController : MonoBehaviour 
{
	public GUIText statusText;

	private GUIContent[] teamsList;
	private ComboBox teamsComboBox;
	private bool teamsListLodaded = false;
	private bool scoresListLodaded = false;

	private int selectedTeam = 0;
	private int oldSelectedTeam = 0;
	
	private Rect teamsComboBoxRect;
	private Rect teamsScoreRect;
	private Rect top10TeamPlayersRect;
	private List<PlayerScore> playerScores;
	//private Rect top10TeamPlayersScrollView;
	//private Vector2 top10TeamPlayersScrollPosition = Vector2.zero;
	private int teamTotalScore;

	// Use this for initialization
	void Start () 
	{
		statusText.text = "";
		teamsComboBoxRect = new Rect (10, 10, 100, 20);
		top10TeamPlayersRect = new Rect (150, 10, Screen.width/3, Screen.height-20);
		playerScores = new List<PlayerScore> ();
		teamTotalScore = 0;
		StartCoroutine (LoadTeams());
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnGUI()
	{
		if(teamsListLodaded == true)
		{
			selectedTeam = teamsComboBox.Show();
			GUI.Label(new Rect(teamsComboBoxRect.x+teamsComboBoxRect.width, teamsComboBoxRect.y, teamsComboBoxRect.width, teamsComboBoxRect.height), "Score: " + teamTotalScore.ToString());

			if(oldSelectedTeam != selectedTeam)
			{
				oldSelectedTeam = selectedTeam;
				scoresListLodaded = false;
				playerScores.Clear();
				teamTotalScore = 0;
				StartCoroutine (LoadScores());
			}

			if(scoresListLodaded == true)
			{
				//top10TeamPlayersScrollView = new Rect (teamsScoreRect.x, teamsScoreRect.y, teamsScoreRect.width, playerScores.Count*40);
				//top10TeamPlayersScrollPosition = GUI.BeginScrollView (top10TeamPlayersRect, top10TeamPlayersScrollPosition, top10TeamPlayersScrollView, true, false);

				for (int i=0; i<playerScores.Count; i++) 
				{
					//GUI.Label(new Rect(top10TeamPlayersRect.x, top10TeamPlayersRect.y + (40 * i), 200, 20), "asd");
					GUI.skin.label.alignment = TextAnchor.MiddleRight;
					GUI.Label(new Rect(top10TeamPlayersRect.x, top10TeamPlayersRect.y + (40 * i), 200, 20), playerScores[i].GetName());
					GUI.skin.label.alignment = TextAnchor.MiddleCenter;
					GUI.Label(new Rect(top10TeamPlayersRect.x + 200, top10TeamPlayersRect.y + (40 * i), 20, 20), " : ");
					GUI.skin.label.alignment = TextAnchor.MiddleLeft;
					GUI.Label(new Rect(top10TeamPlayersRect.x + 220, top10TeamPlayersRect.y + (40 * i), 200, 20), playerScores[i].GetScore());
				}

				//GUI.EndScrollView();
			}
		}

		if(GUI.Button(new Rect(10, Screen.height-50, 50, 30), "Back"))
		{
			Application.LoadLevel("MainMenu");
		}
	}

	IEnumerator LoadTeams()
	{
		statusText.text = "Loading teams";
		WWW teamsLoad = new WWW("http://alas.matf.bg.ac.rs/~mi08204/projekti/IconsDeathmatch/iconsdeathmatchapi.php?action=GetVersuses");
		yield return teamsLoad;

		if (teamsLoad.error != null)
		{
			statusText.guiText.text = "There was an error loading teams: " + teamsLoad.error;
		}
		else
		{
			string[] split = teamsLoad.text.Split("|"[0]);
			split = split.Distinct().ToArray();
			teamsList = new GUIContent[split.Length];
			for (var i = 0; i < split.Length; i++) 
			{
				teamsList[i] = new GUIContent(split[i]);
			}

			GUIStyle teamsComboBoxStyle = new GUIStyle();
			teamsComboBoxStyle.normal.textColor = Color.white;
			var tex = new Texture2D(2, 2);
			var colors = new Color[4];
			colors[0] = Color.white;
			colors[1] = Color.white;
			colors[2] = Color.white;
			colors[3] = Color.white;
			tex.SetPixels(colors);
			tex.Apply();
			teamsComboBoxStyle.hover.background = tex;
			teamsComboBoxStyle.onHover.background = tex;
			teamsComboBoxStyle.padding.left = teamsComboBoxStyle.padding.right = teamsComboBoxStyle.padding.top = teamsComboBoxStyle.padding.bottom = 4;

			teamsComboBox = new ComboBox(teamsComboBoxRect, teamsList[selectedTeam], teamsList, teamsComboBoxStyle);
			teamsListLodaded = true;

			statusText.text = "Loading teams (done)";

			StartCoroutine (LoadScores());
		}
	}

	IEnumerator LoadScores()
	{
		statusText.text = "Loading scores";
		WWW scoresLoad = new WWW("http://alas.matf.bg.ac.rs/~mi08204/projekti/IconsDeathmatch/iconsdeathmatchapi.php?action=GetTeamScores&Team="+teamsList[selectedTeam].text);
		yield return scoresLoad;

		if (scoresLoad.error != null) 
		{
			statusText.guiText.text = "There was an error loading scores: " + scoresLoad.error;
		} 
		else 
		{
			if(scoresLoad.text != "")
			{
				string[] split = scoresLoad.text.Split("|"[0]);
				for (var i = 0; i < split.Length; i+=2) 
				{
					teamTotalScore += System.Int32.Parse(split[i+1]);
					playerScores.Add(new PlayerScore(split[i], split[i+1]));
				}
				scoresListLodaded = true;
			}

			statusText.text = "Loading scores (done)";
		}
	}
}
