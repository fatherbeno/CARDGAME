using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(CardDeck))]
public class GameManager : NetworkBehaviour
{
    public readonly SyncList<PlayerCore> players = new SyncList<PlayerCore>();

    [Header("Hand")]
    public int maxHandSize = 10;

    [Header("Layer Masks")]
    public LayerMask card;
    public LayerMask cardDestination;
    public LayerMask playerHand;

    [Header("Scene Objects")]
    public GameObject playerClient;
    public GameObject playerCardDestinations;
    public GameObject enemyPlayer;
    public GameObject enemyCardDestinations;
    public GameObject enemyCardDeath;

    [Header("Card Deck")]
    public CardDeck cardDeck;
    public GameObject networkCard;
}
