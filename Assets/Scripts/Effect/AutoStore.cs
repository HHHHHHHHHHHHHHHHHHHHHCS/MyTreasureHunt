using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoStore : AutoDestory
{
    [SerializeField]
    private EffectType effectType;

    protected override void OnEnable()
    {
        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(delay);
        PoolManager.Instance.StoreInstance(effectType, gameObject);
    }
}
