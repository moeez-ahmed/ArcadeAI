using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;

public class MainMenuController : MonoBehaviour
{
    public static float currentTime = 0f;
    public static int game = 1;
    public static int diamondsAttained = 0;

    public GameObject help;
    public GameObject MainMenu;
    public GameObject infomationPanel;

    public Text name;
    public Text gamesInTotal;
    public Text diamondsForever;



    private int diamonds;
    private string username;
    private string grade;
    private int numberOfGames;


    public void PlayGame()
    {
        SceneLoader.instance.LoadLevel("SampleScene");
    }

    public void helpLoad()
    {
        MainMenu.SetActive(false);
        help.SetActive(true);
    }

    public void Back()
    {
        MainMenu.SetActive(true);
        help.SetActive(false);
        infomationPanel.SetActive(false);
    }

    public void Information()
    {
        MainMenu.SetActive(false);

        ReadDatabase(AddingInformation.alwaysName);

        ChangeText(name, "Name = " + username, Color.Lerp(Color.blue, Color.green, 0.42f));
        ChangeText(diamondsForever, "Diamonds Attained = " + diamonds, Color.Lerp(Color.blue, Color.green, 0.42f));
        ChangeText(gamesInTotal, "Number of Games = " + numberOfGames, Color.Lerp(Color.blue, Color.green, 0.42f));

        infomationPanel.SetActive(true);
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
            diamonds = reader.GetInt32(1);
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

    Text ChangeText(Text informationText, string information, Color color)
    {
        informationText.text = information;
        informationText.color = color;

        return informationText;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
