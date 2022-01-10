using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    private GameObject selectedObject;
    private Vector3 objectOriginalPos;

    int cardLayer = 1 << 12;
    int destinationLayer = 1 << 11;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(selectedObject == null)
            {
                RaycastHit hit = CastRay(true);

                if(hit.collider != null)
                {
                    if (!hit.collider.CompareTag("drag"))
                    {
                        return;
                    }

                    selectedObject = hit.collider.gameObject;
                    objectOriginalPos = selectedObject.transform.position;
                    Cursor.visible = false;
                }
            }
            else
            {
                RaycastHit destinationCheck = CastRay(false);

                if(destinationCheck.collider != null)
                {
                    if(destinationCheck.collider.CompareTag("destination"))
                    {
                        selectedObject.transform.position = new Vector3(destinationCheck.collider.transform.position.x, 
                                                                        destinationCheck.collider.transform.position.y + 1f, 
                                                                        destinationCheck.collider.transform.position.z);
                    }
                    else
                    {
                        selectedObject.transform.position = objectOriginalPos;
                    }
                }
                else
                {
                    selectedObject.transform.position = objectOriginalPos;
                }

                selectedObject = null;
                Cursor.visible = true;
            }
        }

        if(selectedObject != null)
        {
            MoveObject(0.25f);
        }
    }

    private RaycastHit CastRay(bool isCard)
    {
        Vector3 screenMousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);

        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        
        RaycastHit hit;

        if (isCard)
        {
            Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit, 100, cardLayer);
        }
        else
        {
            Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit, 100, destinationLayer);
        }
        

        return hit;
    }

    private void MoveObject(float height)
    {
        if(selectedObject != null)
        {
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
            selectedObject.transform.position = new Vector3(worldPosition.x, objectOriginalPos.y + height, worldPosition.z);
        }
        
    }
}
