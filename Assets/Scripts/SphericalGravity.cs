using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SphericalGravity : MonoBehaviour
{
    //[SerializeField]
    //private GameObject player;
    [SerializeField]
    private List<GameObject> objects;
    [SerializeField]
    private GameObject planet;
    [SerializeField]
    private float gravitationalPull;

    void FixedUpdate()
    {
        //apply spherical gravity to selected objects (set the objects in editor)
        foreach (GameObject o in objects)
        {
            if (o.GetComponent<Rigidbody>())
            {
                o.GetComponent<Rigidbody>().AddForce((planet.transform.position - o.transform.position).normalized * gravitationalPull);
            }
        }
        //player.GetComponent<Rigidbody>().AddForce((planet.transform.position - player.transform.position).normalized * gravitationalPull * pController.gravForceModifier);
        /*or apply gravity to all game objects with rigidbody
        foreach (GameObject o in UnityEngine.Object.FindObjectsOfType<GameObject>())
        {
            if (o.GetComponent<Rigidbody>() && o != planet)
            {
                o.GetComponent<Rigidbody>().AddForce((planet.transform.position - o.transform.position).normalized * gravitationalPull);
            }
        }
        */
    }

}