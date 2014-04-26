using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LevelSelectorController : MonoBehaviour 
{
	private List<int> levelsList = new List<int>();

	// Use this for initialization
	void Start () 
	{
		StartCoroutine (SetupLevelsList());
	}

	IEnumerator SetupLevelsList()
	{
		string sqlCommand;
		
		sqlCommand = "SELECT COUNT(*) FROM Levels";
		int levelsCount = DBController.Instance.ExecuteSqlForIntScalar (sqlCommand);
		
		for (int i=0; i<levelsCount; i++) 
		{
			sqlCommand = "SELECT id FROM PlayerProfiles LIMIT 1 OFFSET " + i.ToString() + ";";
			object[] sqlReader = new object[1];
			DBController.Instance.ExecuteSqlForReader (sqlCommand, sqlReader);
			
			levelsList.Add(Convert.ToInt32(sqlReader[0].ToString()));
		}
		
		yield return new WaitForSeconds (0);
	}

	void OnGUI()
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
