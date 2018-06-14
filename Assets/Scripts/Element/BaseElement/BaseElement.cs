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
    Door,
    BigWall,
    SmallWall,
    Exit
}

public class BaseElement : MonoBehaviour
{
    protected int x, y;
    public ElementState ElementState { get; protected set; }
    public ElementType ElementType { get; protected set; }
    public ElementContent ElementContent { get; protected set; }

    public virtual void OnInit()
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
        if (Input.GetMouseButtonDown(2) && ElementState == ElementState.UnCovered)
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
        MainGameManager.Instance.FindPath(new AStarPoint(x, y));
    }

    protected virtual void OnMiddleMouseButton()
    {

    }


    protected virtual void OnRightMouseButton()
    {

    }

    public virtual void OnPlayerStand()
    {
    }

    public void ClearShadow()
    {
        Transform shadow = transform.Find("Shadow");
        if (shadow != null)
        {
            Destroy(shadow.gameObject);
        }
    }

    public virtual void ToNumberElement(bool needEffect = false)
    {
        var element = MainGameManager.Instance.SetElement<NumberElement>(x, y);
        element.NeedEffect = needEffect;
        element.UncoveredElement();
    }
}
