using UnityEngine;
using System.Collections;
using System.Data;
using Mono.Data.SqliteClient;


public class DatabaseAccess {

	private IDbConnection _dbc;
	private IDbCommand _dbcm;
	private IDataReader _dbr;

	/** Open the database.
	 @param databaseName databaseName. For example, "ItemDB.sqdb". This .sqdb file should be in the root folder of project.*/
	public void OpenDB(string databaseName)
	{
		_dbc = new SqliteConnection ("URI=file:" + databaseName);
		_dbc.Open ();
	}

	/** Create a new table.
	 * @param name Table name.
	 * @param col an ArrayList of column names.
	 * @param colType an ArrayList of column types.
	 * Example: CreateTable("itemTable", {"ID", "Name"}, {"INTEGER", "TEXT"});
	 * //CREATE TABLE ItemTable (ID INTEGER, Name TEXT);
	 */
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

	/** Select and return a single column of all the matched records.
	 * @param name Table name.
	 * @param itemToSelect Column name that needs to be selected.
	 * @param wCol The column name
	 * @param wPar The operator that is used to compare with. For example, "=",">=",">",etc.
	 * @param wValue The value that is compared against wCol.
	 * @return an ArrayList that contains the values in the column.
	 * Example: Select("itemTable", "Name", "ID", "=", "1");
	 * //SELECT Name FROM itemTable WHERE ID=1;
	 */
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

	/**Select and return all columns of all the matched records.
	 * @return an ArrayList that contains all matched records. Each record is an ArrayList that contains values of each column as well.
	 @see Select(string name, string itemToSelect, string wCol, string wPar, string wValue)
	 */
	public ArrayList SelectAll(string name, string wCol, string wPar, string wValue)
	{
		string query = "SELECT * FROM " + name + " WHERE " + wCol + wPar + wValue;
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

	/** Update certain fields in the database.
	 * @param name Table name.
	 * @param itemToUpdate The name of the field that is going to be updated.
	 * @param value The field is going to be filled with this value.
	 * @param wCol The column name
	 * @param wPar The operator that is used to compare with. For example, "=",">=",">",etc.
	 * @param wValue The value that is compared against wCol.
	 * Example: Update("itemTable", "Name", "Bread", "ID", "=", "1"); 
	 * //UPDATE itemTable SET Name = 'Bread' WHERE ID = 1
	 * //Find the column in "itemTable" whose ID is equal to 1, and update its Name to "Bread".
	 */
	public void Update(string name, string itemToUpdate, string value, string wCol, string wPar, string wValue)
	{
		string query = "UPDATE " + name + " SET " + itemToUpdate + "=" + value + " WHERE " + wCol + wPar + wValue;
		_dbcm = _dbc.CreateCommand ();
		_dbcm.CommandText = query;
		_dbr = _dbcm.ExecuteReader ();
	}

	/** Insert a piece of new record to the table.
	 @param name Table name.
	 @param values An ArrayList that contains all the values of this new record. The values need to keep the same order as the table's columns.
	 Example: InsertInto("itemTable", {"1", "Cookie"});
	 //INSERT INTO itemTable VALUES ("1", "Cookie")
	 //Create a new record in itemTable, and all its field are assigned with the ArrayList - values.*/
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

	/** Delete certain pieces of records.
	 *@param name Table name
	 *@param wCol The column name
	 *@param wPar The operator that is used to compare with. For example, "=",">=",">",etc.
	 *@param wValue The value that is compared against wCol.
	 *Example: DeleteFrom("itemTable", "ID", "=", "1"); 
	 * //DELETE FROM itemTable * WHERE ID = 1
	 * //Find the record in "itemTable" whose ID is equal to 1, and delete this record.
	 */
	public void DeleteFrom(string name, string wCol, string wPar, string wValue)
	{
		ArrayList record = Select (name, "*", wCol, wPar, wValue);

		string query = "DELETE FROM " + name + " WHERE " + wCol + wPar + wValue;
		_dbcm = _dbc.CreateCommand ();
		_dbcm.CommandText = query;
		_dbr = _dbcm.ExecuteReader ();

	}

	/** Read the whole table.
	 * @param name Table name
	 * @return an ArrayList that contains the all the records. Each record is an ArrayList that contains the values of all columns as well.
	 */
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

	/** Close the database.*/
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