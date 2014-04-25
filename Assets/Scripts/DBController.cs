using UnityEngine;
using System.Collections;
using System;
using System.Data;
using System.Data.SQLite;

//using System.Text;
//using System.Collections.Generic;
//using iBoxDB.LocalServer;

public class DBController
{
	private SQLiteConnection con;
	//DB server = new DB(1, "");
	
	public SQLiteDataReader ExecuteSqlForReader(string command)
	{
		SQLiteCommand cmd;
		cmd = con.CreateCommand ();
		cmd.CommandText = command;
		return cmd.ExecuteReader ();
	}

	public string ExecuteSqlForStringScalar(string command)
	{
		SQLiteCommand cmd;
		cmd = con.CreateCommand ();
		cmd.CommandText = command;
		string result = Convert.ToString(cmd.ExecuteScalar ());
		return result;
	}

	public void ExecuteSqlForNonQuery(string command)
	{
		SQLiteCommand cmd;
		cmd = con.CreateCommand ();
		cmd.CommandText = command;
		cmd.ExecuteNonQuery ();
	}

	protected DBController()
	{
		//server.EnsureTable<Record> ("PlayerProfiles", "id");
		//float qwe = server.Open ();
		con = new SQLiteConnection("Data Source=gamedata.db;Version=3;New=False;Compress=True");
		con.Open ();

		SQLiteCommand cmd;
		cmd = con.CreateCommand ();
		cmd.CommandText = "CREATE TABLE IF NOT EXISTS PlayerProfiles (id INTEGER PRIMARY KEY, name TEXT, level NUMERIC damage NUMERIC, health NUMERIC, move_speed NUMERIC, turn_speed NUMERIC)";
		cmd.ExecuteNonQuery ();
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
