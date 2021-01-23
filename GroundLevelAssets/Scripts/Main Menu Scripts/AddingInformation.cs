using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AddingInformation : MonoBehaviour
{
    public InputField name;
    private TestDB test;

    public static string alwaysName;
    private bool isFound = false;

    private void Start()
    {
        test = new TestDB();
    }
    public void PlayGame()
    {
        if (Check() == true)
        {
            alwaysName = name.text;
            SceneLoader.instance.LoadLevel("Main Menu");
        }
        else
        {
            alwaysName = name.text;
            test.InserDatabase(name.text);
            SceneLoader.instance.LoadLevel("Main Menu");
        }
    }

    private bool Check()
    {
        Debug.Log(name.text);
        isFound = test.ReadDatabase(name.text);
        return isFound;
    }
}
