﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapElement : SingleCoverElement
{
    public override void OnInit()
    {
        base.OnInit();
        ElementContent = ElementContent.Trap;
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
        LoadSprite(MainGameManager.Instance.TrapSprites
            [Random.Range(0, MainGameManager.Instance.TrapSprites.Length)]);
        MainGameManager.Instance.TakeDamage();
    }

    public override void OnUncovered()
    {
        
    }
}
