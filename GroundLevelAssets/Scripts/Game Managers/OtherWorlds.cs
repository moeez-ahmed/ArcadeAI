using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OtherWorlds : MonoBehaviour
{
    public Image diamond;
    //public GameObject chosenOne;
    private void OnTriggerEnter(Collider target)
    {
        if (target.tag == "Player")
        {
            if (diamond.fillAmount == 1f)
            {
                //Destroy(chosenOne);
                if (MainMenuController.diamondsAttained <= 2)
                {
                    SceneLoader.instance.LoadLevel(this.name);
                }
                else if(MainMenuController.diamondsAttained == 3)
                {
                    SceneLoader.instance.LoadLevel("FinishScreen");
                }
            }
        }
    }
}
