using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardModelOLD : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    [SerializeField]
    private Sprite[] faces;
    [SerializeField]
    private Sprite cardBack;

    [SerializeField]
    private CardInformationOLD card;
    [HideInInspector]
    public int cardId;
    [HideInInspector]
    public int facesLastIndex;

    public void ToggleFace(bool showFace)
    {
        if (showFace)
        {
            spriteRenderer.sprite = faces[cardId];
        }
        else
        {
            spriteRenderer.sprite = cardBack;
        }
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        facesLastIndex = faces.Length - 1;

        if (card != null)
        {
            cardId = card.id;
        }
        else
        {
            Debug.LogError("Card Model: Card information has not been supplied for " + this.name);
        }
    }
}
