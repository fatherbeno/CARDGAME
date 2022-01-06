using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugFlipCard : MonoBehaviour
{
    private bool showFace = true;

    [SerializeField]
    private GameObject cardObject;

    private CardDisplay cardDisplay;

    private void Awake()
    {
        cardDisplay = cardObject.GetComponent<CardDisplay>();

        if (cardDisplay != null)
            Debug.Log("Debug Flip Card: Card Display loaded correctly");
        else
            Debug.LogError("Debug Flip Card: Card Display not loaded");
    }


    private void OnGUI()
    {
        if (GUI.Button(new Rect(120, 10, 100, 28), "HIT ME TOO!"))
        {
            showFace = !showFace;

            cardDisplay.FlipCard(showFace);
        }
    }
}