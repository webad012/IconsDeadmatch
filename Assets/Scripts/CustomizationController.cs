using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;

[System.Serializable]
public class Vehicles
{
	public GameObject vehicle1;
	public GameObject vehicle2;
}

[System.Serializable]
public class CustomizationDisplayTexts
{
	public GUIText vehicleName;
	public GUIText vehiclePrice;
}

class VehicleObject
{
	private GameObject _object;
	private string _name;
	private int _price;

	public GameObject GetObject()
	{
		return _object;
	}

	public string GetName()
	{
		return _name;
	}

	public int GetPrice()
	{
		return _price;
	}

	public VehicleObject(GameObject vehicleObject, string name, int price)
	{
		_object = vehicleObject;
		_name = name;
		_price = price;
	}
}

public class CustomizationController : MonoBehaviour 
{
	public Vehicles vehicles;
	public CustomizationDisplayTexts displayTexts;

	private List<VehicleObject> vehiclesList = new List<VehicleObject>();
	private Rect leftButtonRect;
	private Rect rightButtonRect;
	private int vehicleSelected = 0;
	private Object instantiatedObject;
	private int chosenTab = 0;

	void Start () 
	{
		InitializeVehicles ();

		leftButtonRect = new Rect (0, Screen.height/2, 100, 40);
		rightButtonRect = new Rect (Screen.width - 100, Screen.height/2, 100, 40);

		InstantiateVehicle ();
	}

	void OnGUI()
	{
		//header
		if (chosenTab == 0) 
		{
			GUI.color = Color.white;
		} 
		else 
		{
			GUI.color = Color.grey;
		}
		if (GUI.Button (new Rect(Screen.width/2 - 200,
		                         0,
		                         200,
		                         30), StaticTexts.Instance.Customization_HeaderVehicles())) 
		{
			chosenTab = 0;
		}
		if (chosenTab == 1) 
		{
			GUI.color = Color.white;
		} 
		else 
		{
			GUI.color = Color.grey;
		}
		if (GUI.Button (new Rect(Screen.width/2,
		                         0,
		                         200,
		                         30), StaticTexts.Instance.Customization_HeaderWeapons())) 
		{
			chosenTab = 1;
		}

		GUI.color = Color.white;
		if (GUI.Button (leftButtonRect, "<")) 
		{
			ChooseLeft();
		}
		if (GUI.Button (rightButtonRect, ">")) 
		{
			ChooseRight();
		}
	}

	void ChooseLeft()
	{
		Destroy (instantiatedObject);

		vehicleSelected--;
		if (vehicleSelected < 0) 
		{
			vehicleSelected = vehiclesList.Count-1;
		}

		InstantiateVehicle ();
	}

	void ChooseRight()
	{
		Destroy (instantiatedObject);
		
		vehicleSelected++;
		if (vehicleSelected == vehiclesList.Count) 
		{
			vehicleSelected = 0;
		}

		InstantiateVehicle ();
	}

	void InstantiateVehicle()
	{
		instantiatedObject = Instantiate (vehiclesList[vehicleSelected].GetObject());
		displayTexts.vehicleName.text = vehiclesList [vehicleSelected].GetName();
		displayTexts.vehiclePrice.text = StaticTexts.Instance.Customization_Price() 
											+ vehiclesList [vehicleSelected].GetPrice ().ToString();
	}
	
	void InitializeVehicles()
	{
		string sqlCommand;
		SQLiteDataReader sqlReader;

		sqlCommand = "SELECT name, price FROM Vehicles WHERE id=1;";
		sqlReader = DBController.Instance.ExecuteSqlForReader(sqlCommand);
		sqlReader.Read ();
		vehiclesList.Add (new VehicleObject(vehicles.vehicle1, sqlReader.GetString(0), sqlReader.GetInt32(1)));
		
		sqlCommand = "SELECT name, price FROM Vehicles WHERE id=2;";
		sqlReader = DBController.Instance.ExecuteSqlForReader (sqlCommand);
		sqlReader.Read ();
		vehiclesList.Add (new VehicleObject(vehicles.vehicle2, sqlReader.GetString(0), sqlReader.GetInt32(1)));
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
