using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorElement : CantCoveredElement
{
    public override void OnInit()
    {
        base.OnInit();
        ElementContent = ElementContent.Door;
        LoadSprite(MainGameManager.Instance.DoorWallSprite);
    }

    protected override void OnLeftMouseButton()
    {
        if(Vector3.Distance(transform.position,MainGameManager.Instance.Player.transform.position)<1.5f)
        {
            if(MainGameManager.Instance.Key>0)
            {
                AudioManager.Instance.PlayClip(AudioManager.Instance.door);
                MainGameManager.Instance.Key--;
                Instantiate(MainGameManager.Instance.OpenDoorEffect,transform);
                ToNumberElement(true);
            }
            else
            {
                base.OnLeftMouseButton();
            }
        }
        else
        {
            base.OnLeftMouseButton();
        }
    }
}
