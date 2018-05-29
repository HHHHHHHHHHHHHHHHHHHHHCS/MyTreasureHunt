using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleCoverElement : BaseElement
{
    protected override void Awake()
    {
        base.Awake();
        //设置自己的初始类型
        elementType = ElementType.SingleCovered;
        //随机加载一张图片
        LoadSprite(MainGameManager.Instance.CoverTiledSprites
            [Random.Range(0, MainGameManager.Instance.CoverTiledSprites.Length)]);
    }

    public void UncoveredElement()
    {
        if (elementState == ElementState.UnCovered)
            return;
        UncoveredElementSingle();
        OnUncovered();
    }

    public void UncoveredElementSingle()
    {

    }

    public void OnUncovered()
    {

    }

    public void AddCoverElement()
    {

    }

    public void AddFlag()
    {
        elementState = ElementState.Marked;
    }

    public void RemoveFlag()
    {
        elementState = ElementState.Covered;
    }
}
