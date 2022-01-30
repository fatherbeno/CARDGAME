using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerCore : NetworkBehaviour
{


    // --- ASSIGNED DURING RUNTIME --- //
    [Header("Component References")]
    [HideInInspector] public GameManager gameManager;
    [SerializeField] private PlayerHand playerHand;
    [SerializeField] private PlayerMoveCard playerMoveCard;
    [HideInInspector] public Camera playerCamera;
    public NetworkIdentity networkIdentity;

    // --- DATA BELOW INHERITED FROM GAMEMANAGER --- //
    //Player hand settings
    [HideInInspector] public int maxHandSize;

    //Raycast layer masks
    [HideInInspector] public LayerMask cardLayer;
    [HideInInspector] public LayerMask cardDestinationLayer;
    [HideInInspector] public LayerMask playerHandLayer;

    //Scene objects
    [HideInInspector] public GameObject playerClient;
    [HideInInspector] public GameObject playerCardDestinations;
    [HideInInspector] public GameObject enemyClient;
    [HideInInspector] public GameObject enemyCardDestinations;
    [HideInInspector] public GameObject enemyCardDeath;

    //Player deck
    [HideInInspector] public CardDeck cardDeck;

    // --- DATA BELOW UPDATED DURING RUNTIME --- //
    //Player hand
    [HideInInspector] public bool isHoveringCard = false;
    [HideInInspector] public int hoveredCardIndex = -1;
    [HideInInspector] public int oldHoveredCardIndex = -1;
    [HideInInspector] public int cardCountInHand = 0;
    [HideInInspector] public int cardCountInHandOld = 0;
    [HideInInspector] public int enemyCardCount = 0;

    //Enemy player
    [HideInInspector] public PlayerCore enemyPlayer;

    // --- DATA BELOW USED FOR PLAYERCORE FUNCTIONALITY --- //
    private bool isPlayerConnected = false;
    private bool playerAddedToManager = false;

    private void Update()
    {
        /*if (cardCountInHandOld != cardCountInHand)
        {
            cardCountInHandOld = cardCountInHand;
        }*/

        if (gameManager != null)
        {
            if (isPlayerConnected && !playerAddedToManager && GameObject.FindGameObjectsWithTag("Player").Length == 2)
            {
                playerAddedToManager = true;
                SendPlayersToServer(this);
            }

            return;
        }
        else
        {
            //Get GameManager component from GameManager object
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            playerCamera = Camera.main;

            //Assign data from GameManager to PlayerCore
            //Hand
            maxHandSize = gameManager.maxHandSize;

            //Layer Masks
            cardLayer = gameManager.card;
            cardDestinationLayer = gameManager.cardDestination;
            playerHandLayer = gameManager.playerHand;

            //Scene objects
            playerClient = gameManager.playerClient;
            playerCardDestinations = gameManager.playerCardDestinations;
            enemyClient = gameManager.enemyPlayer;
            enemyCardDestinations = gameManager.enemyCardDestinations;
            enemyCardDeath = gameManager.enemyCardDeath;

            //Card Deck
            cardDeck = gameManager.cardDeck;

            //Assign playercore to other player scripts
            playerHand.playerCore = this;
            playerMoveCard.playerCore = this;
        }
    }

    public override void OnStartClient()
    {
        isPlayerConnected = true;
    }



    // --- PLAYER LIST SYNCLIST FUNCTIONS --- //

    [ClientRpc]
    public void RpcSetEnemyPlayer()
    {
        foreach (PlayerCore item in gameManager.players)
        {
            if (item != this)
            {
                enemyPlayer = item;
            }
        }
    }

    [Command(requiresAuthority = false)]
    public void SendPlayersToServer(PlayerCore player)
    {
        if (!gameManager.players.Contains(player))
        {
            gameManager.players.Add(player);
        }
        else
        {
            RpcSetEnemyPlayer();
        }
    }



    // --- PLAYER HAND FUNCTIONS --- //
    public void UpdateEnemyCardCount(int cardIndex)
    {
        GameObject playerHand = playerClient.transform.Find("Hand").gameObject;

        if(cardCountInHand != playerHand.transform.childCount)
        {
            cardCountInHandOld = cardCountInHand;
            //cardCountInHand = playerHand.transform.childCount; // <--------
        }

        CmdUpdateEnemyCardCount(playerHand.transform.childCount, cardIndex);
    }


    [Command(requiresAuthority = false)]
    private void CmdUpdateEnemyCardCount(int cardCount, int cardIndex)
    {
        TargetUpdateEnemyCardCount(enemyPlayer.networkIdentity.connectionToClient, cardCount, cardIndex);
    }

    [TargetRpc]
    private void TargetUpdateEnemyCardCount(NetworkConnection conn, int cardCount, int cardIndex)
    {
        int oldEnemyCardCount = enemyCardCount;
        enemyCardCount = cardCount;

        if (cardCount > oldEnemyCardCount)
            playerHand.AddEnemyCard();

        if (cardCount < oldEnemyCardCount)
            playerHand.RemoveEnemyCard(cardIndex);
    }

    [Command(requiresAuthority = false)]
    private void CmdHoverEnemyCard(int cardIndex, int oldCardIndex, bool isHovering, string debugString)
    {
        TargetHoverEnemyCard(enemyPlayer.networkIdentity.connectionToClient, cardIndex, oldCardIndex, isHovering, debugString);
    }

    [TargetRpc]
    private void TargetHoverEnemyCard(NetworkConnection conn, int cardIndex, int oldCardIndex, bool isHovering, string debugString)
    {
        playerHand.HoverEnemyCard(cardIndex, oldCardIndex, isHovering, debugString);
    }

    public void TriggerHoverEnemyCard(string debugString)
    {
        CmdHoverEnemyCard(hoveredCardIndex, oldHoveredCardIndex, isHoveringCard, debugString);
    }


    // --- PLAYER MOVE CARD FUNCTIONS --- //
    public void CoreMoveEnemyCard(int cardIndex, float cardPositionX, float cardPositionZ)
    {
        CmdMoveEnemyCard(cardIndex, cardPositionX, cardPositionZ);
    }

    [Command(requiresAuthority = false)]
    private void CmdMoveEnemyCard(int cardIndex, float cardPositionX, float cardPositionZ)
    {
        TargetMoveEnemyCard(enemyPlayer.networkIdentity.connectionToClient, cardIndex, cardPositionX, cardPositionZ);
    }

    [TargetRpc]
    private void TargetMoveEnemyCard(NetworkConnection conn, int cardIndex, float cardPositionX, float cardPositionZ)
    {
        playerMoveCard.MoveEnemyCard(cardIndex, cardPositionX, cardPositionZ);
    }
}
