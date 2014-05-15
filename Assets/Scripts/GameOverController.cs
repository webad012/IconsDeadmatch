using UnityEngine;
using System.Collections;

public class GameOverController : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		Screen.showCursor = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			Application.LoadLevel("MainMenu");
		}
	}

	void OnGUI()
	{
		GUI.Label(new Rect(Screen.width/2 - 100, Screen.height/2, 200, 50), "You lost, ESC to main menu");
	}
}