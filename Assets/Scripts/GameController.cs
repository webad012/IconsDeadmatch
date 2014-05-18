using UnityEngine;
using System.Collections;

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
}

[System.Serializable]
public class DisplayTexts
{
	public GUIText timerTextFront;
	public GUIText timerTextBack;
	public GUIText pickupsTextFront;
	public GUIText pickupsTextBack;
}

public class GameController : MonoBehaviour 
{
	public GameObject playerObject;
	public GameObject[] enemiyBuzzers;
	public EnemySpider[] enemiySpiders;
	public GameObject[] pickUps;
	public DisplayTexts displayTexts;
	public int numberOfPickUps;

	//public Texture crvenaZvezdaTexture;
	//public Texture partizanTexture;
	public Textures playerTextures;
	public Textures pickUpsTextures;
	public Textures buzzersTextures;
	public Textures spidersTextures;

	private string selectedTeam;
	private string enemyTeam;

	private int countDownSeconds = 30;
	private float startTime;
	private int restSeconds;

	private int score = 0;

	// Use this for initialization
	void Start () 
	{
		selectedTeam = PlayerPrefs.GetString ("SelectedTeam");
		enemyTeam = PlayerPrefs.GetString ("EnemyTeam");

		if (selectedTeam == "CrvenaZvezda") 
		{
			playerObject.renderer.material.mainTexture = playerTextures.crvenaZvezdaTexture;
			foreach(GameObject p in pickUps)
			{
				p.renderer.material.mainTexture = pickUpsTextures.crvenaZvezdaTexture;
			}
		}
		else if(selectedTeam == "Partizan")
		{
			playerObject.renderer.material.mainTexture = playerTextures.partizanTexture;
			foreach(GameObject p in pickUps)
			{
				p.renderer.material.mainTexture = pickUpsTextures.partizanTexture;
			}
		}

		if (enemyTeam == "CrvenaZvezda") 
		{
			foreach(GameObject eb in enemiyBuzzers)
			{
				eb.renderer.material.mainTexture = buzzersTextures.crvenaZvezdaTexture;
			}
			foreach(EnemySpider es in enemiySpiders)
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
			foreach(GameObject eb in enemiyBuzzers)
			{
				eb.renderer.material.mainTexture = buzzersTextures.partizanTexture;
			}
			foreach(EnemySpider es in enemiySpiders)
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
		restSeconds = Mathf.CeilToInt (countDownSeconds - (Time.time - startTime));

		if (numberOfPickUps == 0) 
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
		displayTexts.pickupsTextFront.text = "Left: " + numberOfPickUps;
		displayTexts.pickupsTextBack.text = "Left: " + numberOfPickUps;

		displayTexts.timerTextFront.text = "Time: " + restSeconds.ToString();
		displayTexts.timerTextBack.text = displayTexts.timerTextFront.text;
	}

	public void PickedUp()
	{
		numberOfPickUps--;
		score += 10;
	}
}
