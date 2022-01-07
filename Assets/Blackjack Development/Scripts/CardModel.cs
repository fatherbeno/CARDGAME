using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardModel : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Sprite[] faces;
    [SerializeField]
    private Sprite cardBack;

    public int cardIndex;
    [HideInInspector]
    public int cardIndexLast = 0;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        cardIndexLast = faces.Length - 1;
    }

    public void ToggleFace(bool showFace)
    {
        if (showFace)
        {
            spriteRenderer.sprite = faces[cardIndex];
        }
        else
        {
            spriteRenderer.sprite = cardBack;
        }
    }

    public Sprite GetFace(int index)
    {
        return faces[index];
    }

    public Sprite GetCardBack()
    {
        return cardBack;
    }
}
