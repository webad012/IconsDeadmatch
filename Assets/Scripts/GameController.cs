using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EnemySpider
{
	public GameObject main;
	public GameObject head;
	public GameObject leg1;
	public GameObject leg2;
	public GameObject leg3;
	public GameObject leg4;
}

[System.Serializable]
public class Textures
{
	public Texture crvenaZvezdaTexture;
	public Texture partizanTexture;
	public Texture androidTexture;
	public Texture appleTexture;
}

[System.Serializable]
public class DisplayTexts
{
	public GUIText timerTextFront;
	public GUIText timerTextBack;
	public GUIText pickupsTextFront;
	public GUIText pickupsTextBack;
}

[System.Serializable]
public class Boundary
{
	public float xMin;
	public float xMax;
	public float yPos;
	public float zMin;
	public float zMax;
}

[System.Serializable]
public class Level
{
	public Vector3 playerStartPosition;
	public GameObject[] enemiyBuzzers;
	public EnemySpider[] enemiySpiders;
	public List<Boundary> pickUpsSpawnPints;
	public int countDownSeconds;
	public int numberOfPickUps;
}

public class GameController : MonoBehaviour 
{
	public GameObject playerObjectForPositioning;
	public GameObject playerObjectForTexture;
	public GameObject pickUpObject;
	public DisplayTexts displayTexts;

	public List<Level> levels;

	public Textures playerTextures;
	public Textures pickUpsTextures;
	public Textures buzzersTextures;
	public Textures spidersTextures;

	private List<GameObject> pickUps = new List<GameObject>();

	private string selectedTeam;
	private string enemyTeam;

	private float startTime;
	private int restSeconds;

	private int score = 0;

	// Use this for initialization
	void Start () 
	{
		playerObjectForPositioning.transform.position = levels [PlayerPrefs.GetInt ("SelectedLevel", 0)].playerStartPosition;

		InitializePickUps ();

		selectedTeam = PlayerPrefs.GetString ("SelectedTeam");
		enemyTeam = PlayerPrefs.GetString ("EnemyTeam");

		if (selectedTeam == "CrvenaZvezda") 
		{
			playerObjectForTexture.renderer.material.mainTexture = playerTextures.crvenaZvezdaTexture;
			foreach(GameObject p in pickUps)
			{
				p.renderer.material.mainTexture = pickUpsTextures.crvenaZvezdaTexture;
			}
		}
		else if(selectedTeam == "Partizan")
		{
			playerObjectForTexture.renderer.material.mainTexture = playerTextures.partizanTexture;
			foreach(GameObject p in pickUps)
			{
				p.renderer.material.mainTexture = pickUpsTextures.partizanTexture;
			}
		}
		else if(selectedTeam == "Android")
		{
			playerObjectForTexture.renderer.material.mainTexture = playerTextures.androidTexture;
			foreach(GameObject p in pickUps)
			{
				p.renderer.material.mainTexture = pickUpsTextures.androidTexture;
			}
		}

		if (enemyTeam == "CrvenaZvezda") 
		{
			foreach(GameObject eb in levels[PlayerPrefs.GetInt("SelectedLevel", 0)].enemiyBuzzers)
			{
				eb.renderer.material.mainTexture = buzzersTextures.crvenaZvezdaTexture;
			}
			foreach(EnemySpider es in levels[PlayerPrefs.GetInt("SelectedLevel", 0)].enemiySpiders)
			{
				es.main.renderer.material.mainTexture = spidersTextures.crvenaZvezdaTexture;
				es.head.renderer.material.mainTexture = spidersTextures.crvenaZvezdaTexture;
				es.leg1.renderer.material.mainTexture = spidersTextures.crvenaZvezdaTexture;
				es.leg2.renderer.material.mainTexture = spidersTextures.crvenaZvezdaTexture;
				es.leg3.renderer.material.mainTexture = spidersTextures.crvenaZvezdaTexture;
				es.leg4.renderer.material.mainTexture = spidersTextures.crvenaZvezdaTexture;
			}
		}
		else if(enemyTeam == "Partizan")
		{
			foreach(GameObject eb in levels[PlayerPrefs.GetInt("SelectedLevel", 0)].enemiyBuzzers)
			{
				eb.renderer.material.mainTexture = buzzersTextures.partizanTexture;
			}
			foreach(EnemySpider es in levels[PlayerPrefs.GetInt("SelectedLevel", 0)].enemiySpiders)
			{
				es.main.renderer.material.mainTexture = spidersTextures.partizanTexture;
				es.head.renderer.material.mainTexture = spidersTextures.partizanTexture;
				es.leg1.renderer.material.mainTexture = spidersTextures.partizanTexture;
				es.leg2.renderer.material.mainTexture = spidersTextures.partizanTexture;
				es.leg3.renderer.material.mainTexture = spidersTextures.partizanTexture;
				es.leg4.renderer.material.mainTexture = spidersTextures.partizanTexture;
			}
		}

		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () 
	{
		restSeconds = Mathf.CeilToInt (levels[PlayerPrefs.GetInt("SelectedLevel", 0)].countDownSeconds - (Time.time - startTime));

		if (pickUps.Count == 0) 
		{
			score += restSeconds;
			PlayerPrefs.SetString("PlayerScore", score.ToString());
			Application.LoadLevel("GameWon");
		}

		if (restSeconds == 0) 
		{
			Application.LoadLevel("GameLost");
		}
	}

	void OnGUI()
	{
		displayTexts.pickupsTextFront.text = "Left: " + pickUps.Count;
		displayTexts.pickupsTextBack.text = "Left: " + pickUps.Count;

		displayTexts.timerTextFront.text = "Time: " + restSeconds.ToString();
		displayTexts.timerTextBack.text = displayTexts.timerTextFront.text;
	}

	void InitializePickUps()
	{
		List<int> taken_spawn_points = new List<int> ();

		for (int i=0; i<levels[PlayerPrefs.GetInt("SelectedLevel", 0)].numberOfPickUps; i++) 
		{
			bool spawned = false;
			do
			{
				int rnd = Random.Range(0, levels[PlayerPrefs.GetInt("SelectedLevel", 0)].pickUpsSpawnPints.Count);
				if(!taken_spawn_points.Contains(rnd))
				{
					spawned = true;
					taken_spawn_points.Add(rnd);
					Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(levels[PlayerPrefs.GetInt("SelectedLevel", 0)].pickUpsSpawnPints[rnd].xMin, levels[PlayerPrefs.GetInt("SelectedLevel", 0)].pickUpsSpawnPints[rnd].xMax),
					                                    levels[PlayerPrefs.GetInt("SelectedLevel", 0)].pickUpsSpawnPints[rnd].yPos,
					                                    UnityEngine.Random.Range(levels[PlayerPrefs.GetInt("SelectedLevel", 0)].pickUpsSpawnPints[rnd].zMin, levels[PlayerPrefs.GetInt("SelectedLevel", 0)].pickUpsSpawnPints[rnd].zMax));

					pickUps.Add( (GameObject)Instantiate(pickUpObject, spawnPosition, Quaternion.identity) );
				}
			}
			while(!spawned);
		}
	}

	public void PickedUp(GameObject pickUp)
	{
		//numberOfPickUps--;
		pickUps.Remove (pickUp);
		score += 10;
	}
}
