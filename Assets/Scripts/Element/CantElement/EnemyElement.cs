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

    protected override void OnLeftMouseButton()
    {
        if (Vector3.Distance(transform.position, MainGameManager.Instance.Player.transform.position) < 1.5f)
        {

            switch (MainGameManager.Instance.WeaponType)
            {
                case WeaponType.None:
                    base.OnLeftMouseButton();
                    break;
                case WeaponType.Arrow:
                    AudioManager.Instance.PlayClip(AudioManager.Instance.enemy);
                    MainGameManager.Instance.Arrow--;
                    if(MainGameManager.Instance.Arrow<=0)
                    {
                        MainGameManager.Instance.WeaponType = WeaponType.None;
                    }
                    ToNumberElement(true);
                    break;
                case WeaponType.Sword:
                    AudioManager.Instance.PlayClip(AudioManager.Instance.enemy);
                    ToNumberElement(true);
                    break;
            }

        }
        else
        {
            base.OnLeftMouseButton();
        }
    }
}
