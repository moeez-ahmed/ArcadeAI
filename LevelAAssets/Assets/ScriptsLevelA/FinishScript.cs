using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;

public class FinishScript : MonoBehaviour
{
    private int diamondBefore;
    private string username;
    private string grade;
    private int numberOfGames;


    private bool isFound = false;

    public Text name;
    public Text diamonds;
    public Text time;
    // Start is called before the first frame update
    void Start()
    {
        ChangeText(name, "Name = " + AddingInformation.alwaysName, Color.Lerp(Color.blue, Color.green, 0.42f));

        ChangeText(diamonds, "Diamonds Attained = " + MainMenuController.diamondsAttained.ToString(), Color.Lerp(Color.blue, Color.green, 0.42f));

        ReadDatabase(AddingInformation.alwaysName);

        UpdateDatabase(MainMenuController.diamondsAttained, MainMenuController.game);

        ChangeText(time, "Time Taken = " + MainMenuController.currentTime.ToString(), Color.Lerp(Color.blue, Color.green, 0.42f));
    }

    Text ChangeText(Text informationText, string information, Color color)
    {
        informationText.text = information;
        informationText.color = color;

        return informationText;
    }

    public void ReadDatabase(string name)
    {
        string conn = "URI=file:" + Application.dataPath + "/testdb.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT username, diamonds, grade, noOfGames " + "FROM user WHERE username = '" + name + "'";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            username = reader.GetString(0);
            diamondBefore = reader.GetInt32(1);
            grade = reader.GetString(2);
            numberOfGames = reader.GetInt32(3);
            //Debug.Log("username= " + username + "  diamonds =" + diamonds + "  grade =" + grade + "  Number of Games =" + numberOfGames);
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }

    public void UpdateDatabase(int newDiamonds, int noOfGames)
    {
        string conn = "URI=file:" + Application.dataPath + "/testdb.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "UPDATE user SET diamonds = '" + (newDiamonds + diamondBefore) + "', noOfGames = '" + (noOfGames + numberOfGames) + "' WHERE username = '" + username + "';";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();

        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }
}
