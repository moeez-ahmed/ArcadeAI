using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private GameObject loadingScreen;

    private string levelName;

    public static SceneLoader instance;
    // Start is called before the first frame update
    void Awake()
    {
        MakeSingleton();
    }

    void MakeSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LoadLevel(string name)
    {
        levelName = name;
        StartCoroutine(LoadLevelWithName());
    }

    IEnumerator LoadLevelWithName()
    {
        loadingScreen.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(levelName);
        yield return new WaitForSeconds(0.5f);
        loadingScreen.SetActive(false);
    }
}
