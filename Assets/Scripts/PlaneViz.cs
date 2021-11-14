using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARPlaneManager))]
public class PlaneViz : MonoBehaviour
{
    public ARPlaneManager planeManager;

    [SerializeField]
    SpawnManager manager;
    private int i = 0;
    
    private void awake()
    {
        planeManager = GetComponent<ARPlaneManager>();
        manager = GetComponent<SpawnManager>();
    }

    
    void Update()
    {
        
        if(manager._actualSpawnedPrefabs>0 && i==0)
        {
            i++;
            SetAllPlanesState(false);
        }
        else if(i>0)
        {
            return;
        }
        
    }



    private void SetAllPlanesState(bool state)
    {

        planeManager.enabled = !planeManager.enabled;
        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(state);
            
        }
    }
}
