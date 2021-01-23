using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBLoader : MonoBehaviour
{
    void Update()
    {
        MainMenuController.currentTime += 1 * Time.deltaTime;
        //print(MainMenuController.currentTime);
    }
    public void PlayGame()
    {
        SceneLoader.instance.LoadLevel("Starter");
    }
}
