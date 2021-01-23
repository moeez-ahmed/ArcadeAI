using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyPlayerControlLevelA : MonoBehaviour
{
    public Image[] girlImage;
    public GameObject[] girls;
    public Image diamond;
    public Text informationText;

    private int randomNumber;
    private int getTarget;
    private GameObject goal;

    // Start is called before the first frame update
    void Start()
    {
        getTarget = generateRandom();
        goal = girls[getTarget];
    }

    private void OnTriggerEnter(Collider target)
    {
        if (target.gameObject == goal)
        {
            if (!diamond.gameObject.activeInHierarchy)
            {
                diamond.gameObject.SetActive(true);
                diamond.fillAmount = 1f;
                MainMenuController.diamondsAttained += 1;
                Debug.Log(MainMenuController.diamondsAttained);

                ChangeText(informationText, "Diamond attained, grats!", Color.yellow);

                StartCoroutine(WaitAndExecute());
            }
            girlImage[randomNumber].enabled = false;
            goal.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        girlImage[randomNumber].fillAmount += 0.1f * Time.deltaTime;
        MainMenuController.currentTime += 1 * Time.deltaTime;
        //print(MainMenuController.currentTime);
    }

    private int generateRandom()
    {
        randomNumber = Random.Range(0, 2);
        return randomNumber;
    }

    Text ChangeText(Text informationText, string information, Color color)
    {
        informationText.text = information;
        informationText.color = color;

        return informationText;
    }

    IEnumerator WaitAndExecute()
    {
        yield return new WaitForSeconds(3f);
        ChangeText(informationText, "Find the next stage!", Color.green);
        yield return new WaitForSeconds(25f);
        ChangeText(informationText, "The shop you are trying to find is; \n OLD TRAFFORD, be quick.", Color.red);
    }
}
