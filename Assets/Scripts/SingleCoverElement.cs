using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleCoverElement : BaseElement
{
    protected override void Awake()
    {
        base.Awake();
        elementType = ElementType.SingleCovered;
        LoadSprite(MainGameManager.Instance.CoverTiledSprites
            [Random.Range(0, MainGameManager.Instance.CoverTiledSprites.Length)]);
    }

    public void AddCoverElement()
    {

    }

    public void AddFlag()
    {

    }

    public void RemoveFlag()
    {

    }
}
