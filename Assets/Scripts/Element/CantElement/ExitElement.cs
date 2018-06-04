using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitElement : CantCoveredElement
{
    public override void OnInit()
    {
        base.OnInit();
        ElementContent = ElementContent.Exit;
        ClearShadow();
        LoadSprite(MainGameManager.Instance.ExitSprite);
    }
}
