using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectGizmos : MonoBehaviour
{
    public ToolType toolType;

    private void OnMouseUp()
    {
        int posX = (int)transform.position.x;
        int posY = (int)transform.position.y;
        switch (toolType)
        {
            case ToolType.Hoe:
                MainGameManager.Instance.Hoe--;
                UIManager.Instance.HoeToggle.isOn = false;
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
                MainGameManager.Instance.Tnt--;
                UIManager.Instance.TntToggle.isOn = false;
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
                        else
                        {
                            ((SingleCoverElement)temp).ToNumberElement(true);
                        }
                    });
                break;
            case ToolType.Map:
                MainGameManager.Instance.Map--;
                UIManager.Instance.MapToggle.isOn = false;
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
