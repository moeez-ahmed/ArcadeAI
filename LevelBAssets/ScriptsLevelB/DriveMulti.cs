using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DriveMulti : MonoBehaviour
{
    private float positionY;
    private float positionYChange;

    public float speed = 22f;
    public float rotationSpeed = 100.0f;

    public Image diamond;
    public Text informationText;
    public GameObject goal;

    private void OnTriggerEnter(Collider target)
    {
        if (target.gameObject == goal)
        {
            Debug.Log("ENTERED");
            if (!diamond.gameObject.activeInHierarchy)
            {
                diamond.gameObject.SetActive(true);
                diamond.fillAmount = 1f;
                MainMenuController.diamondsAttained += 1;
                Debug.Log(MainMenuController.diamondsAttained);

                ChangeText(informationText, "Diamond attained, grats!", Color.yellow);

                StartCoroutine(WaitAndExecute());
            }
            goal.SetActive(false);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "wall")
        {
            StartCoroutine(CollissionCoroutine());
        }
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
        ChangeText(informationText, "You`ll be taken to next stage.", Color.green);
        Debug.Log(MainMenuController.diamondsAttained);
        yield return new WaitForSeconds(5f);
        if (MainMenuController.diamondsAttained <= 2)
        {
            SceneLoader.instance.LoadLevel("Level A");
        }
        else if (MainMenuController.diamondsAttained == 3)
        {
            print("The final time = " + MainMenuController.currentTime);

            SceneLoader.instance.LoadLevel("FinishScreen");
        }
    }

    IEnumerator CollissionCoroutine()
    {
        yield return new WaitForSeconds(1f);
        ChangeText(informationText, "You lost.", Color.yellow);
        Debug.Log("Number of diamonds = " + MainMenuController.diamondsAttained);
        yield return new WaitForSeconds(3f);
        SceneLoader.instance.LoadLevel("FinishScreen");
    }

    void Update()
    {
        // Get the horizontal and vertical axis.
        // By default they are mapped to the arrow keys.
        // The value is in the range -1 to 1
        MainMenuController.currentTime += 1 * Time.deltaTime;
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

        // Make it move 10 meters per second instead of 10 meters per frame...
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        // Move translation along the object's z-axis
        transform.Translate(0, 0, translation);

        // Rotate around our y-axis
        transform.Rotate(0, rotation, 0);
    }
}
