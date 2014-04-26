using UnityEngine;
using System.Collections;
using System;
using System.Data;
using System.Data.SQLite;

public class DBController
{
	private string connectionString = "Data Source=gamedata.db;Version=3;New=False;Compress=True";

	public void ExecuteSqlForReader(string command, object[] result)
	{
		using (SQLiteConnection c = new SQLiteConnection(connectionString)) 
		{
			c.Open();
			using(SQLiteCommand cmd = new SQLiteCommand(command, c))
			{
				using(SQLiteDataReader rdr = cmd.ExecuteReader())
				{
					rdr.Read();
					rdr.GetValues(result);
				}
				cmd.Dispose();
			}
			c.Close();
		}
	}

	public string ExecuteSqlForStringScalar(string command)
	{
		using (SQLiteConnection c = new SQLiteConnection(connectionString)) 
		{
			c.Open();
			using(SQLiteCommand cmd = new SQLiteCommand(command, c))
			{
				string result = Convert.ToString (cmd.ExecuteScalar ());
				cmd.Dispose();
				c.Close();
				return result;
			}
		}
	}

	public int ExecuteSqlForIntScalar(string command)
	{
		using (SQLiteConnection c = new SQLiteConnection(connectionString)) 
		{
			c.Open();
			using(SQLiteCommand cmd = new SQLiteCommand(command, c))
			{
				int result = Convert.ToInt32(cmd.ExecuteScalar ());
				cmd.Dispose();
				c.Close();
				return result;
			}
		}
	}

	public void ExecuteSqlForNonQuery(string command)
	{
		using (SQLiteConnection c = new SQLiteConnection(connectionString)) 
		{
			c.Open();
			using(SQLiteCommand cmd = new SQLiteCommand(command, c))
			{
				cmd.ExecuteNonQuery();
				cmd.Dispose();
			}
			c.Close();
		}
	}

	protected DBController()
	{
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
