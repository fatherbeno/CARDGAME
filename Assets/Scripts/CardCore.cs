using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCore : MonoBehaviour
{
    [SerializeField]
    private CardData cardData;

    [HideInInspector]
    public int cardId;

    [HideInInspector]
    public string cardName;
    [HideInInspector]
    public Texture2D cardArt;
    [HideInInspector]
    public string cardDescription;

    [HideInInspector]
    public int cardTier;
    [HideInInspector]
    public int cardCost;
    [HideInInspector]
    public int cardHealth;
    [HideInInspector]
    public int cardAttack;
    [HideInInspector]
    public int cardArmour;

    [HideInInspector]
    public CardData.CardType cardType;
    [HideInInspector]
    public CardData.BodyPartType bodyPartType;
    [HideInInspector]
    public CardData.AccessoryType accessoryType;

    private void Awake()
    {
        if(cardData != null)
        {
            cardId = cardData.cardId;

            cardName = cardData.cardName;
            cardArt = cardData.cardArt;
            cardDescription = cardData.cardDescription;

            cardTier = cardData.cardTier;
            cardCost = cardData.cardCost;
            cardHealth = cardData.cardHealth;
            cardAttack = cardData.cardAttack;
            cardArmour = cardData.cardArmour;

            cardType = cardData.cardType;
            bodyPartType = cardData.bodyPartType;
            accessoryType = cardData.accessoryType;

            //cardData.Debugging();
        }
    }
}
