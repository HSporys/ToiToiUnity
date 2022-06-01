using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public class ToiletBehaviour : MonoBehaviour
{
    public Toilet Toilet;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Init()
    {
        var position = gameObject.transform.position;
        Toilet = new Toilet(position.x,position.y);
        var meterHandler = GetComponentInChildren(typeof(MeterHandler)) as MeterHandler;
        meterHandler.Init(Toilet);
    }
}
