using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;
using Vector3 = UnityEngine.Vector3;

public class SpawnAreaBehaviour : MonoBehaviour
{
    private SpawnAreaHandler _spawnAreaHandler;
    public SpawnArea SpawnArea;
    // Start is called before the first frame update

    public void Init(SpawnAreaHandler spawnAreaHandler)
    {
        SpawnArea = new SpawnArea();
        var renderer = gameObject.GetComponent(typeof(Renderer)) as Renderer;
        if (renderer != null)
        {
            var bounds = renderer.bounds;
            Vector3 min = bounds.min;
            Vector3 max = bounds.max;

            SpawnArea.StartPoint = new Vector2(min.x, min.y);
            SpawnArea.EndPoint = new Vector2(max.x, max.y);
        }

        _spawnAreaHandler = spawnAreaHandler;
        spawnAreaHandler.AddSpawnArea(SpawnArea);

    }
    
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown((int)MouseButton.Right))
        {
            Destroy(gameObject);
        }
    }
    
    private void OnDestroy()
    {
        _spawnAreaHandler.RemoveSpawnArea(SpawnArea);
    }
}
