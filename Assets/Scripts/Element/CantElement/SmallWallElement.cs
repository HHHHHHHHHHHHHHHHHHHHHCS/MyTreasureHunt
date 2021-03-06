﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallWallElement : CantCoveredElement
{
    public override void OnInit()
    {
        base.OnInit();
        ElementContent = ElementContent.SmallWall;
        ClearShadow();
        LoadSprite(MainGameManager.Instance.SmallWallSprites[
            Random.Range(0, MainGameManager.Instance.SmallWallSprites.Length)]);
    }
}
