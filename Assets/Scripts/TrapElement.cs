using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapElement : SingleCoverElement
{
    protected override void Awake()
    {
        base.Awake();
        elementState = ElementState.Covered;
        elementContent = ElementContent.Trap;
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
        LoadSprite(MainGameManager.Instance.TrapTiledSprites
            [Random.Range(0, MainGameManager.Instance.TrapTiledSprites.Length)]);

    }

    public override void OnUncovered()
    {
        
    }
}
