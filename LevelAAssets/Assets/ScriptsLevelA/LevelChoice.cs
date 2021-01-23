using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelChoice : MonoBehaviour
{
    void Update()
    {
        MainMenuController.currentTime += 1 * Time.deltaTime;
        //print(MainMenuController.currentTime);
    }
    public void PlayGame()
    {
        SceneLoader.instance.LoadLevel("Crowd");
    }
}
