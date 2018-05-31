using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestory : MonoBehaviour
{
    [SerializeField]
    private float delay = 0.75f;

    private void Start()
    {
        Destroy(gameObject, delay);
    }
}
