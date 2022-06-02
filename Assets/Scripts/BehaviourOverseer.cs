using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class BehaviourOverseer : MonoBehaviour
{
    private ToiletHandler _toiletHandler;
    private SpawnAreaHandler _spawnAreaHandler;

    // Start is called before the first frame update
    void Start()
    {
        _toiletHandler = transform.GetComponentInChildren(typeof(ToiletHandler)) as ToiletHandler;
        _spawnAreaHandler = transform.GetComponentInChildren(typeof(SpawnAreaHandler)) as SpawnAreaHandler;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_toiletHandler.IsInitiated() && _spawnAreaHandler.IsInitiated())
        {
            SimulationHandler.SimultationStep(_toiletHandler.ToiletList,_spawnAreaHandler.SpawnAreaList, Time.deltaTime);
        }
    }
}
