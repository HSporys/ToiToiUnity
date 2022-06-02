using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.PlayerLoop;

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

    private void Update()
    {
        if (_toiletHandler.IsInitiated() && _spawnAreaHandler.IsInitiated())
        {
            var queue = GameObject.Find("Queue");
            var queueText = queue.transform.GetComponentInChildren(typeof(TMPro.TextMeshProUGUI)) as TMPro.TextMeshProUGUI;
            queueText.text = String.Format("Queue: {0:F0}", SimulationHandler.QueueCount);

            
            var serviceStatus = GameObject.Find("ServiceStatus");
            var serviceStatusText = serviceStatus.transform.GetComponentInChildren(typeof(TMPro.TextMeshProUGUI)) as TMPro.TextMeshProUGUI;
            if (SimulationHandler.MaintinenceTimer >= 0)
            {
                serviceStatusText.text = String.Format("Maintenance in {0:F0}", SimulationHandler.MaintinenceTimer);
            }
            else
            {
                serviceStatusText.text = "";
            }
            
            var aiStatus = GameObject.Find("AiStatus");
            var aiStatusText = aiStatus.transform.GetComponentInChildren(typeof(TMPro.TextMeshProUGUI)) as TMPro.TextMeshProUGUI;

            aiStatusText.text = String.Format("Maintenance mode: {0:F0}", (int)SimulationHandler.maintenanceMode);

            if (Input.GetKeyDown(KeyCode.S))
            {
                SimulationHandler.ToggleMaintenanceMode();
            }

        }
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
