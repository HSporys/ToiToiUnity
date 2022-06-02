using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnAreaHandler : MonoBehaviour
{
    [SerializeField] public GameObject areaPrefab;
    
    public List<SpawnArea> SpawnAreaList = new();
    private Vector3 _spawnAreaStart;
    private GameObject _spawnArea;
    
    private bool _initiated;
    private bool _spawning;

    private void Awake()
    {
        Transform[] transforms = gameObject.GetComponentsInChildren<Transform>();
        foreach (var child in transforms)
        {
            var component = child.gameObject.GetComponent(typeof(SpawnAreaBehaviour)) as SpawnAreaBehaviour;

            if (component != null)
            {
                var spawnArea =  gameObject.GetComponent(typeof(SpawnAreaHandler)) as SpawnAreaHandler;
                component.Init(spawnArea);
            }
        }
        
        _initiated = true;
    }

    private void Update()
    {
        SpawnArea();
    }

    public bool IsInitiated()
    {
        return _initiated;
    }


    private void SpawnArea()
    {
        Vector3 spawnAreaDistance;
        if (Input.GetKeyDown(KeyCode.D) && !_spawning)
        {
            _spawnAreaStart = CurrentPosition();
            _spawnArea = Instantiate(areaPrefab, _spawnAreaStart, Quaternion.identity);
            _spawnArea.transform.parent = gameObject.transform;
            spawnAreaDistance = CurrentPosition() - _spawnAreaStart;
            _spawnArea.transform.localScale = spawnAreaDistance;
            _spawnArea.transform.position += spawnAreaDistance * 0.5f;
            _spawning = true;
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            _spawning = false;
            var spawnComponent = _spawnArea.GetComponent(typeof(SpawnAreaBehaviour)) as SpawnAreaBehaviour;
            var spawnAreaHandlerComponent = gameObject.GetComponent(typeof(SpawnAreaHandler)) as SpawnAreaHandler;
            spawnComponent.Init(spawnAreaHandlerComponent);

        }
        
        if (Input.GetKey(KeyCode.D) && _spawning)
        {
            var shiftDirection = CurrentPosition() - _spawnAreaStart;
            _spawnArea.transform.position += shiftDirection;
            
            _spawnArea.transform.position = _spawnAreaStart;
            _spawnArea.transform.localScale = Vector3.one;
             spawnAreaDistance = CurrentPosition() - _spawnAreaStart;
            _spawnArea.transform.localScale = spawnAreaDistance;
            _spawnArea.transform.position += spawnAreaDistance * 0.5f;
        }
    }

    public void AddSpawnArea(SpawnArea spawnArea)
    {
        SpawnAreaList.Add(spawnArea);
        Debug.Log(SpawnAreaList.Count);
    }

    public void RemoveSpawnArea(SpawnArea spawnArea)
    {
        SpawnAreaList.Remove(spawnArea);
    }
    
    private Vector3 CurrentPosition()
    {
        var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position.z = -1;
        return position;
    }
}
