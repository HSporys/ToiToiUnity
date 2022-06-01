using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class InputManager : MonoBehaviour
{
    [FormerlySerializedAs("ToiletPrefab")] [SerializeField] public GameObject toiletPrefab;

    private ToiletHandler _toiletHandler;
    // Start is called before the first frame update
    void Start()
    {
        _toiletHandler = gameObject.GetComponent(typeof(ToiletHandler)) as ToiletHandler;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            position.z = -1;
            var toiletGameObject = Instantiate(toiletPrefab,position,Quaternion.identity);
            toiletGameObject.transform.parent = gameObject.transform;
            var toiletBehaviour = toiletGameObject.GetComponent(typeof(ToiletBehaviour)) as ToiletBehaviour;
            _toiletHandler.AddToilet(toiletBehaviour);
            
        }
        RefreshScene();

    }

    void RefreshScene()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }
}
