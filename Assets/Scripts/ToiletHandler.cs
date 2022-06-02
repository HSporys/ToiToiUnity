using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class ToiletHandler : MonoBehaviour
{
    public List<Toilet> ToiletList = new();
    private Vector3 _selectionStart;
    private Vector3 _selectionEnd;
    private bool _selecting = false;
    private bool _selected = false;
    private GameObject _selectionArea;
    private bool _shiftingSelection = false;
    private Vector3 _shiftStart = Vector3.zero;
    private List<GameObject> _shiftingToilets = new();
    private List<SpawnArea> _spawnAreas = new();

    private bool _initiated;

    [SerializeField] public GameObject selectionPrefab;

    // Start is called before the first frame update
    void Awake()
    {
        Transform[] transforms = gameObject.GetComponentsInChildren<Transform>();
        foreach (var child in transforms)
        {
            var component = child.gameObject.GetComponent(typeof(ToiletBehaviour)) as ToiletBehaviour;

            if (component != null)
            {
                var toiletHandler =  gameObject.GetComponent(typeof(ToiletHandler)) as ToiletHandler;
                component.Init(toiletHandler);
                if (component != null) ToiletList.Add(component.Toilet);
            }
        }

        _initiated = true;
    }

    private void Update()
    {
        SelectToilets();
    }

    public bool IsInitiated()
    {
        return _initiated;
    }

    public void AddToilet(ToiletBehaviour toilet)
    {
        var toiletHandler =  gameObject.GetComponent(typeof(ToiletHandler)) as ToiletHandler;
        toilet.Init(toiletHandler);
        ToiletList.Add(toilet.Toilet);
    }

    void SelectToilets()
    {
        //select area
        Vector3 selectionDistance;
        if (Input.GetKeyDown(KeyCode.Mouse0) && !_selected)
        {
            _selectionStart = CurrentPosition();
            _selectionArea = Instantiate(selectionPrefab, _selectionStart, Quaternion.identity);
            selectionDistance = CurrentPosition() - _selectionStart;
            _selectionArea.transform.localScale = selectionDistance;
            _selectionArea.transform.position += selectionDistance * 0.5f;
            _selecting = true;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && _selected)
        {
            if (IsInsideArea(_selectionStart, _selectionEnd, CurrentPosition()))
            {
                _shiftingSelection = true;
                _shiftStart = CurrentPosition();

                foreach (Transform child in transform)
                {
                    if (IsInsideArea(_selectionStart, _selectionEnd, child.transform.position))
                    {
                        _shiftingToilets.Add(child.gameObject);
                    }
                }
            }
        }

        if (Input.GetKey(KeyCode.Mouse0) && _shiftingSelection)
        {
            var shiftDirection = CurrentPosition() - _shiftStart;
            _selectionArea.transform.position += shiftDirection;
            foreach (GameObject toilet in _shiftingToilets)
            {
                toilet.transform.position += shiftDirection;
            }

            _shiftStart += shiftDirection;
        }

        if (Input.GetKey(KeyCode.Mouse0) && _selecting)
        {
            _selectionArea.transform.position = _selectionStart;
            _selectionArea.transform.localScale = Vector3.one;
            selectionDistance = CurrentPosition() - _selectionStart;
            _selectionArea.transform.localScale = selectionDistance;
            _selectionArea.transform.position += selectionDistance * 0.5f;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            _shiftingSelection = false;
            _selectionEnd = CurrentPosition();
            _selecting = false;
            selectionDistance = _selectionEnd - _selectionStart;
            if (Vector3.Magnitude(selectionDistance) > 1.0f && _selected == false)
            {
                _selected = true;
            }
            else
            {
                _selected = false;
                Destroy(_selectionArea);
                _shiftingToilets = new();
            }
        }
    }

    private bool IsInsideArea(Vector3 start, Vector3 end, Vector3 position)
    {
        Vector3 min = Vector3.Min(start, end);
        Vector3 max = Vector3.Max(start, end);

        if (position.x > max.x || position.y > max.y)
        {
            return false;
        }

        if (position.x < min.x || position.y < min.y)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private Vector3 CurrentPosition()
    {
        var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position.z = -1;
        return position;
    }

    public void RemoveToilet(Toilet toilet)
    {
        ToiletList.Remove(toilet);
    }
}