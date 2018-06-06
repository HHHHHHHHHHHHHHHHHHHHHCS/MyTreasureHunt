using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyElement : CantCoveredElement
{
    public override void OnInit()
    {
        base.OnInit();
        ElementContent = ElementContent.BigWall;
        ClearShadow();
        LoadSprite(MainGameManager.Instance.EnemyWallSprites[
            Random.Range(0, MainGameManager.Instance.EnemyWallSprites.Length)]);
    }
}
