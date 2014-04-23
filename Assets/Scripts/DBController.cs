using UnityEngine;
using System.Collections;
using System;
using System.Data;
using System.Data.SQLite;

public class DBController
{
	private SQLiteConnection con;

	public SQLiteDataReader ExecuteSqlForReader(string command)
	{
		SQLiteCommand cmd;
		cmd = con.CreateCommand ();
		cmd.CommandText = command;
		SQLiteDataReader sqlReader = cmd.ExecuteReader ();
		return sqlReader;
	}

	protected DBController()
	{
		con = new SQLiteConnection("Data Source=gamedata.db;Version=3;New=False;Compress=True");
		con.Open ();
	}
	private static DBController _instance = null;
	public static DBController Instance
	{
		get
		{
			return DBController._instance == null ? new DBController() : DBController._instance;
		}
	}
}
