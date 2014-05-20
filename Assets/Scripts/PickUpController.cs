using UnityEngine;
using System.Collections;

public class PickUpController : MonoBehaviour 
{
	// min and max rotation speeds
	public float minRotationSpeed = 0.5f;
	public float maxRotationSpeed = 2.0f;
	
	// calculated rotation speed
	private float rotationSpeed;

	// Use this for initialization
	void Start () 
	{
	
	}

	void Awake()
	{
		// calculate a random rotation speed 
		rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
	}
	
	// Update is called once per frame
	void Update () 
	{
		// rotate the gameobject every frame
		transform.Rotate(rotationSpeed, rotationSpeed, 0);
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			GameObject gameController = GameObject.FindWithTag("GameController");
			gameController.GetComponent<GameController>().PickedUp(this.gameObject);
			Destroy(gameObject);
		}
	}
}