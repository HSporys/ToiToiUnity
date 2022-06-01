using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class ToiletHandler : MonoBehaviour
{
    private List<Toilet> _toiletList = new();
    
    // Start is called before the first frame update
    void Awake()
    {
        Transform[] transforms = gameObject.GetComponentsInChildren<Transform>();
        foreach (var child in transforms)
        {
            var component = child.gameObject.GetComponent(typeof(ToiletBehaviour)) as ToiletBehaviour;

            if (component != null)
            {
                component.Init();
                Debug.Log(component);
                if (component != null) _toiletList.Add(component.Toilet);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        SimulationHandler.SimultationStep(_toiletList, Time.deltaTime);
        foreach (var toilet in _toiletList)
        {
            Debug.Log(toilet.FreshWater);
            Debug.Log(toilet.WasteWater1);
            Debug.Log(toilet.WasteWater2);

        }
    }
}
