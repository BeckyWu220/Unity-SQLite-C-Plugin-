using UnityEngine;
using System.Collections;
using System.Data;
using Mono.Data.SqliteClient;


public class dbAccess {

	private IDbConnection _dbc;
	private IDbCommand _dbcm;
	private IDataReader _dbr;

	public void OpenDB(string databaseName)
	{
		_dbc = new SqliteConnection ("URI=file:" + databaseName);
		_dbc.Open ();
	}

	public void CreateTable(string name, ArrayList col, ArrayList colType)
	{
		string query = "CREATE TABLE " + name + "(" + col[0] + " " + colType[0];
		for (int i = 1; i < col.Count; i++) {
			query += ", " + col[i] + " " + colType[i];
		}
		query += ")";
		_dbcm = _dbc.CreateCommand ();
		_dbcm.CommandText = query;
		_dbr = _dbcm.ExecuteReader ();
	}

	public ArrayList Select(string name, string itemToSelect, string wCol, string wPar, string wValue)
	{
		string query = "SELECT " + itemToSelect + " FROM " + name + " WHERE " + wCol + wPar + wValue;
		_dbcm = _dbc.CreateCommand ();
		_dbcm.CommandText = query;
		_dbr = _dbcm.ExecuteReader ();

		ArrayList readArray = new ArrayList ();

		while (_dbr.Read ()) {

			for (int i = 0; i < _dbr.FieldCount; i++)
			{
				readArray.Add (_dbr.GetValue (i));
			}
		}

		return readArray;
	}

	public ArrayList SelectAll(string name, string itemToSelect, string wCol, string wPar, string wValue)
	{
		string query = "SELECT " + itemToSelect + " FROM " + name + " WHERE " + wCol + wPar + wValue;
		_dbcm = _dbc.CreateCommand ();
		_dbcm.CommandText = query;
		_dbr = _dbcm.ExecuteReader ();

		ArrayList readArray = new ArrayList ();

		while(_dbr.Read())
		{
			ArrayList lineArray = new ArrayList ();
			for (int i = 0; i < _dbr.FieldCount; i++)
			{
				lineArray.Add (_dbr.GetValue (i));
			}
			readArray.Add (lineArray);
		}
		return readArray;

	}

	public void Update(string name, string itemToUpdate, string value, string wCol, string wPar, string wValue)
	{
		string query = "UPDATE " + name + " SET " + itemToUpdate + "=" + value + " WHERE " + wCol + wPar + wValue;
		_dbcm = _dbc.CreateCommand ();
		_dbcm.CommandText = query;
		_dbr = _dbcm.ExecuteReader ();
	}

	public void InsertInto(string name, ArrayList values)
	{
		string query = "INSERT INTO " + name + " VALUES (" + values [0];
		for (int i = 1; i<values.Count; i++)
		{
			query += ", " + values[i];
		}
		query += ")";
		_dbcm = _dbc.CreateCommand ();
		_dbcm.CommandText = query;
		_dbr = _dbcm.ExecuteReader ();
	}

	public void DeleteFrom(string name, string wCol, string wPar, string wValue)
	{
		ArrayList record = Select (name, "*", wCol, wPar, wValue);

		string query = "DELETE FROM " + name + " WHERE " + wCol + wPar + wValue;
		_dbcm = _dbc.CreateCommand ();
		_dbcm.CommandText = query;
		_dbr = _dbcm.ExecuteReader ();

	}

	public ArrayList ReadWholeTable(string name)
	{
		string query = "SELECT * FROM " + name;
		_dbcm = _dbc.CreateCommand ();
		_dbcm.CommandText = query;
		_dbr = _dbcm.ExecuteReader ();

		ArrayList readArray = new ArrayList ();

		while(_dbr.Read())
		{
			ArrayList lineArray = new ArrayList ();
			for (int i = 0; i < _dbr.FieldCount; i++)
			{
				lineArray.Add (_dbr.GetValue (i));
			}
			readArray.Add (lineArray);
		}
		return readArray;
	}

	public void CloseDB()
	{
		_dbr.Close ();
		_dbr = null;
		_dbcm.Dispose ();
		_dbcm = null;
		_dbc.Close ();
		_dbc = null;
	}
}