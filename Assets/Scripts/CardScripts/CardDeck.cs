using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CardDeck : NetworkBehaviour
{
    public GameObject CardTemplate;
    public CardData[] CardsInDeck;

    public void DrawCard(PlayerHand playerHand)
    {
        GameObject Card = CardTemplate;
        Card.GetComponent<CardCore>().CardData = CardsInDeck[1];

        playerHand.AddCardToHand(Card);
    }
}
