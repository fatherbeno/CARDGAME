using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCore : MonoBehaviour
{
    //Card info
    [HideInInspector] public CardData CardData;

    //Card system info
    [HideInInspector] public int cardId;
    [HideInInspector] public int cardHandIndex = -1;
    private GameObject handParent;

    //Card display info
    [HideInInspector] public string cardName;
    [HideInInspector] public Texture2D cardArt;
    [HideInInspector] public string cardDescription;

    //Card gameplay info
    [HideInInspector] public int cardTier;
    [HideInInspector] public int cardCost;
    [HideInInspector] public int cardHealth;
    [HideInInspector] public int cardAttack;
    [HideInInspector] public int cardArmour;

    //Card type
    [HideInInspector] public CardData.CardType cardType;
    [HideInInspector] public CardData.BodyPartType bodyPartType;
    [HideInInspector] public CardData.AccessoryType accessoryType;

    private void Awake()
    {
        if(CardData != null)
        {
            cardId = CardData.cardId;

            cardName = CardData.cardName;
            cardArt = CardData.cardArt;
            cardDescription = CardData.cardDescription;

            cardTier = CardData.cardTier;
            cardCost = CardData.cardCost;
            cardHealth = CardData.cardHealth;
            cardAttack = CardData.cardAttack;
            cardArmour = CardData.cardArmour;

            cardType = CardData.cardType;
            bodyPartType = CardData.bodyPartType;
            accessoryType = CardData.accessoryType;
        }
    }

    private void Update()
    {
        if(handParent == null && this.transform.parent.CompareTag("hand"))
        {
            handParent = this.transform.parent.gameObject;
        }

        if (this.transform.parent == handParent.transform && handParent.CompareTag("hand"))
        {
            if (cardHandIndex != this.transform.GetSiblingIndex())
            {
                cardHandIndex = this.transform.GetSiblingIndex();
            }
        }
    }
}
