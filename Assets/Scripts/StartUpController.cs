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

		WWW hs_get = new WWW(StaticTexts.Instance.web_api_location + "?action=GetCurrentVersion");
		yield return hs_get;
		
		if (hs_get.error != null)
		{
			statusText.guiText.text = "There was an error checking version: " + hs_get.error;
		}
		else
		{
			statusText.guiText.text = hs_get.text;
			if(!hs_get.text.Contains("bad action"))
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
			}
		}
	}
}
