using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleCoverElement : SingleCoverElement
{
    public bool isHide = true;

    public override void OnInit()
    {
        base.OnInit();
        ElementType = ElementType.DoubleCovered;
        if (Random.value < MainGameManager.Instance.UncoverProbability)
        {
            UncoveredElementSingle();
        }
    }

    public override void OnPlayerStand()
    {
        switch (ElementState)
        {
            case ElementState.Covered:
                if (isHide)
                {
                    UncoveredElementSingle();
                }
                else
                {
                    UncoveredElement();
                }
                break;
            case ElementState.UnCovered:
                break;
            case ElementState.Marked:
                if (isHide)
                {
                    RemoveFlag();
                }
                break;
            default:
                break;
        }
    }

    //protected override void OnMiddleMouseButton()
    //{
    //    if(isHide)
    //    {
    //        MainGameManager.Instance.UncoveredAdjacentElements(x, y);
    //    }
    //}

    protected override void OnRightMouseButton()
    {
        switch (ElementState)
        {
            case ElementState.Covered:
                if (isHide)
                {
                    AddFlag();
                }
                break;
            case ElementState.UnCovered:
                break;
            case ElementState.Marked:
                if (isHide)
                {
                    RemoveFlag();
                }
                break;
            default:
                break;
        }
    }

    public override void UncoveredElementSingle()
    {
        if (ElementState == ElementState.UnCovered) return;
        isHide = false;
        RemoveFlag();
        ClearShaodow();
        ConfirmSprite();
    }

    public override void OnUncovered()
    {
        ElementState = ElementState.UnCovered;
        ToNumberElement();
    }

    public virtual void ConfirmSprite()
    {

    }

}
