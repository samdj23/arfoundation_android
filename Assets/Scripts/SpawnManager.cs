using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;      //Important pour les trucs AR
using TMPro;

/// <summary>
/// Mecanisme d'interaction basique en AR
/// </summary>



public class SpawnManager : MonoBehaviour
{


    [SerializeField]
    ARRaycastManager m_RaycastManager;

    [SerializeField]
    GameObject spawnablePrefab;

    [SerializeField]
    Camera arCam;


    [SerializeField]
    GameObject t;

    TextMeshProUGUI text;

    List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();

    public int maxSpawnablePrefabs = 1;
    public int _actualSpawnedPrefabs = 0;


    void Start()
    {
        text = t.GetComponent<TextMeshProUGUI>();
        text.text = "hello";
    }

    void Update()
    {

        Ray ray = arCam.ScreenPointToRay(Input.GetTouch(0).position); // conversion from ScreenSpace to WorldSpace.

        if (m_RaycastManager.Raycast(Input.GetTouch(0).position, m_Hits))
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began && _actualSpawnedPrefabs == 0)
            {
                SpawnPrefab(m_Hits[0].pose.position);

            }
        }
    }


    private void SpawnPrefab(Vector3 spawnPosition)
    {
        if (_actualSpawnedPrefabs == 0)
        {
            Instantiate(spawnablePrefab, spawnPosition, Quaternion.identity);
            _actualSpawnedPrefabs = _actualSpawnedPrefabs + 1;
        }
    }



}
