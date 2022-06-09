using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;      //Important pour les trucs AR

/// <summary>
/// Mecanisme d'interaction basique en AR
/// </summary>



public class SpawnManager : MonoBehaviour
{

    /// <summary>
    /// J'utilise un <c>ARRaycastManager</c> pour avoir une reference sur le composant et pouvoir tester un raycast sur un plane
    /// comme utilisé dans la fonction <c>Update</c>.
    /// Le <c>spawnablePrefab</c> est le prefab a instantié.
    /// La <c>arCam</c> est là pour donner la direction vers laquelle l'ecran est touché en World Space.
    /// Le <c>spawnedObject</c> est une reference sur le dernier objet posé.
    /// </summary>
    /// 
    /// <remarks>
    /// Screen Space est défini en pixels en coordonnées (x,y), Bottom_Left de l'ecran est le point (0,0).
    /// </remarks>

    [SerializeField]
    ARRaycastManager m_RaycastManager;

    [SerializeField]
    GameObject spawnablePrefab;

    [SerializeField]
    Camera arCam;

    GameObject spawnedObject;
    List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();

    public int maxSpawnablePrefabs = 1;
    public int _actualSpawnedPrefabs = 0;

    void Start()
    {
        spawnedObject = null; 
        //arCam = GameObject.Find("AR Camera").GetComponent<Camera>();
    }


    void Update()
    {
        if(Input.touchCount==0) // S'il n'y a pas de points touchés sur l'ecran.
        {
            return;
        }


        /// <summary>
        /// Si(on touche un plane ou un point d'un point cloud (see doc) garde la position dans la liste m_Hits)
        ///     Si(On vient de commencer a toucher l'ecran et la ref spawnedObject est nulle)
        ///         Si(un Raycast physique touche un objet)
        ///             Si(Objet touché porte le bon TAG) => spawnedObject reçoit l'objet.
        ///             Sinon => Un spawnablePrefab est instantié a l'endroit où l'ecran est touché.
        ///     SinonSi(Y'a un mouvement a l'écran et spawnedObject existe) => on bouge spawnedObject.
        ///     SinonSi(Pas de toucher sur l'ecran) => La reference devient NULL.
        /// </summary>
        /// 
        /// <remarks>
        /// <c>hit</c> et <c>ray</c> sont pour le Raycast physique.
        /// </remarks>

        RaycastHit hit;
        Ray ray = arCam.ScreenPointToRay(Input.GetTouch(0).position); // conversion from ScreenSpace to WorldSpace.

        if (m_RaycastManager.Raycast(Input.GetTouch(0).position,m_Hits)) 
        {
            if(Input.GetTouch(0).phase==TouchPhase.Began && spawnedObject==null)
            {
                if(Physics.Raycast(ray, out hit))
                {
                    if(hit.collider.gameObject.tag=="Spawnable")
                    {
                        spawnedObject = hit.collider.gameObject;
                    }
                    else
                    {
                        SpawnPrefab(m_Hits[0].pose.position);
                    }
                }
            }
            else if(Input.GetTouch(0).phase == TouchPhase.Moved && spawnedObject != null)
            {
                spawnedObject.transform.position = m_Hits[0].pose.position;
            }
            if(Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                spawnedObject = null;
            }

        }
    }

    /// <summary>
    /// Fonction qui instantie un prefab au dérnier endroit où l'écran est touché
    /// </summary>

    private void SpawnPrefab(Vector3 spawnPosition)
    {
        spawnedObject = Instantiate(spawnablePrefab, spawnPosition, Quaternion.identity);
    }
}
