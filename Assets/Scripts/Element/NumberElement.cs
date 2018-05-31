using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberElement : SingleCoverElement
{
    public override void OnInit()
    {
        base.OnInit();
        ElementState = ElementState.Covered;
        ElementContent = ElementContent.Number;
    }

    protected override void OnMiddleMouseButton()
    {
        base.OnMiddleMouseButton();
    }

    public override void UncoveredElementSingle()
    {
        if (ElementState == ElementState.UnCovered)
            return;
        RemoveFlag();
        ElementState = ElementState.UnCovered;
        ClearShaodow();
        Instantiate(MainGameManager.Instance.UncoveredEffect, transform);
        LoadSprite(MainGameManager.Instance.GetNumberSpriteByPos(x,y));
    }

    public override void OnUncovered()
    {
        MainGameManager.Instance.FloodFillElement(x, y);
    }
}
