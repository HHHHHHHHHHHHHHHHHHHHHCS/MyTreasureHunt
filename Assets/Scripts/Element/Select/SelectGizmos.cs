using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectGizmos : MonoBehaviour
{
    public ToolType toolType;

    private void OnMouseUpAsButton()
    {

        int posX = (int)transform.position.x;
        int posY = (int)transform.position.y;
        switch (toolType)
        {
            case ToolType.Hoe:
                AudioManager.Instance.PlayClip(AudioManager.Instance.hoe);
                MainGameManager.Instance.Hoe--;
                MainUIManager.Instance.HoeToggle.isOn = false;
                MainGameManager.Instance.ForNearElement(posX, posY
                    , (x, y) =>
                    {
                        var temp = MainGameManager.Instance.MapArray[x, y];
                        if(temp.ElementContent == ElementContent.Exit)
                        {
                            
                        }
                        else if (temp.ElementType == ElementType.DoubleCovered)
                        {
                            ((DoubleCoverElement)temp).UncoveredElementSingle();
                        }
                        else if(temp.ElementContent == ElementContent.SmallWall)
                        {
                            temp.ToNumberElement(true);
                        }
                    });
                break;
            case ToolType.Tnt:
                AudioManager.Instance.PlayClip(AudioManager.Instance.tnt);
                MainGameManager.Instance.Tnt--;
                MainUIManager.Instance.TntToggle.isOn = false;
                MainGameManager.Instance.ForNearElement(posX, posY
                    , (x, y) =>
                    {
                        var temp = MainGameManager.Instance.MapArray[x, y];
                        if (temp.ElementContent == ElementContent.Exit)
                        {

                        }
                        else if(temp.ElementType == ElementType.DoubleCovered)
                        {
                            ((DoubleCoverElement)temp).UncoveredElementSingle();
                        }
                        else if (temp.ElementContent ==  ElementContent.BigWall)
                        {
                            ((BaseElement)temp).ToNumberElement(true);
                        }
                    });
                break;
            case ToolType.Map:
                AudioManager.Instance.PlayClip(AudioManager.Instance.map);
                MainGameManager.Instance.Map--;
                MainUIManager.Instance.MapToggle.isOn = false;
                MainGameManager.Instance.ForNearElement(posX, posY, 3
                , (x, y) =>
                {
                    var temp = MainGameManager.Instance.MapArray[x, y];
                    if (temp.ElementContent == ElementContent.Exit)
                    {

                    }
                    else if(temp.ElementContent == ElementContent.Trap)
                    {
                        temp.OnRightMouseButton();
                    }
                    else if (temp.ElementState == ElementState.Marked)
                    {
                        temp.OnRightMouseButton();
                    }
                });

                break;
            default:
                break;
        }
    }
}
