using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public class ToiletBehaviour : MonoBehaviour
{
    public Toilet Toilet;
    private ToiletHandler _toiletHandler;

    private bool _attachedToMouse = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp((int)MouseButton.Left))
        {
            _attachedToMouse = false;
        }
        if (_attachedToMouse)
        {
            var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            position.z = -1;
            transform.position = position;
            Toilet.coordinates.X = position.x;
            Toilet.coordinates.Y = position.y;
        }
    }

    public void Init(ToiletHandler toiletHandler)
    {
        _toiletHandler = toiletHandler;
        var position = gameObject.transform.position;
        Toilet = new Toilet(position.x,position.y);
        var meterHandler = GetComponentInChildren(typeof(MeterHandler)) as MeterHandler;
        meterHandler.Init(Toilet);
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown((int)MouseButton.Left))
        {
            _attachedToMouse = true;
        }

        if (Input.GetMouseButtonDown((int)MouseButton.Right))
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        _toiletHandler.RemoveToilet(Toilet);
    }
}
