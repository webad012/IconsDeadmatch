using UnityEngine;
using System.Collections;

public class StaticTexts
{
	private string[] _GameController_EndGame = {"Kraj igre!", "Game over!"};
	private string[] _GameController_PickUpsLeft = {"Preostalo: ", "Remaining: "};
	private string[] _GameController_Time = {"Vreme: ", "Time: "};
	private string[] _GameController_Win = {"Pobeda!", "Win!"};
	private static string[] _MainMenu_Option1 = {"Igraj", "Play"};
	private static string[] _MainMenu_Option2 = {"Rankiranja", "Rankings"};
	private static string[] _MainMenu_Option3 = {"Opcije", "Options"};
	private static string[] _MainMenu_Option4 = {"Izadji", "Exit"};
	private string[][] _MainMenu_Options = new string[][]{_MainMenu_Option1,
															_MainMenu_Option2,
															_MainMenu_Option3,
															_MainMenu_Option4};
	private string[] _MainMenu_Header = {"Glavni meni", "Main menu"};
	private string[] _MainMenu_NotYourProfile = {"(N)ije vas profil?", "(N)ot your profile?"};
	private string[] _MainMenu_ProfileNotSelected = {"Molimo izaberite/kreirajte profil", "Please select/create profile"};
	private string[] _PlayerProfiles_Header = {"Profili", "Profiles"};
	private string[] _PlayerProfiles_NewNameDefault = {"Novo ime", "New Name"};
	private string[] _PlayerProfiles_CreateNewButton = {"Kreiraj novo", "Create new"};
	private string[] _PlayerProfiles_NameTaken = {"Ime zauzeto", "Name taken"};
	private string[] _PlayerProfiles_Rename = {"Preimenuj", "Rename"};
	private string[] _PlayerProfiles_DeleteDialogTitle = {"Brisanje profila", "Delete profile"};
	private string[] _PlayerProfiles_DeleteDialogMessage = {"Da li ste sigurni da zelite da obrisete profil:\n", "Are you sure you want to delete profile:\n"};
	private string[] _Yes = {"Da", "Yes"};
	private string[] _No = {"Ne", "No"};
	private string[] _PlayMenu_Header = {"Meni igranja", "Play menu"};
	private string[] _PlayMenu_Name = {"Ime: ", "Name: "};
	private string[] _PlayMenu_VehicleWeaponNotChosen = {"Vozilo i/ili oruzje nije izabrano (Kustomizacija)", "Vehicle and/or weapon not chosen (Customization)"};
	private string[] _Money = {"Novac: ", "Money: "};
	private static string[] _PlayMenu_Option1 = {"Jedan igrac", "Single player"};
	private static string[] _PlayMenu_Option2 = {"Vise igraca", "Multiplayer"};
	private static string[] _PlayMenu_Option3 = {"Kustomizacija", "Customization"};
	private static string[] _PlayMenu_Option4 = {"Nazad", "Back"};
	private string[][] _PlayMenu_Options = new string[][]{_PlayMenu_Option1,
															_PlayMenu_Option2,
															_PlayMenu_Option3,
															_PlayMenu_Option4};
	private static string[] _Customization_Price = {"Cena: ", "Price: "};
	private static string[] _Customization_HeaderVehicles = {"Kustomizacija vozila", "Vehicle customization"};
	private static string[] _Customization_HeaderWeapons = {"Kustomizacija oruzja", "Weapon customization"};
	private static string[] _Customization_Select = {"Izaberi", "Select"};
	private static string[] _Customization_Unlock = {"Otkljucaj", "Unlock"};
	private static string[] _Customization_Back = {"Nazad", "Back"};
	private static string[] _Customization_NotEnoughMoney = {"Nedovoljno novca", "Not enough money"};
	private static string[] _Customization_AdditionalInfoDamage = {"Snaga: ", "Power: "};
	private static string[] _Customization_AdditionalInfoMoveSpeed = {"Brzina kretanja: ", "Move speed: "};
	private static string[] _Customization_AdditionalInfoTurnSpeed = {"Brzina skretanja: ", "Turn speed: "};

	public string Customization_AdditionalInfoMoveSpeed()
	{
		return _Customization_AdditionalInfoMoveSpeed[PlayerPrefs.GetInt ("SelectedLanguage", 0)];
	}

	public string Customization_AdditionalInfoTurnSpeed()
	{
		return _Customization_AdditionalInfoTurnSpeed[PlayerPrefs.GetInt ("SelectedLanguage", 0)];
	}

	public string Customization_AdditionalInfoDamage()
	{
		return _Customization_AdditionalInfoDamage[PlayerPrefs.GetInt ("SelectedLanguage", 0)];
	}

	public string PlayMenu_VehicleWeaponNotChosen()
	{
		return _PlayMenu_VehicleWeaponNotChosen[PlayerPrefs.GetInt ("SelectedLanguage", 0)];
	}

	public string Customization_NotEnoughMoney()
	{
		return _Customization_NotEnoughMoney[PlayerPrefs.GetInt ("SelectedLanguage", 0)];
	}

	public string Customization_Select()
	{
		return _Customization_Select[PlayerPrefs.GetInt ("SelectedLanguage", 0)];
	}

	public string Customization_Unlock()
	{
		return _Customization_Unlock[PlayerPrefs.GetInt ("SelectedLanguage", 0)];
	}

	public string Customization_Back()
	{
		return _Customization_Back[PlayerPrefs.GetInt ("SelectedLanguage", 0)];
	}

	public string Money()
	{
		return _Money[PlayerPrefs.GetInt ("SelectedLanguage", 0)];
	}

	public string PlayMenu_Name()
	{
		return _PlayMenu_Name[PlayerPrefs.GetInt ("SelectedLanguage", 0)];
	}

	public string MainMenu_ProfileNotSelected()
	{
		return _MainMenu_ProfileNotSelected[PlayerPrefs.GetInt ("SelectedLanguage", 0)];
	}

	public string Customization_HeaderVehicles()
	{
		return _Customization_HeaderVehicles[PlayerPrefs.GetInt ("SelectedLanguage", 0)];
	}

	public string Customization_HeaderWeapons()
	{
		return _Customization_HeaderWeapons[PlayerPrefs.GetInt ("SelectedLanguage", 0)];
	}

	public string Customization_Price()
	{
		return _Customization_Price[PlayerPrefs.GetInt ("SelectedLanguage", 0)];
	}

	public int PlayMenu_OptionsLength()
	{
		return _PlayMenu_Options.Length;
    }
    
	public string PlayMenu_Options(int opt_num)
    {
		return _PlayMenu_Options[opt_num][PlayerPrefs.GetInt ("SelectedLanguage", 0)];
    }
    
    public string PlayMenu_Header()
	{
		return _PlayMenu_Header[PlayerPrefs.GetInt ("SelectedLanguage", 0)];
    }

	public string Yes()
	{
		return _Yes[PlayerPrefs.GetInt ("SelectedLanguage", 0)];
	}

	public string No()
	{
		return _No[PlayerPrefs.GetInt ("SelectedLanguage", 0)];
	}
	
	public string PlayerProfiles_DeleteDialogMessage()
	{
		return _PlayerProfiles_DeleteDialogMessage[PlayerPrefs.GetInt ("SelectedLanguage", 0)];
	}

	public string PlayerProfiles_DeleteDialogTitle()
	{
		return _PlayerProfiles_DeleteDialogTitle[PlayerPrefs.GetInt ("SelectedLanguage", 0)];
	}

	public string PlayerProfiles_Rename()
	{
		return _PlayerProfiles_Rename[PlayerPrefs.GetInt ("SelectedLanguage", 0)];
	}

	public string PlayerProfiles_NameTaken()
	{
		return _PlayerProfiles_NameTaken[PlayerPrefs.GetInt ("SelectedLanguage", 0)];
	}

	public string PlayerProfiles_CreateNewButton()
	{
		return _PlayerProfiles_CreateNewButton[PlayerPrefs.GetInt ("SelectedLanguage", 0)];
	}

	public string PlayerProfiles_NewNameDefault()
	{
		return _PlayerProfiles_NewNameDefault[PlayerPrefs.GetInt ("SelectedLanguage", 0)];
	}

	public string PlayerProfiles_Header()
	{
		return _PlayerProfiles_Header[PlayerPrefs.GetInt ("SelectedLanguage", 0)];
	}

	public string MainMenu_NotYourProfile()
	{
		return _MainMenu_NotYourProfile[PlayerPrefs.GetInt ("SelectedLanguage", 0)];
	}

	public string MainMenu_Header()
	{
		return _MainMenu_Header[PlayerPrefs.GetInt ("SelectedLanguage", 0)];
	}

	public string MainMenu_Options(int opt_num)
	{
		return _MainMenu_Options[opt_num][PlayerPrefs.GetInt ("SelectedLanguage", 0)];
	}

	public int MainMenu_OptionLength()
	{
		return _MainMenu_Options.Length;
	}

	public string GameController_Win()
	{
		return _GameController_Win[PlayerPrefs.GetInt ("SelectedLanguage", 0)];
	}

	public string GameController_Time()
	{
		return _GameController_Time[PlayerPrefs.GetInt ("SelectedLanguage", 0)];
	}

	public string GameController_PickUpsLeft()
	{
		return _GameController_PickUpsLeft[PlayerPrefs.GetInt ("SelectedLanguage", 0)];
	}

	public string GameController_EndGame()
	{
		return _GameController_EndGame[PlayerPrefs.GetInt ("SelectedLanguage", 0)];
	}

	protected StaticTexts(){}
	private static StaticTexts _instance = null;
	public static StaticTexts Instance
	{
		get
		{
			return StaticTexts._instance == null ? new StaticTexts() : StaticTexts._instance;
		}
	}
}
