using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateByGravity : MonoBehaviour
{
    [SerializeField]
    private GameObject planet;
    // Start is called before the first frame update
    void Start()
    {
        alignToGrav();
    }
    void Update()
    {
        

    }
    private void alignToGrav()
    {
        Vector3 v3 = new Vector3(planet.transform.position.x, planet.transform.position.y-250, planet.transform.position.z);
        transform.LookAt(v3);
        transform.Rotate(-90, 0, 0);
    }
}
