using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GoldType
{
    One,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven
}


public class GoldElement : DoubleCoverElement
{
    private GoldType goldType;

    public override void OnInit()
    {
        base.OnInit();
        ElementContent = ElementContent.Gold;

        //goldType = (GoldType)UnityEngine.Random.Range(0
        //    , Enum.GetValues(typeof(GoldType)).Length);
        goldType = (GoldType)UnityEngine.Random.Range(0, 7);

        if (!isHide)
        {
            ConfirmSprite();
        }
    }

    public override void OnUncovered()
    {
        Transform gold = transform.Find("GoldEffect");
        if (gold != null)
        {
            Destroy(gold.gameObject);
        }
        GetGold();
        base.OnUncovered();
    }

    private void GetGold()
    {
        int x = MainGameManager.Instance.IsGrass ? 2 : 1;
        MainGameManager.Instance.Gold += x * (int)goldType * 30;
        UIManager.Instance.OnUpdateUI();
    }

    public override void ConfirmSprite()
    {
        Transform gold = transform.Find("GoldEffect");
        if (gold != null)
        {
            Instantiate(MainGameManager.Instance.GoldEffect, transform).name = "GoldEffect";
        }
        LoadSprite(MainGameManager.Instance.GoldSprites[(int)goldType]);
    }
}
