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

public enum WeaponType
{
    None,
    Arrow,
    Sword,
}


public class ToolElement : DoubleCoverElement
{
    private ToolType toolType;


    public override void OnInit()
    {
        base.OnInit();
        ElementContent = ElementContent.Tool;

        //toolType = (ToolType)UnityEngine.Random.Range(0
        //    , Enum.GetValues(typeof(ToolType)).Length);
        toolType = (ToolType)UnityEngine.Random.Range(0, 4);
        if (!isHide)
        {
            ConfirmSprite();
        }
    }

    public void ReOnInit(ToolType _toolType)
    {
        toolType = _toolType;
        if (!isHide)
        {
            ConfirmSprite();
        }
    }

    public override void OnUncovered()
    {
        GetTool();
        base.OnUncovered();
    }

    private void GetTool()
    {
        AudioManager.Instance.PlayClip(AudioManager.Instance.pick);
        MainGameManager manager = MainGameManager.Instance;
        switch (toolType)
        {
            case ToolType.Hp:
                manager.Hp++;
                break;
            case ToolType.Armor:
                manager.Armor++;
                break;
            case ToolType.Sword:
                manager.WeaponType = WeaponType.Sword;
                manager.Arrow = 0;
                break;
            case ToolType.Map:
                manager.Map++;
                break;
            case ToolType.Arrow:
                if(manager.WeaponType== WeaponType.None)
                {
                    manager.WeaponType = WeaponType.Arrow;
                    manager.Arrow++;
                }
                break;
            case ToolType.Key:
                manager.Key++;
                break;
            case ToolType.Tnt:
                manager.Tnt++;
                break;
            case ToolType.Hoe:
                manager.Hoe++;
                break;
            case ToolType.Grass:
                manager.IsGrass=true;
                break;
            default:
                break;
        }
    }

    public override void ConfirmSprite()
    {
        LoadSprite(MainGameManager.Instance.ToolSprites[(int)toolType]);
    }
}
