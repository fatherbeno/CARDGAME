using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(PlayerCore))]
public class PlayerMoveCard : NetworkBehaviour
{
    //player core used to inherit important data
    [HideInInspector] public PlayerCore playerCore;

    private GameObject selectedObject = null;
    private GameObject enemySelectedObject = null;
    private Vector3 objectOriginalPos;
    private Vector3 enemyObjectOriginalPos;

    private GameObject handObject;
    private GameObject enemyHandObject;

    private GameObject locationGameObject = null;
    private Vector3 locationOriginalPos;

    public enum WhichLayer {Card, Destination, Hand, Default};

    // Update is called once per frame
    void Update()
    {
        if(handObject == null)
        {
            handObject = playerCore.playerClient.transform.Find("Hand").gameObject;
            enemyHandObject = playerCore.enemyClient.transform.Find("Hand").gameObject;
        }
        else if(playerCore.playerCamera == null)
        {
            return;
        }
        else
        {
            if(isLocalPlayer)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (selectedObject == null)
                    {
                        RaycastHit hit = CastRay(WhichLayer.Card);

                        if (hit.collider != null)
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

        }
    }

    private RaycastHit CastRay(WhichLayer whichLayer)
    {
        Vector3 screenMousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, playerCore.playerCamera.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, playerCore.playerCamera.nearClipPlane);

        Vector3 worldMousePosFar = playerCore.playerCamera.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = playerCore.playerCamera.ScreenToWorldPoint(screenMousePosNear);
        
        RaycastHit hit;

        switch (whichLayer)
        {
            case WhichLayer.Card:
                Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit, 50, playerCore.cardLayer);
                return hit;
            case WhichLayer.Destination:
                Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit, 50, playerCore.cardDestinationLayer);
                return hit;
            case WhichLayer.Hand:
                Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit, 50, playerCore.playerHandLayer);
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

                Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, playerCore.playerCamera.WorldToScreenPoint(selectedObject.transform.position).z);
                Vector3 worldPosition = playerCore.playerCamera.ScreenToWorldPoint(position);
                selectedObject.transform.position = new Vector3(worldPosition.x, locationOriginalPos.y + height, worldPosition.z);
                playerCore.CoreMoveEnemyCard(selectedObject.transform.GetSiblingIndex(), selectedObject.transform.position.x, selectedObject.transform.position.z);

                selectedObject.tag = "moving";
            }
        }
    }

    public void MoveEnemyCard(int cardIndex, float cardPositionX, float cardPositionZ)
    {
        Debug.Log("BASE: " + cardPositionX + ", " + cardPositionZ + ", " + cardIndex);

        Vector3 translatedPosition = new Vector3(cardPositionX - 13.5f, 1f, cardPositionZ);

        if (enemySelectedObject != enemyHandObject.transform.GetChild(cardIndex).gameObject)
        {
            enemySelectedObject = enemyHandObject.transform.GetChild(cardIndex).gameObject;
            //enemyObjectOriginalPos = new Vector3();
        }

        if(enemySelectedObject.transform.position != translatedPosition)
        {
            enemySelectedObject.transform.position = translatedPosition;
        }
    }

    private void CardDestination()
    {
        RaycastHit destinationCheck = CastRay(WhichLayer.Destination);

        if (destinationCheck.collider != null && destinationCheck.collider.CompareTag("destination"))
        {
            int cardIndex = selectedObject.GetComponent<CardCore>().cardHandIndex;

            selectedObject.transform.SetParent(destinationCheck.collider.transform);
            selectedObject.transform.localPosition = new Vector3(
                destinationCheck.collider.transform.localPosition.x,
                destinationCheck.collider.transform.localPosition.y + 1f,
                destinationCheck.collider.transform.localPosition.z);

            playerCore.UpdateEnemyCardCount(cardIndex);
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