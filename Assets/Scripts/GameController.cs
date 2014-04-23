using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PickUps
{
	public GameObject partizanPickUp;
	public GameObject crvenazvezdaPickUp;
}

[System.Serializable]
public class DisplayTexts
{
	public GUIText countTextFront;
	public GUIText countTextBack;
	public GUIText timerTextFront;
	public GUIText timerTextBack;
	public GUIText statusTextFront;
	public GUIText statusTextBack;
}

public class GameController : MonoBehaviour 
{
	public DisplayTexts displayTexts;
	public PickUps pickUps;
	public GameObject barrier;
	public Camera camera1;
	public Camera camera2;
	public int barrierCount;
	public int pickUpsCount;
	public int minDistance;
	public int maxDistance;

	private List<GameObject> barrierList = new List<GameObject>();
	private List<GameObject> pickUpList = new List<GameObject>();

	public int countDownSeconds = 30;
	private float startTime;
	private int restSeconds;

	private bool gameOver = false;
	private bool gameWon = false;

	// Use this for initialization
	void Start () 
	{
		camera1.enabled = true;
		camera2.enabled = false;
		startTime = Time.time;
		displayTexts.statusTextFront.text = "";
		displayTexts.statusTextBack.text = "";
		InitializeGameField ();
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.C)) 
		{
			camera1.enabled = !camera1.enabled;
			camera2.enabled = !camera2.enabled;
		}
	}

	void OnGUI()
	{
		if (!gameOver && !gameWon) 
		{
			float guiTime = Time.time - startTime;
			restSeconds = Mathf.CeilToInt (countDownSeconds - guiTime);
			if (restSeconds == 0) 
			{
				gameOver = true;
				displayTexts.statusTextFront.text = "Kraj igre!";
				displayTexts.statusTextBack.text = displayTexts.statusTextFront.text;
			}

			if (pickUpList.Count == 0) 
			{
				gameWon = true;
				displayTexts.statusTextFront.text = "Pobeda!";
				displayTexts.statusTextBack.text = displayTexts.statusTextFront.text;
			}
		}

		displayTexts.countTextFront.text = "Preostalo: " + pickUpList.Count.ToString ();
		displayTexts.countTextBack.text = displayTexts.countTextFront.text;
		
		displayTexts.timerTextFront.text = "Vreme: " + restSeconds.ToString ();
		displayTexts.timerTextBack.text = displayTexts.timerTextFront.text;
	}
	
	void InitializeGameField()
	{
		for(int i=0; i<pickUpsCount; i++)
		{
			pickUpList.Add((GameObject)Instantiate(pickUps.crvenazvezdaPickUp));
        }

		for(int i=0; i<barrierCount; i++)
		{
			barrierList.Add((GameObject)Instantiate(barrier));
		}

		PlaceBarriers ();
	}

	void PlaceBarriers()
	{
		foreach(GameObject ppu in pickUpList)
		{
			ppu.transform.position = GenerateRandomPosition(0.59f);
        }

		foreach(GameObject b in barrierList)
		{
			b.transform.position = GenerateRandomPosition(0);
		}
	}

	Vector3 GenerateRandomPosition(float yValue)
	{
		Vector3 newPosition;
		do 
		{
			float xValue, zValue;
			xValue = Random.Range(minDistance, maxDistance);
			zValue = Random.Range(minDistance, maxDistance);

			if(Random.value < 0.5f)
			{
				xValue *= -1;
			}
			if(Random.value < 0.5f)
			{
				zValue *= -1;
			}

			newPosition = new Vector3(xValue, yValue, zValue);
		//} while(IsOccupied(xValue, zValue));
		} while(IsOccupied(newPosition));
    
		return newPosition;
		//return new Vector3(xValue, yValue, zValue);
	}

	//bool IsOccupied(float xValue, float zValue)
	bool IsOccupied(Vector3 newPosition)
	{
		foreach(GameObject ppu in pickUpList)
		{
			if(Vector3.Distance(ppu.transform.position, newPosition) < 2f)
			{
				return true;
			}
		}
		
		foreach(GameObject b in barrierList)
		{
			if(Vector3.Distance(b.transform.position, newPosition) < 2f)
			{
                return true;
            }
        }
        
        return false;
	}

	public void PickUpHit(GameObject pu)
	{
		pickUpList.Remove (pu);
		Destroy (pu);
	}
}
