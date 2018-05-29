using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementState
{
    Covered,
    UnCovered,
    Marked
}

public enum ElementType
{
    SingleCovered,
    DoubleCovered,
    CantCovered,
}

public enum ElementContent
{
    Number,
    Trap,
    Tool,
    Gold,
    Enemy,
    Door
}

public class BaseElement : MonoBehaviour
{
    private int x, y;
    private ElementState elementState;
    private ElementType elementType;
    private ElementContent elementContent;

    public virtual void Awake()
    {
        x = (int)transform.position.x;
        y = (int)transform.position.y;
        name = "(" + x + "," + y + ")";
    }

    public void LoadSprite(Sprite sprite)
    {
        gameObject.GetComponent<SpriteRenderer>()
            .sprite = sprite;
    }

    public virtual void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(2) && elementState == ElementState.UnCovered)
        {
            OnMiddleMouseButton();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            OnLeftMouseButton();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            OnRightMouseButton();
        }
    }

    public virtual void OnLeftMouseButton()
    {

    }

    public virtual void OnMiddleMouseButton()
    {

    }


    public virtual void OnRightMouseButton()
    {

    }

    public virtual void OnPlayerStand()
    {

    }
}
