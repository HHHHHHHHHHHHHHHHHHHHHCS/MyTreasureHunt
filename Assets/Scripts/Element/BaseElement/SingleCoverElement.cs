using System.Collections;
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
        ElementState = ElementState.Covered;
        ElementType = ElementType.SingleCovered;
        //随机加载一张图片
        LoadSprite(MainGameManager.Instance.CoverTiledSprites
            [Random.Range(0, MainGameManager.Instance.CoverTiledSprites.Length)]);
    }

    /// <summary>
    /// 整体翻开 翻开表现和事件
    /// </summary>
    public virtual void UncoveredElement()
    {
        if (ElementState == ElementState.UnCovered)
            return;
        UncoveredElementSingle();
        OnUncovered();
    }

    /// <summary>
    /// 单纯的翻开表现
    /// </summary>
    public virtual void UncoveredElementSingle()
    {

    }

    /// <summary>
    /// 翻开事件
    /// </summary>
    public virtual void OnUncovered()
    {

    }

    public virtual void AddCoverElement()
    {

    }

    public virtual void AddFlag()
    {
        AudioManager.Instance.PlayClip(AudioManager.Instance.flag);
        ElementState = ElementState.Marked;
        flag = Instantiate(MainGameManager.Instance.FlagElement, transform);
        flag.transform.localPosition = Vector3.zero;
        flag.transform.localScale = Vector3.one*1.25f;
        flag.transform.DOScale(Vector3.one, 0.15f);
        PoolManager.Instance.GetInstance(EffectType.SmokeEffect,transform);
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

    public override void OnRightMouseButton()
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


}
