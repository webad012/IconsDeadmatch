using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour 
{
	private GameController gameController;

	void Start()
	{
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) 
		{
			gameController = gameControllerObject.GetComponent<GameController>();
				
		} else 
		{
			Debug.Log("Cannot find 'GameController' script.");
				
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (gameObject.tag == "Bullet") {
			if (other.gameObject.tag == "PickUp") 
			{
				gameController.PickUpHit(other.gameObject);
				//Destroy (other.gameObject);
				Destroy (gameObject);
			}
			else if (other.gameObject.tag == "Barrier") 
			{
				Destroy (gameObject);
			}
				}
	}
}
