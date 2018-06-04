using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ToolType
{//和ElementContent 对应
    Hp,
    Armor,
    Sword,
    Map,
    Arrow,
    Key,
    Tnt,
    Hoe,
    Grass,
}

public class ToolElement : DoubleCoverElement
{
    private ToolType toolType;


    public override void OnInit()
    {
        base.OnInit();
        ElementContent = ElementContent.Tool;

        toolType = (ToolType)UnityEngine.Random.Range(0
            , Enum.GetValues(typeof(ToolType)).Length);

    }

    public override void ConfirmSprite()
    {
        LoadSprite(MainGameManager.Instance.ToolSprites[(int)toolType]);
    }
}
