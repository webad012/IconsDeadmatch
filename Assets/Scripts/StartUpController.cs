using UnityEngine;
using System.Collections;

public class StartUpController : MonoBehaviour 
{
	public GUIText statusText;

	private float currentVersion = 0.1f;

	// Use this for initialization
	void Start () 
	{
		PlayerPrefs.SetInt ("LoggedIn", 0);

		StartCoroutine (CheckGameVersion());
	}
	
	// Update is called once per frame
	IEnumerator CheckGameVersion () 
	{
		statusText.guiText.text = "Checking version";
		//statusTextStartTime = Time.time;
		
		WWW hs_get = new WWW("http://alas.matf.bg.ac.rs/~mi08204/projekti/IconsDeathmatch/currentversion.txt");
		yield return hs_get;
		
		if (hs_get.error != null)
		{
			statusText.guiText.text = "There was an error checking version: " + hs_get.error;
		}
		else
		{
			statusText.guiText.text = hs_get.text;

			float serverVersion;
			float.TryParse(hs_get.text, out serverVersion);
			if(currentVersion != serverVersion)
			{
				statusText.guiText.text = "Version obsolete, please download latest version";
			}
			else
			{
				Application.LoadLevel("MainMenu");
			}
			//gameObject.guiText.text = hs_get.text; // this is a GUIText that will display the scores in game.
		}
	}
}
