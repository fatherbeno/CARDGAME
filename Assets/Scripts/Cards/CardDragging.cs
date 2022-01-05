using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragging : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector2 cursorDisplacement;
    private Vector3 cursorPosition;

    //OnBeginDrag is called when the drag event begins
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Use for functionality during the start of the drag event. EG: Displacement calculations

        Cursor.visible = false;

        cursorPosition = eventData.position; //Make Vector2 into Vector3
        cursorDisplacement = this.transform.position - cursorPosition; //Set cursorDisplacement as the difference between this origin point and the cursor position 
    }

    //OnDrag is called while the drag event is happening
    public void OnDrag(PointerEventData eventData)
    {
        //Use for constant functionality while being dragged. EG: Updating object position

        this.transform.position = eventData.position + cursorDisplacement; //Set position of this equal to the new eventData + cursorDisplacement
    }

    //OnEndDrag is called when the drag event ends
    public void OnEndDrag(PointerEventData eventData)
    {
        //Use for functionality during the end of the drag event. EG: Check if the object can be dragged to the end location

        Cursor.visible = true;
    }
}
