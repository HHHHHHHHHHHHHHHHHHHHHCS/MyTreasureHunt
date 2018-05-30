using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberElement : SingleCoverElement
{
    protected override void Awake()
    {
        base.Awake();
        elementState = ElementState.Covered;
        elementContent = ElementContent.Number;
    }

    protected override void OnMiddleMouseButton()
    {
        base.OnMiddleMouseButton();
    }

    public override void UncoveredElementSingle()
    {
        if (elementState == ElementState.UnCovered)
            return;
        RemoveFlag();
        elementState = ElementState.UnCovered;
        ClearShaodow();
    }

    public override void OnUncovered()
    {

    }
}
