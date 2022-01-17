using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandOfCards : MonoBehaviour
{
    private int siblingIndex;

    [SerializeField]
    private GameObject cardPrefab;
    private List<GameObject> cards = new List<GameObject>();

    private GameObject hoveredCard = null;
    private Vector3 hoveredCardOriginalPos;

    private GameObject handObject;
    private Vector3 handOriginalPos;

    private int cardAmount;
    private int middleCard;
    private int counter = 0;

    private float distanceBetweenCards = 2f;

    [SerializeField]
    private LayerMask cardLayer;
    [SerializeField]
    private LayerMask handLayer;


    // Start is called before the first frame update
    private void Awake()
    {
        handObject = this.transform.Find("Hand").gameObject;
        handOriginalPos = handObject.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (cardAmount < 10)
            {
                Instantiate(cardPrefab, handObject.transform.position, handObject.transform.rotation, handObject.transform);

                SortCards();
            }
        }

        if(cardAmount != handObject.transform.childCount)
        {
            if (hoveredCard != null)
            {
                ResetCard();
                hoveredCard = null;
            }

            SortCards();
        }

        RaycastHit hand = CastRay(false);

        if (hand.collider == null)
        {
            return;
        }
        else
        {
            if (hand.collider.CompareTag("hand"))
            {
                if (handObject.transform.localPosition != handOriginalPos)
                {
                    handObject.transform.localPosition = handOriginalPos;
                }
            }
            else
            {
                if (handObject.transform.localPosition == handOriginalPos)
                {
                    handObject.transform.localPosition = new Vector3(handOriginalPos.x, -5.8f, handOriginalPos.z);
                }
            }
        }

        RaycastHit hit = CastRay(true);

        

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("drag") && hit.collider.transform.parent.tag == handObject.tag)
            {
                if(hoveredCard == null || hoveredCard != hit.collider.gameObject)
                {
                    //first reset old card
                    if (hoveredCard != null)
                    {
                        ResetCard();
                    }

                    siblingIndex = hit.collider.transform.GetSiblingIndex();

                    //then assign new card
                    hoveredCard = cards[siblingIndex].transform.Find("CardDisplay").gameObject;

                    //then move new card
                    hoveredCard.transform.localPosition = new Vector3(
                        hoveredCard.transform.localPosition.x, 
                        hoveredCard.transform.localPosition.y + 2f, 
                        hoveredCard.transform.localPosition.z - 2f);
                }
            }
        }
        else
        {
            if(hoveredCard != null)
            {
                ResetCard();
                hoveredCard = null;
            }
        }
    }

    private void SortCards()
    {
        cards.Clear();

        cardAmount = handObject.transform.childCount;

        for (int i = 0; i < cardAmount; i++)
        {
            cards.Add(handObject.transform.GetChild(i).gameObject);
        }

        middleCard = (cardAmount + 1) / 2 - 1;

        if (cardAmount % 2 != 0)
        {
            cards[middleCard].transform.localPosition = new Vector3(0f, 0f, 0f);

            if (middleCard != 0)
            {
                counter = 0;
                for (int i = middleCard + 1; i < cards.Count; i++)
                {
                    counter += 1;
                    MoveCards(i, counter * distanceBetweenCards, counter * 0.01f);
                }

                counter = 0;
                for (int i = middleCard - 1; i >= 0; i--)
                {
                    counter += 1;
                    MoveCards(i, counter * -distanceBetweenCards, counter * -0.01f);
                }
            }
        }
        else
        {
            counter = 0;
            for (int i = middleCard + 1; i < cards.Count; i++)
            {
                if (counter == 0)
                {
                    MoveCards(i, distanceBetweenCards / 2f, +0.01f);
                }
                else
                {
                    MoveCards(i, ((counter * distanceBetweenCards) + distanceBetweenCards / 2f), ((counter + 1f) * 0.01f));
                }

                counter += 1;
            }

            counter = 0;
            for (int i = middleCard; i >= 0; i--)
            {
                if (counter == 0)
                {
                    MoveCards(i, -distanceBetweenCards / 2f, -0.01f);
                }
                else
                {
                    MoveCards(i, ((counter * -distanceBetweenCards) - distanceBetweenCards / 2f), ((counter + 1f) * -0.01f));
                }

                counter += 1;
            }
        }
    }

    private void MoveCards(int i, float xPos, float yPos)
    {
        cards[i].transform.localPosition = new Vector3(xPos, yPos, 0f);
    }

    private void ResetCard()
    {
        hoveredCard.transform.localPosition = hoveredCardOriginalPos;
    }

    private RaycastHit CastRay(bool onCards)
    {
        Vector3 screenMousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);

        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);

        RaycastHit hit;

        if(onCards)
        {
            Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit, 50, cardLayer);
        }
        else
        {
            Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit, 50, handLayer);
        }


        return hit;
            
    }
}
