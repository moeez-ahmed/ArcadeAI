using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseScript : MonoBehaviour
{

    public Texture2D cursorTexture;
    public GameObject mousePoint;
    private CursorMode mode = CursorMode.ForceSoftware;
    private Vector2 hotSpot = Vector2.zero;

    private GameObject instantiatedMouse;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, mode);
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider is TerrainCollider)
                {
                    Vector3 temp = hit.point;
                    temp.y = 0.25f;
                    if (instantiatedMouse == null)
                    {
                        instantiatedMouse = Instantiate(mousePoint, temp, Quaternion.identity) as GameObject;
                    }
                    else
                    {
                        Destroy(instantiatedMouse);
                        instantiatedMouse = Instantiate(mousePoint, temp, Quaternion.identity) as GameObject;
                    }
                }
            }
        }
    }
}
