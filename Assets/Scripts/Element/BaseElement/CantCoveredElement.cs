﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CantCoveredElement : BaseElement
{
    public override void OnInit()
    {
        base.OnInit();
        ElementType = ElementType.CantCovered;
        ElementState = ElementState.UnCovered;
    }
}
