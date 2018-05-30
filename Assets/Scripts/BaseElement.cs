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
    protected int x, y;
    protected ElementState elementState;
    protected ElementType elementType;
    protected ElementContent elementContent;

    protected virtual void Awake()
    {
        x = (int)transform.position.x;
        y = (int)transform.position.y;
        name = "(" + x + "," + y + ")";
    }

    /// <summary>
    /// 切换当前元素的图片
    /// </summary>
    /// <param name="sprite"></param>
    protected void LoadSprite(Sprite sprite)
    {
        gameObject.GetComponent<SpriteRenderer>()
            .sprite = sprite;
    }

    protected virtual void OnMouseOver()
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

    protected virtual void OnLeftMouseButton()
    {
        OnPlayerStand();
    }

    protected virtual void OnMiddleMouseButton()
    {

    }


    protected virtual void OnRightMouseButton()
    {

    }

    protected virtual void OnPlayerStand()
    {
    }

}
