using UnityEngine;
using System.Data.SQLite;

public class DBConnector : MonoBehaviour
{
    //private string m_path;
    string db_file;
    private string db_conn;
    void Start()
    {
        db_file = Application.dataPath + @"\Db\mydb.db";
       
        db_conn = "Data Source=" + db_file + ";Version=3;";
        Debug.Log("mpath: " + db_conn);


        //SQLiteConnection connection =
        //                  new SQLiteConnection(@"Data Source=F:\mydb.db;Version=3;");

        SQLiteConnection connection =
            new SQLiteConnection(@db_conn);
        connection.Open();
        SQLiteCommand command = connection.CreateCommand();
        command.CommandType = System.Data.CommandType.Text;
        command.CommandText = "CREATE TABLE IF NOT EXISTS 'highscores' ( " +
                          "  'id' INTEGER PRIMARY KEY, " +
                          "  'name' TEXT NOT NULL, " +
                          "  'score' INTEGER NOT NULL" +
                          ");";
        command.ExecuteNonQuery();
        connection.Close();

    }

}

