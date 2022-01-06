using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCore : MonoBehaviour
{
    //This script is to hold any information and supply any functions that multiple card scripts require

    [SerializeField]
    private CardInformation card;

    [HideInInspector]
    public int cardID;
    [HideInInspector]
    public string cardName;
    [HideInInspector]
    public string cardDescription;
    [HideInInspector]
    public Sprite cardArt;
    [HideInInspector]
    public Sprite cardBackArt;

    private void Awake()
    {
        if(card != null)
        {
            Debug.Log("Card Core: Card information read successfully for " + this.name);

            cardID = card.id;
            cardName = card.name.ToString();
            cardDescription = card.description.ToString();
            cardArt = card.artwork;
            cardBackArt = card.cardBack;
        }
        else
        {
            Debug.LogError("Card Core: Card information could not be read for " + this.name);
        }
        
    }
}
