using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System;

public class PlayMenuController : MonoBehaviour 
{
	public GUIText nameText;
	public GUIText moneyText;
	public GUIText statusText;
	public TextMesh UnknownText;
	public Vehicles vehicles;
	public Weapons weapons;

	private Rect headerArea;
	private int headerHeight = 40;
	
	private Rect contentArea;
	private float menuOptionAreaHeight;
	private float menuOptionWidth = 100;
	private float menuOptionHeight = 30;

	private int selectedMenuOption = 0;

	private int selectedProfileId = -1;
	private int profileVehicle = -1;
	private int profileWeapon = -1;
	private List<GameObject> vehiclesList = new List<GameObject>();
	private List<GameObject> weaponsList = new List<GameObject>();
	bool weapon_unlocked = false;
	bool vehicle_unlocked = false;
	private float statusTextStartTime;
	private float statusTextTimeout = 5;

	// Use this for initialization
	void Start () 
	{
		statusText.text = "";

		headerArea = new Rect (10, 0, Screen.width-10, headerHeight);
		contentArea = new Rect (10, headerHeight, Screen.width-10, Screen.height - headerHeight);

		menuOptionAreaHeight = StaticTexts.Instance.MainMenu_OptionLength () * menuOptionHeight;
		if (menuOptionAreaHeight > contentArea.height) 
		{
			menuOptionAreaHeight = contentArea.height;
		}

		nameText.text = StaticTexts.Instance.PlayMenu_Name ();
		moneyText.text = StaticTexts.Instance.Money ();
		selectedProfileId = PlayerPrefs.GetInt ("ChoosenProfileId", -1);
		if (selectedProfileId == -1) 
		{
			Debug.Log("Error getting player pref: ChoosenProfileId");
			nameText.text += "?";
			moneyText.text += "?";
		} 
		else 
		{
			string sqlCommand = "SELECT name, money, selected_vehicle_id, selected_weapon_id "
									+"FROM PlayerProfiles WHERE id=" + selectedProfileId.ToString();
			object[] sqlResult = new object[4];
			DBController.Instance.ExecuteSqlForReader(sqlCommand, sqlResult);
			nameText.text += sqlResult[0].ToString();
			moneyText.text += sqlResult[1].ToString();
			profileVehicle = Convert.ToInt32(sqlResult[2].ToString());
			profileWeapon = Convert.ToInt32(sqlResult[3].ToString());

			sqlCommand = "SELECT COUNT(*) "
							+"FROM PlayerVehiclesUnlocked "
							+"WHERE player_id=" + selectedProfileId.ToString() 
								+ " AND vehicle_id=" + profileVehicle.ToString() + ";";
			int result1 = DBController.Instance.ExecuteSqlForIntScalar(sqlCommand);
			if(result1 != 0)
			{
				vehicle_unlocked = true;
			}

			sqlCommand = "SELECT COUNT(*) "
				+"FROM PlayerWeaponsUnlocked "
					+"WHERE player_id=" + selectedProfileId.ToString() 
					+ " AND weapon_id=" + profileWeapon.ToString() + ";";
			int result2 = DBController.Instance.ExecuteSqlForIntScalar(sqlCommand);
			if(result2 != 0)
			{
				weapon_unlocked = true;
			}

			if(weapon_unlocked && vehicle_unlocked)
			{
				UnknownText.text = "";
				InitializeVehiclesAndWeapons ();
				Instantiate (vehiclesList[profileVehicle-1]);
				GameObject weaponPositionObject = GameObject.FindWithTag ("WeaponPosition");
				if (weaponPositionObject == null)
				{
					Debug.Log("Cannot find 'WeaponPosition' object.");
					
				}
				GameObject playerWeaponObject = (GameObject)Instantiate (weaponsList[profileWeapon-1]);
				playerWeaponObject.transform.position = weaponPositionObject.transform.position;
				playerWeaponObject.transform.parent = weaponPositionObject.transform;
			}
		}
	}

	void OnGUI()
	{
		// header
		GUI.Label (headerArea, "<size=30>"+StaticTexts.Instance.PlayMenu_Header()+"</size>");

		// content
		for (int i=0; i < StaticTexts.Instance.PlayMenu_OptionsLength(); i++) 
		{
			if(selectedMenuOption == i)
			{
				GUI.color = Color.white;
			}
			else
			{
				GUI.color = Color.grey;
			}

			float menuOptionX = contentArea.width/4 - menuOptionWidth/2;
			float menuOptionY = contentArea.height/2 - menuOptionAreaHeight/2 + (menuOptionHeight * i) + (10*i);
			if(GUI.Button(new Rect(menuOptionX,
			                    menuOptionY,
			                    menuOptionWidth,
			                    menuOptionHeight), 
			           StaticTexts.Instance.PlayMenu_Options(i)))
			{
				SelectedOption(i);
			}
		}

		float guiTime = Time.time - statusTextStartTime;
		int restSeconds = Mathf.CeilToInt (statusTextTimeout - guiTime);
		if (restSeconds == 0) 
		{
			statusText.text = "";
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.DownArrow)
		    || Input.GetKeyDown (KeyCode.RightArrow)) 
		{
			selectedMenuOption++;
			if(selectedMenuOption == StaticTexts.Instance.PlayMenu_OptionsLength())
			{
				selectedMenuOption = 0;
			}
		}
		if (Input.GetKeyDown (KeyCode.UpArrow)
		    || Input.GetKeyDown (KeyCode.LeftArrow)) 
		{
			selectedMenuOption--;
			if(selectedMenuOption < 0)
			{
				selectedMenuOption = StaticTexts.Instance.PlayMenu_OptionsLength() - 1;
			}
		}
	}

	void InitializeVehiclesAndWeapons()
	{
		vehiclesList.Add (vehicles.vehicle1);
		vehiclesList.Add (vehicles.vehicle2);

		weaponsList.Add (weapons.weapon1);
	}

	void SelectedOption(int opt_num)
	{
		if (opt_num == 0) 
		{
			if(weapon_unlocked && vehicle_unlocked)
			{
				Application.LoadLevel("LevelSelector");
			}
			else
			{
				statusText.text = StaticTexts.Instance.PlayMenu_VehicleWeaponNotChosen();
				statusTextStartTime = Time.time;
			}
		}
		if (opt_num == 2) 
		{
			Application.LoadLevel("Customization");
		}
		if (opt_num == 3) 
		{
			Application.LoadLevel("MainMenu");
		}
	}
}
