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

public class GameController : MonoBehaviour 
{
	public GameObject playerObject;
	public GameObject[] enemiyBuzzers;
	public EnemySpider[] enemiySpiders;
	public GameObject[] pickUps;

	public Texture crvenaZvezdaTexture;
	public Texture partizanTexture;
	public Textures playerTextures;


	private int selectedComboBox;

	// Use this for initialization
	void Start () 
	{
		selectedComboBox = PlayerPrefs.GetInt ("SelectedOption", 0);
		if (selectedComboBox == 0) 
		{
			playerObject.renderer.material.mainTexture = playerTextures.partizanTexture;
			foreach(GameObject p in pickUps)
			{
				p.renderer.material.mainTexture = partizanTexture;
			}
			foreach(GameObject eb in enemiyBuzzers)
			{
				eb.renderer.material.mainTexture = crvenaZvezdaTexture;
			}
			foreach(EnemySpider es in enemiySpiders)
			{
				es.main.renderer.material.mainTexture = crvenaZvezdaTexture;
				es.head.renderer.material.mainTexture = crvenaZvezdaTexture;
				es.leg1.renderer.material.mainTexture = crvenaZvezdaTexture;
				es.leg2.renderer.material.mainTexture = crvenaZvezdaTexture;
				es.leg3.renderer.material.mainTexture = crvenaZvezdaTexture;
				es.leg4.renderer.material.mainTexture = crvenaZvezdaTexture;
			}
		} 
		else 
		{
			playerObject.renderer.material.mainTexture = playerTextures.crvenaZvezdaTexture;
			foreach(GameObject p in pickUps)
			{
				p.renderer.material.mainTexture = crvenaZvezdaTexture;
			}
			foreach(GameObject eb in enemiyBuzzers)
			{
				eb.renderer.material.mainTexture = partizanTexture;
			}
			foreach(EnemySpider es in enemiySpiders)
			{
				es.main.renderer.material.mainTexture = partizanTexture;
				es.head.renderer.material.mainTexture = partizanTexture;
				es.leg1.renderer.material.mainTexture = partizanTexture;
				es.leg2.renderer.material.mainTexture = partizanTexture;
				es.leg3.renderer.material.mainTexture = partizanTexture;
				es.leg4.renderer.material.mainTexture = partizanTexture;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
