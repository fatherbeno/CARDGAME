using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    private GameObject selectedObject;
    private Vector3 objectOriginalPos;

    private GameObject handObject;

    private GameObject locationGameObject = null;
    private Vector3 locationOriginalPos;

    public enum WhichLayer {Card, Destination, Hand, Default};

    [SerializeField]
    private LayerMask cardLayer;
    [SerializeField]
    private LayerMask destinationLayer;
    [SerializeField]
    private LayerMask handLayer;

    private void Awake()
    {
        handObject = this.transform.Find("Hand").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(selectedObject == null)
            {
                RaycastHit hit = CastRay(WhichLayer.Card);

                if(hit.collider != null)
                {
                    if (!hit.collider.CompareTag("drag"))
                    {
                        return;
                    }

                    selectedObject = hit.collider.gameObject;
                    objectOriginalPos = selectedObject.transform.localPosition;
                    Cursor.visible = false;
                }
            }
            else
            {
                CardDestination();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            FlipCard();
        }

        if (selectedObject != null)
        {
            MoveObject(0.25f);
        }

        
    }

    private RaycastHit CastRay(WhichLayer whichLayer)
    {
        Vector3 screenMousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);

        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        
        RaycastHit hit;

        switch (whichLayer)
        {
            case WhichLayer.Card:
                Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit, 50, cardLayer);
                return hit;
            case WhichLayer.Destination:
                Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit, 50, destinationLayer);
                return hit;
            case WhichLayer.Hand:
                Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit, 50, handLayer);
                return hit;
            case WhichLayer.Default:
                Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit, 50);
                return hit;
            default:
                Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit, 50);
                return hit;
        }
    }

    private void MoveObject(float height)
    {
        if(selectedObject != null)
        {
            RaycastHit location = CastRay(WhichLayer.Hand);

            if(location.collider != null)
            {
                if (locationGameObject != location.collider.gameObject)
                {
                    locationGameObject = location.collider.gameObject;
                    locationOriginalPos = locationGameObject.transform.position;

                    selectedObject.transform.rotation = Quaternion.Euler(new Vector3(
                    locationGameObject.transform.rotation.eulerAngles.x,
                    selectedObject.transform.rotation.eulerAngles.y,
                    selectedObject.transform.rotation.eulerAngles.z));
                }

                Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
                selectedObject.transform.position = new Vector3(worldPosition.x, locationOriginalPos.y + height, worldPosition.z);

                selectedObject.tag = "moving";
            }
        }
    }

    private void CardDestination()
    {
        RaycastHit destinationCheck = CastRay(WhichLayer.Destination);

        if (destinationCheck.collider != null && destinationCheck.collider.CompareTag("destination"))
        {
            selectedObject.transform.SetParent(destinationCheck.collider.transform);
            selectedObject.transform.localPosition = new Vector3(
                destinationCheck.collider.transform.localPosition.x,
                destinationCheck.collider.transform.localPosition.y + 1f,
                destinationCheck.collider.transform.localPosition.z);
        }
        else
        {
            selectedObject.transform.localPosition = objectOriginalPos;
            selectedObject.transform.rotation = Quaternion.Euler(new Vector3(
                handObject.transform.rotation.eulerAngles.x,
                selectedObject.transform.rotation.eulerAngles.y,
                selectedObject.transform.rotation.eulerAngles.z));
        }

        selectedObject.tag = "drag";
        selectedObject = null;
        Cursor.visible = true;
    }

    private void FlipCard()
    {
        if (selectedObject != null)
        {
            selectedObject.transform.rotation = Quaternion.Euler(new Vector3(
                selectedObject.transform.rotation.eulerAngles.x,
                selectedObject.transform.rotation.eulerAngles.y,
                selectedObject.transform.rotation.eulerAngles.z + 180f));
        }
    }
}