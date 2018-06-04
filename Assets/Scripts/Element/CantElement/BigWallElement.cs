using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigWallElement : CantCoveredElement
{
    public override void OnInit()
    {
        base.OnInit();
        ElementContent = ElementContent.BigWall;
        LoadSprite(MainGameManager.Instance.BigWallSprites[
            Random.Range(0, MainGameManager.Instance.BigWallSprites.Length)]);
    }
}
