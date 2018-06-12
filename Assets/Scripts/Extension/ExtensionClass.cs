using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionClass
{
    public static Vector3[] ToVector3Array(this List<AStarPoint> pointList)
    {
        Vector3[] vec3Array = new Vector3[pointList.Count];
        for (int i = 0; i < vec3Array.Length; i++)
        {
            vec3Array[i] =new Vector3( pointList[i].x, pointList[i].y,0);
        }
        return vec3Array;
    }


}
