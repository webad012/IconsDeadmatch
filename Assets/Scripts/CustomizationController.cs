using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System;

[System.Serializable]
public class Vehicles
{
	public GameObject vehicle1;
	public GameObject vehicle2;
}

[System.Serializable]
public class Weapons
{
	public GameObject weapon1;
}

[System.Serializable]
public class CustomizationDisplayTexts
{
	public GUIText name;
	public GUIText price;
	public GUIText money;
	public GUIText status;
	public GUIText additionaInfo;
}

class ChoiceObject
{
	public GameObject GObject { get; set; }
	public int Id { get; set; }
	public string Name { get; set; }
	public int Price { get; set; }
	public bool Unlocked { get; set; }
}

class VehicleObject : ChoiceObject
{
	public int MoveSpeed { get; set; }
	public int TurnSpeed { get; set; }

	public string GetInfo()
	{
		return StaticTexts.Instance.Customization_AdditionalInfoMoveSpeed () + MoveSpeed
						+ "\n" + StaticTexts.Instance.Customization_AdditionalInfoTurnSpeed () + TurnSpeed;
	}

	public VehicleObject(GameObject vehicleObject, int id, string name, int price, bool unlocked, int moveSpeed, int turnSpeed)
	{
		GObject = vehicleObject;
		Id = id;
		Name = name;
		Price = price;
		Unlocked = unlocked;
		MoveSpeed = moveSpeed;
		TurnSpeed = turnSpeed;
	}
}

class WeaponObject : ChoiceObject
{
	public int Damage { get; set; }

	public string GetInfo()
	{
		return StaticTexts.Instance.Customization_AdditionalInfoDamage () + Damage;
	}

	public WeaponObject(GameObject vehicleObject, int id, string name, int price, bool unlocked, int damage)
	{
		GObject = vehicleObject;
		Id = id;
		Name = name;
		Price = price;
		Unlocked = unlocked;
		Damage = damage;
	}
}

public class CustomizationController : MonoBehaviour 
{
	public Vehicles vehicles;
	private int numberOfVehicles = 2;
	public Weapons weapons;
	private int numberOfWeapons = 1;
	public CustomizationDisplayTexts displayTexts;
	public Light light1;
	public Light light2;
	public GUITexture lockTexture;
	public GUITexture selectedTexture;

	private float statusTextStartTime;
	private float statusTextTimeout = 5;

	private List<VehicleObject> vehiclesList = new List<VehicleObject>();
	private List<WeaponObject> weaponsList = new List<WeaponObject>();
	private Rect leftButtonRect;
	private Rect rightButtonRect;
	private Rect select_unlockButtonRect;
	private Rect backButtonRect;
	private string select_unlockButtonText;
	private int vehicleShown = 0;
	private int vehicleSelected = 0;
	private int weaponShown = 0;
	private int weaponSelected = 0;
	private GameObject instantiatedVehicleObject;
	private GameObject instantiatedWeaponObject;
	private GameObject weaponPositionObject;
	private int chosenTab = 0;
	private int selectedProfileId = -1;
	private int playerMoney;

	void Start () 
	{
		displayTexts.status.text = "";
		light1.enabled = false;
		light2.enabled = false;
		lockTexture.enabled = false;

		leftButtonRect = new Rect (0, Screen.height/2, 100, 40);
		rightButtonRect = new Rect (Screen.width - 100, Screen.height/2, 100, 40);
		select_unlockButtonRect = new Rect (Screen.width/2 - 50, Screen.height-70, 100, 40);
		backButtonRect = new Rect (Screen.width - 100, Screen.height-70, 100, 40);

		selectedProfileId = PlayerPrefs.GetInt ("ChoosenProfileId", -1);
		if (selectedProfileId == -1) 
		{
			displayTexts.status.text = "Error getting player pref: ChoosenProfileId";
			statusTextStartTime = Time.time;
		} 
		else 
		{
			InitializeVehiclesAndWeapons ();

			string sqlCommand = "SELECT money, selected_vehicle_id, selected_weapon_id "
								+"FROM PlayerProfiles WHERE id=" + selectedProfileId;
			object[] sqlResult = new object[3];
			DBController.Instance.ExecuteSqlForReader(sqlCommand, sqlResult);
			playerMoney = Convert.ToInt32(sqlResult[0].ToString());
			vehicleSelected = Convert.ToInt32(sqlResult[1].ToString());
			vehicleShown = vehicleSelected;
			weaponSelected = Convert.ToInt32(sqlResult[2].ToString());
			weaponShown = weaponSelected;
		}

		if(chosenTab == 0)
		{
			InstantiateVehicle ();
		} 
		else if (chosenTab == 1)  
		{
			InstantiateWeapon ();
		}
	}

	void OnGUI()
	{
		//header
		displayTexts.money.text = StaticTexts.Instance.Money () + playerMoney.ToString ();

		if(chosenTab == 0)
		{
			displayTexts.name.text = vehiclesList [vehicleShown-1].Name;
			displayTexts.price.text = StaticTexts.Instance.Customization_Price() 
				+ vehiclesList [vehicleShown-1].Price.ToString();
			displayTexts.additionaInfo.text = vehiclesList [vehicleShown-1].GetInfo();
		} 
		else if (chosenTab == 1)  
		{
			displayTexts.name.text = weaponsList [weaponShown-1].Name;
			displayTexts.price.text = StaticTexts.Instance.Customization_Price() 
				+ weaponsList [weaponShown-1].Price.ToString();
			displayTexts.additionaInfo.text = weaponsList [weaponShown-1].GetInfo();
		}

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
			if(chosenTab != 0)
			{
				chosenTab = 0;
				Destroy(instantiatedWeaponObject);
				InstantiateVehicle();
			}

			//displayTexts.name.text = vehiclesList [vehicleShown-1].Name;
			//displayTexts.price.text = StaticTexts.Instance.Customization_Price() 
			//	+ vehiclesList [vehicleShown-1].Price.ToString();
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
			if(chosenTab != 1)
			{
				chosenTab = 1;
				Destroy(instantiatedVehicleObject);
				InstantiateWeapon();
			}

			//displayTexts.name.text = weaponsList [weaponShown-1].Name;
			//displayTexts.price.text = StaticTexts.Instance.Customization_Price() 
			//	+ weaponsList [weaponShown-1].Price.ToString();
		}

		// content
		GUI.color = Color.white;
		if (GUI.Button (leftButtonRect, "<")) 
		{
			ChooseLeft();
		}
		if (GUI.Button (rightButtonRect, ">")) 
		{
			ChooseRight();
		}

		if (GUI.Button (select_unlockButtonRect, select_unlockButtonText)) 
		{
			PerformSelectUnlock();
		}

		if (GUI.Button (backButtonRect, StaticTexts.Instance.Customization_Back())) 
		{
			PerformBack();
		}

		float guiTime = Time.time - statusTextStartTime;
		int restSeconds = Mathf.CeilToInt (statusTextTimeout - guiTime);
		if (restSeconds == 0) 
		{
			displayTexts.status.text = "";
		}

		if ((chosenTab == 0 && vehicleSelected == vehicleShown && vehiclesList[vehicleShown-1].Unlocked)
		    || (chosenTab == 1 && weaponSelected == weaponShown && weaponsList[weaponShown-1].Unlocked)) 
		{
			selectedTexture.enabled = true;
		} 
		else 
		{
			selectedTexture.enabled = false;
		}
	}

	void ChooseLeft()
	{
		if (chosenTab == 0) 
		{
			Destroy (instantiatedVehicleObject);

			vehicleShown--;
			if (vehicleShown <= 0) 
			{
				vehicleShown = vehiclesList.Count;
			}

			InstantiateVehicle ();
			
			//displayTexts.name.text = vehiclesList [vehicleShown-1].Name;
			//displayTexts.price.text = StaticTexts.Instance.Customization_Price() 
			//	+ vehiclesList [vehicleShown-1].Price.ToString();
		} 
		else if 
		(chosenTab == 1)  
		{
			Destroy (instantiatedWeaponObject);

			weaponShown--;
			if (weaponShown <= 0) 
			{
				weaponShown = weaponsList.Count;
			}

			InstantiateWeapon ();
			
			//displayTexts.name.text = weaponsList [weaponShown-1].Name;
			//displayTexts.price.text = StaticTexts.Instance.Customization_Price() 
			//	+ weaponsList [weaponShown-1].Price.ToString();
		}
	}

	void ChooseRight()
	{
		if(chosenTab == 0)
		{
			Destroy (instantiatedVehicleObject);

			vehicleShown++;
			if (vehicleShown > vehiclesList.Count) 
			{
				vehicleShown = 1;
			}

			InstantiateVehicle ();

			//displayTexts.name.text = vehiclesList [vehicleShown-1].Name;
			//displayTexts.price.text = StaticTexts.Instance.Customization_Price() 
			//	+ vehiclesList [vehicleShown-1].Price.ToString();
		} 
		else if (chosenTab == 1)  
		{
			Destroy (instantiatedWeaponObject);

			weaponShown++;
			if (weaponShown >= weaponsList.Count) 
			{
				weaponShown = 1;
			}

			InstantiateWeapon ();

			//displayTexts.name.text = weaponsList [weaponShown-1].Name;
			//displayTexts.price.text = StaticTexts.Instance.Customization_Price() 
			//	+ weaponsList [weaponShown-1].Price.ToString();
		}
	}

	void InstantiateVehicle()
	{
		instantiatedVehicleObject = (GameObject)Instantiate (vehiclesList[vehicleShown-1].GObject);

		PerformLockedUnlockedChekAndDisplay ();
	}

	void InstantiateWeapon()
	{
		instantiatedWeaponObject = (GameObject)Instantiate (weaponsList[weaponShown-1].GObject);

		PerformLockedUnlockedChekAndDisplay ();
	}
	
	void InitializeVehiclesAndWeapons()
	{
		// vehicles
		for (int i=0; i<numberOfVehicles; i++) 
		{
			string sqlCommand = "SELECT id, name, price, move_speed, turn_speed FROM Vehicles LIMIT 1 OFFSET " + i + ";";
			object[] sqlResult = new object[5];
			DBController.Instance.ExecuteSqlForReader(sqlCommand, sqlResult);
			int id = Convert.ToInt32(sqlResult[0].ToString());
			string name = sqlResult[1].ToString();
			int price = Convert.ToInt32(sqlResult[2].ToString());
			int move_speed = Convert.ToInt32(sqlResult[3].ToString());
			int turn_speed = Convert.ToInt32(sqlResult[4].ToString());

			sqlCommand = "SELECT COUNT(*) "
				+"FROM PlayerVehiclesUnlocked "
					+"WHERE player_id=" + selectedProfileId.ToString () + " AND vehicle_id=" + id + ";";
			int temp_unlocked = DBController.Instance.ExecuteSqlForIntScalar (sqlCommand);
			bool unlocked = false;
			if (temp_unlocked > 0)
			{
				unlocked = true;
			}

			if(i == 0)
			{
				vehiclesList.Add (new VehicleObject(vehicles.vehicle1, id, name, price, unlocked, move_speed, turn_speed));
			}
			else if(i == 1)
			{
				vehiclesList.Add (new VehicleObject(vehicles.vehicle2, id, name, price, unlocked, move_speed, turn_speed));
			}
		}

		// weapons
		for (int i=0; i<numberOfWeapons; i++) 
		{
			string sqlCommand = "SELECT id, name, price, damage FROM Weapons LIMIT 1 OFFSET " + i + ";";
			object[] sqlResult = new object[4];
			DBController.Instance.ExecuteSqlForReader(sqlCommand, sqlResult);
			int id = Convert.ToInt32(sqlResult[0].ToString());
			string name = sqlResult[1].ToString();
			int price = Convert.ToInt32(sqlResult[2].ToString());
			int damage = Convert.ToInt32(sqlResult[3].ToString());
			
			sqlCommand = "SELECT COUNT(*) "
				+"FROM PlayerWeaponsUnlocked "
					+"WHERE player_id=" + selectedProfileId.ToString () + " AND weapon_id=" + id + ";";
			int temp_unlocked = DBController.Instance.ExecuteSqlForIntScalar (sqlCommand);
			bool unlocked = false;
			if (temp_unlocked > 0)
			{
				unlocked = true;
			}

			if(weaponsList.Count == 0)
			{
				weaponsList.Add (new WeaponObject (weapons.weapon1, id, name, price, unlocked, damage));
			}
		}
	}

	void PerformLockedUnlockedChekAndDisplay()
	{
		if ((chosenTab == 0 && vehiclesList[vehicleShown-1].Unlocked)
		    || (chosenTab == 1 && weaponsList[weaponShown-1].Unlocked)) 
		{
			light1.enabled = true;
			light2.enabled = true;
			lockTexture.enabled = false;
			//selectedTexture.enabled = false;
			
			select_unlockButtonText = StaticTexts.Instance.Customization_Select();
		}
		else
		{
			light1.enabled = false;
			light2.enabled = false;
			lockTexture.enabled = true;
			//selectedTexture.enabled = false;
			
			select_unlockButtonText = StaticTexts.Instance.Customization_Unlock();
		}
	}

	void PerformSelectUnlock()
	{
		// select
		if ((chosenTab == 0 && vehiclesList[vehicleShown-1].Unlocked)
		    || (chosenTab == 1 && weaponsList[weaponShown-1].Unlocked)) 
		{

		}
		else // unlock
		{
			if(playerMoney < vehiclesList[vehicleShown-1].Price)
			{
				displayTexts.status.text = StaticTexts.Instance.Customization_NotEnoughMoney();
				statusTextStartTime = Time.time;
			}
			else
			{
				string sqlCommand;
				int price;

				if(chosenTab == 0)
				{
					sqlCommand = "INSERT INTO PlayerVehiclesUnlocked (player_id, vehicle_id) "
						+ "VALUES (" + selectedProfileId + ", " + vehiclesList[vehicleShown-1].Id + ");";
					price = vehiclesList[vehicleShown-1].Price;
				}
				else
				{
					sqlCommand = "INSERT INTO PlayerWeaponsUnlocked (player_id, weapon_id) "
						+ "VALUES (" + selectedProfileId + ", " + weaponsList[weaponShown-1].Id + ");";
					price = weaponsList[weaponShown-1].Price;
				}
				DBController.Instance.ExecuteSqlForNonQuery(sqlCommand);

				playerMoney -= price;
				sqlCommand = "UPDATE PlayerProfiles"
								+" SET money=" + playerMoney 
								+ " WHERE id=" + selectedProfileId + ";";
				DBController.Instance.ExecuteSqlForNonQuery(sqlCommand);

				if(chosenTab == 0)
				{
					vehiclesList[vehicleShown-1].Unlocked = true;
					vehicleSelected = vehicleShown;
					Destroy(instantiatedVehicleObject);
					InstantiateVehicle();
				}
				else
				{
					weaponsList[weaponShown-1].Unlocked = true;
					weaponSelected = weaponShown;
					Destroy(instantiatedWeaponObject);
					InstantiateWeapon();
				}
			}
		}
	}

	void PerformBack()
	{
		Application.LoadLevel ("PlayMenu");
	}
}
