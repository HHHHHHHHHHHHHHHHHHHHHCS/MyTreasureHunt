using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorElement : CantCoveredElement
{
    public override void OnInit()
    {
        base.OnInit();
        ElementContent = ElementContent.Door;
        LoadSprite(MainGameManager.Instance.DoorWallSprite);
    }
}
