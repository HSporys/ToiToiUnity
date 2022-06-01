using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public class ToiletBehaviour : MonoBehaviour
{
    public Toilet Toilet;
    private List<Toilet> _toiletList = new();
    // Start is called before the first frame update
    void Awake()
    {
        var position = gameObject.transform.position;
        Toilet = new Toilet(position.x,position.y);
        _toiletList.Add(Toilet);
    }

    // Update is called once per frame
    void Update()
    {
        SimulationHandler.SimultationStep(_toiletList, Time.deltaTime);
        Toilet.Tick(Time.deltaTime);
    }
}
