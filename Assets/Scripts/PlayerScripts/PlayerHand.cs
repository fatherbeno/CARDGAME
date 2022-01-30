using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerHand : NetworkBehaviour
{
    [SerializeField] private GameObject enemyCardPrefab;

    //player core used to inherit important data
    [HideInInspector] public PlayerCore playerCore;

    //card lists
    private List<GameObject> cards = new List<GameObject>();
    private List<GameObject> playerCards = new List<GameObject>();
    private List<GameObject> enemyCards = new List<GameObject>();

    //objects
    private GameObject hitCard = null;
    private GameObject hoveredCard = null;
    private GameObject playerHandObject = null;
    private GameObject enemyHandObject = null;

    public float distanceBetweenCards = 1.75f;
    // Update is called once per frame
    void Update()
    {
        if(playerHandObject == null)
        {
            playerHandObject = playerCore.playerClient.transform.Find("Hand").gameObject;
        }
        else if(enemyHandObject == null)
        {
            enemyHandObject = playerCore.enemyClient.transform.Find("Hand").gameObject;
        }
        else if(playerCore.playerCamera == null)
        {
            return;
        }
        else
        {
            if(!isLocalPlayer)
            {
                return;
            }
            else
            {

                if (playerCore.isHoveringCard == true && hoveredCard == null)
                {
                    playerCore.isHoveringCard = false;
                    playerCore.TriggerHoverEnemyCard("PlayerHand: L55");
                }

                if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                    if (playerCore.cardCountInHand < playerCore.maxHandSize)
                    {
                        playerCore.gameManager.cardDeck.DrawCard(this);

                        SortCards(true);
                    }
                }

                //SORT CARDS WHEN CARDS IN HAND != CARD AMOUNT
                {
                    //detect when the amount of cards actually in hand do not match our records: aka cardAmount
                    //and sort them all to look all organised while de-highlighting a card if we are doing so
                    if (playerCore.cardCountInHand != playerHandObject.transform.childCount)
                    {
                        if (hoveredCard != null)
                        {
                            PlayerResetCard();
                            hoveredCard = null;
                        }

                        SortCards(true);
                    }

                    //now do the same for the enemy hand
                    if(playerCore.enemyCardCount != enemyHandObject.transform.childCount)
                    {
                        //SortCards(false);
                    }
                }

                //CARD HOVER HIGLIGHTING
                {
                    RaycastHit hit = CastRay(); //cast a ray detecting if hovering over a card

                    if (hit.collider != null) //check if ray hits anything
                    {
                        if (hit.collider.CompareTag("drag") && hit.collider.transform.parent.tag == playerHandObject.tag) //check if collided object is a card we can highlight
                        {
                            if (hoveredCard == null || hitCard != hit.collider.gameObject) //check if not highlighting card or if new hit card is not the same as the old/current hit card
                            {
                                if (hoveredCard != null) //first reset old card if we're hitting a new card immedietly after hitting another one
                                {
                                    PlayerResetCard();
                                }

                                playerCore.isHoveringCard = true; //tell PlayerCore we're hovering over a card

                                playerCore.hoveredCardIndex = hit.collider.transform.GetSiblingIndex(); //get the index of the hit card
                                if(hitCard != null)
                                {
                                    playerCore.oldHoveredCardIndex = hitCard.transform.GetSiblingIndex();
                                }

                                playerCore.TriggerHoverEnemyCard("PlayerHand: L113");

                                hoveredCard = playerCards[playerCore.hoveredCardIndex].transform.Find("CardDisplay").gameObject; //get the display of the hit card

                                hoveredCard.transform.localPosition = new Vector3( //then move CardDisplay to look like we're making the whole card bigger, while leaving the hitbox alone
                                    hoveredCard.transform.localPosition.x,
                                    hoveredCard.transform.localPosition.y + 2f,
                                    hoveredCard.transform.localPosition.z - 6f);

                                hitCard = hit.collider.gameObject;
                            }
                        }

                        if (hit.collider.CompareTag("moving") && playerCore.isHoveringCard == true) //stop card highlighting if we're moving the highlighted card
                        {
                            PlayerResetCard();
                        }
                    }
                    else
                    {
                        if (hoveredCard != null) //if we're not hovering over a card make sure none are being highlighted
                        {
                            PlayerResetCard();
                            hoveredCard = null;
                            playerCore.hoveredCardIndex = -1;
                        }
                    }
                }
            }
        }
    }

    private void SortCards(bool isPlayerHand)
    {
        Debug.Log("IS SORTING ENEMY?: " + !isPlayerHand);

        int middleCard;
        int counter;
        GameObject handObject;

        if(isPlayerHand)
        {
            playerCards.Clear();
            handObject = playerHandObject;
            playerCore.cardCountInHand = handObject.transform.childCount;
        }
        else
        {
            handObject = enemyHandObject;
        }

        cards.Clear();

        GameObject newCard;
        int cardCount = handObject.transform.childCount;

        for (int i = 0; i < cardCount; i++)
        {
            newCard = handObject.transform.GetChild(i).gameObject;

            if (isPlayerHand)
            {
                playerCards.Add(newCard);
            }
                
            cards.Add(newCard);
        }

        middleCard = (cardCount + 1) / 2 - 1;

        if (cardCount % 2 != 0)
        {
            cards[middleCard].transform.localPosition = new Vector3(0f, 0f, 0f);

            if (middleCard != 0)
            {
                counter = 0;
                for (int i = middleCard + 1; i < cards.Count; i++)
                {
                    counter += 1;
                    MoveCards(i, counter * distanceBetweenCards, counter * -0.01f);
                }

                counter = 0;
                for (int i = middleCard - 1; i >= 0; i--)
                {
                    counter += 1;
                    MoveCards(i, counter * -distanceBetweenCards, counter * 0.01f);
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
                    MoveCards(i, distanceBetweenCards / 2f, -0.01f);
                }
                else
                {
                    MoveCards(i, ((counter * distanceBetweenCards) + distanceBetweenCards / 2f), ((counter + 1f) * -0.01f));
                }

                counter += 1;
            }

            counter = 0;
            for (int i = middleCard; i >= 0; i--)
            {
                if (counter == 0)
                {
                    MoveCards(i, -distanceBetweenCards / 2f, +0.01f);
                }
                else
                {
                    MoveCards(i, ((counter * -distanceBetweenCards) - distanceBetweenCards / 2f), ((counter + 1f) * 0.01f));
                }

                counter += 1;
            }
        }
    }

    private void MoveCards(int i, float xPos, float yPos)
    {
        cards[i].transform.localPosition = new Vector3(xPos, yPos, 0f);
    }

    private void PlayerResetCard()
    {
        hoveredCard.transform.localPosition = new Vector3(0f, 0f, 0f);

        playerCore.isHoveringCard = true;
    }

    private RaycastHit CastRay()
    {
        Vector3 screenMousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, playerCore.playerCamera.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, playerCore.playerCamera.nearClipPlane);

        Vector3 worldMousePosFar = playerCore.playerCamera.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = playerCore.playerCamera.ScreenToWorldPoint(screenMousePosNear);

        RaycastHit hit;

        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit, 50, playerCore.cardLayer);
        
        return hit;
    }

    public void AddCardToHand(GameObject newCard)
    {
        Instantiate(newCard, playerHandObject.transform.position, playerHandObject.transform.rotation, playerHandObject.transform);
        playerCore.UpdateEnemyCardCount(-1);
    }

    public void AddEnemyCard()
    {
        Instantiate(enemyCardPrefab, enemyHandObject.transform.position, enemyHandObject.transform.rotation, enemyHandObject.transform);
        SetEnemyCardList();
        SortCards(false);
    }

    public void RemoveEnemyCard(int cardIndex)
    {
        if (cardIndex < 0 || cardIndex >= enemyHandObject.transform.childCount)
            cardIndex = 0;

        GameObject removedCard = enemyHandObject.transform.GetChild(cardIndex).gameObject;
        removedCard.transform.parent = playerCore.enemyCardDeath.transform;
        removedCard.transform.localPosition = new Vector3(0f, 0f, 0f);

        SetEnemyCardList();
        SortCards(false);
    }

    private void SetEnemyCardList()
    {
        enemyCards.Clear();

        for (int i = 0; i < enemyHandObject.transform.childCount; i++)
        {
            enemyCards.Add(enemyHandObject.transform.GetChild(i).transform.Find("CardDisplay").gameObject);
        }
    }

    public void HoverEnemyCard(int cardIndex, int oldCardIndex, bool isHovering, string debugString)
    {
        //Debug.Log("CI: " + cardIndex + ", OCI: " + oldCardIndex + ", IH: " + isHovering + ". " + debugString);

        if (isHovering)
        {
            if(oldCardIndex >= 0)
            {
                enemyCards[oldCardIndex].transform.localPosition = new Vector3(0f, 0f, 0f);
            }

            if(cardIndex >= 0)
            {
                enemyCards[cardIndex].transform.localPosition = new Vector3(0f, 0f, -2f);
            }
        }
        else
        {
            foreach (GameObject item in enemyCards)
            {
                if (item != null && item.transform.localPosition != new Vector3(0f, 0f, 0f))
                    item.transform.localPosition = new Vector3(0f, 0f, 0f);
            }
        }
    }
}