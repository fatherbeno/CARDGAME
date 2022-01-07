using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugChangeCardOLD : MonoBehaviour
{
    private CardModelOLD cardModel;
    private int cardId = 0;

    [SerializeField]
    private GameObject card;

    // Start is called before the first frame update
    void Awake()
    {
        cardModel = card.GetComponent<CardModelOLD>();
    }

    private void OnGUI()
    {
        if(GUI.Button(new Rect(10, 10, 100, 28), "HIT ME!"))
        {
            cardModel.cardId = cardId;

            
            if(cardId > cardModel.facesLastIndex)
            {
                cardId = 0;
                cardModel.ToggleFace(false);
            }
            else
            {

                cardModel.ToggleFace(true);
                cardId++;
            }
        }
    }
}
