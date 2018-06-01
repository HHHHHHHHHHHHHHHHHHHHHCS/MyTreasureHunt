using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ToolType
{
    Hp,
    Armor,
    Sword,
    Arrow,
    Key,
    Tnt,
    Hoe,
    Grass,
    Map
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
