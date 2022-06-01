using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeterHandler : MonoBehaviour
{
    private Transform _freshWaterMeter;
    private Transform _wasteMeter1;
    private Transform _wasteMeter2;
    private SpriteRenderer _occupiedLamp;

    private Toilet _toilet;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var meterTransform = _freshWaterMeter;
        _freshWaterMeter.localScale = new Vector3(meterTransform.localScale.x, _toilet.FreshWater/ _toilet.FreshWaterMax *2,1);
        meterTransform = _wasteMeter1;
        _wasteMeter1.localScale = new Vector3(meterTransform.localScale.x, _toilet.WasteWater1/ _toilet.WasteWater1Max *2,1);
        meterTransform = _wasteMeter2;
        _wasteMeter2.localScale = new Vector3(meterTransform.localScale.x, _toilet.WasteWater2/ _toilet.WasteWater2Max *2,1);

        if (_toilet.IsAvailable())
        {
            _occupiedLamp.color = Color.green;
        }
        else
        {
            _occupiedLamp.color = Color.red;
        }
    }

    public void Init(Toilet toilet)
    {
        _toilet = toilet;

        _freshWaterMeter = gameObject.transform.Find("FreshWaterMeter");
        _wasteMeter1 = gameObject.transform.Find("WasteMeter1");
        _wasteMeter2 = gameObject.transform.Find("WasteMeter2");
        _occupiedLamp = gameObject.transform.Find("OccupiedLamp").gameObject.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;

        Debug.Log(_freshWaterMeter);
        Debug.Log("start");
    }
}
