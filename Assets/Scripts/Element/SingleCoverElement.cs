﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SingleCoverElement : BaseElement
{
    private GameObject flag;

    public override void OnInit()
    {
        base.OnInit();
        //设置自己的初始类型
        ElementType = ElementType.SingleCovered;
        //随机加载一张图片
        LoadSprite(MainGameManager.Instance.CoverTiledSprites
            [Random.Range(0, MainGameManager.Instance.CoverTiledSprites.Length)]);
    }

    public virtual void UncoveredElement()
    {
        if (ElementState == ElementState.UnCovered)
            return;
        UncoveredElementSingle();
        OnUncovered();
    }

    public virtual void UncoveredElementSingle()
    {

    }

    public virtual void OnUncovered()
    {

    }

    public virtual void AddCoverElement()
    {

    }

    public virtual void AddFlag()
    {
        ElementState = ElementState.Marked;
        flag = Instantiate(MainGameManager.Instance.FlagElement, transform);
        flag.transform.localPosition = Vector3.zero;
        flag.transform.localScale = Vector3.one*1.25f;
        flag.transform.DOScale(Vector3.one, 0.15f);
        Instantiate(MainGameManager.Instance.SmokeEffect,transform);
    }

    public virtual void RemoveFlag()
    {
        if (flag)
        {
            ElementState = ElementState.Covered;
            flag.transform.DOLocalMoveY(0.15f, 0.1f)
                .onComplete += () => { Destroy(flag); flag = null; };
        }
    }

    protected override void OnRightMouseButton()
    {
        switch (ElementState)
        {
            case ElementState.Covered:
                AddFlag();
                break;
            case ElementState.UnCovered:
                return;
            case ElementState.Marked:
                RemoveFlag();
                break;
        }
    }

    public override void OnPlayerStand()
    {
        switch (ElementState)
        {
            case ElementState.Covered:
                UncoveredElement();
                break;
            case ElementState.UnCovered:
                break;
            case ElementState.Marked:
                RemoveFlag();
                break;
            default:
                break;
        }
    }

    public void ClearShaodow()
    {
        Transform shadow = transform.Find("Shadow");
        if(shadow!=null)
        {
            Destroy(shadow.gameObject);
        }
    }
}