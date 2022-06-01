using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreshWaterMeter : MonoBehaviour
{
    private Toilet _toilet;

    // Start is called before the first frame update
    void Start()
    {
        var parent = GetComponentInParent(typeof(ToiletBehaviour)) as ToiletBehaviour;
        Debug.Log(parent);
        if (parent != null) _toilet = parent.Toilet;
    }

    // Update is called once per frame
    void Update()   
    {
        var o = gameObject;
        o.transform.localScale = new Vector3(o.transform.localScale.x, _toilet.FreshWater/ _toilet.FreshWaterMax *2,1);
    }
}