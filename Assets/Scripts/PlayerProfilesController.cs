using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerProfilesController : MonoBehaviour 
{
	private Rect headerArea;
	private int headerHeight = 40;
	private Rect contentArea;
	private List<string> PlayerProfilesNames;

	// Use this for initialization
	void Start () 
	{
		headerArea = new Rect (10, 0, Screen.width-10, headerHeight);
		contentArea = new Rect (10, headerHeight, Screen.width-10, Screen.height - headerHeight);

		PlayerProfilesNames = new List<string> ();
		StartCoroutine (SetupPlayersList());
	}
	IEnumerator SetupPlayersList()
    {
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnGUI()
	{

	}
}
