using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestory : MonoBehaviour
{
    [SerializeField]
    protected float delay = 0.75f;


    protected virtual void OnEnable()
    {
        Destroy(gameObject, delay);
    }
}
