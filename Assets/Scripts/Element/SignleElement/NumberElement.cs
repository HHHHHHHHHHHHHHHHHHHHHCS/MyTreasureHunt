using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberElement : SingleCoverElement
{
    public bool NeedEffect { get; set; }

    public override void OnInit()
    {
        base.OnInit();
        NeedEffect = true;
        ElementState = ElementState.Covered;
        ElementContent = ElementContent.Number;
    }

    protected override void OnMiddleMouseButton()
    {
        if((int)MainGameManager.Instance.Player.transform.position.x==x
            && (int)MainGameManager.Instance.Player.transform.position.y == y)
        {
            if (ElementState == ElementState.UnCovered)
            {
                MainGameManager.Instance.UncoveredAdjacentElements(x, y);
            }
            MainGameManager.Instance.Anim.SetTrigger("Quick");
        }
    }

    public override void UncoveredElementSingle()
    {
        if (ElementState == ElementState.UnCovered)
            return;
        RemoveFlag();
        ElementState = ElementState.UnCovered;
        ClearShadow();
        if(NeedEffect)
        {
            Instantiate(MainGameManager.Instance.UncoveredEffect, transform);
        }
        LoadSprite(MainGameManager.Instance.GetNumberSpriteByPos(x,y));
    }

    public override void OnUncovered()
    {
        MainGameManager.Instance.FloodFillElement(x, y);
    }
}
