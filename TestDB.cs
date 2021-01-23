using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;


public class TestDB : MonoBehaviour
{
    private int diamonds;
    private string username;
    private string grade;
    private int numberOfGames;

    void Start()
    {
        //InserDatabase("");
        //ReadDatabase();
        //UpdateDatabase(2, 3);
        //ReadDatabase();
    }

    public void CheckDatabase()
    {
        string conn = "URI=file:" + Application.dataPath + "/testdb.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT username, diamonds, grade, noOfGames " + "FROM user";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            username = reader.GetString(0);
            diamonds = reader.GetInt32(1);
            grade = reader.GetString(2);
            numberOfGames = reader.GetInt32(3);
            Debug.Log("username= " + username + "  diamonds =" + diamonds + "  grade =" + grade + "  Number of Games =" + numberOfGames);
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }

    public bool ReadDatabase(string name)
    {
        string conn = "URI=file:" + Application.dataPath + "/testdb.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT username, diamonds, grade, noOfGames " + "FROM user WHERE username = '" + name +"'";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            username = reader.GetString(0);
            diamonds = reader.GetInt32(1);
            grade = reader.GetString(2);
            numberOfGames = reader.GetInt32(3);

            if (username == name)
            {
                return true;
            }
            //Debug.Log("username= " + username + "  diamonds =" + diamonds + "  grade =" + grade + "  Number of Games =" + numberOfGames);
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
        return false;
    }

    public void InserDatabase(string name)
    {
        string conn = "URI=file:" + Application.dataPath + "/testdb.db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "INSERT INTO user VALUES ('" + name + "', '0', '0', '0')";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();

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
        string sqlQuery = "UPDATE user SET diamonds = '" + (newDiamonds + diamonds) + "', noOfGames = '" + (noOfGames + numberOfGames) + "' WHERE username = '" + username + "';";
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
