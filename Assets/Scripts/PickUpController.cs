using UnityEngine;
using System.Collections;

public class PickUpController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			GameObject gameController = GameObject.FindWithTag("GameController");
			gameController.GetComponent<GameController>().PickedUp();
			Destroy(gameObject);
		}
	}
}