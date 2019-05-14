using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public PlayerController pController;
    [SerializeField]
    private void Update()
    {
        pController.inAir = true;
    }
    
    void OnCollisionStay(Collision collisionInfo)
    {
        foreach (ContactPoint contact in collisionInfo.contacts)
        {
            //Debug.DrawRay(contact.point, contact.normal * 10, Color.white);
            //print(contact.normal);

            if(contact.normal.y > pController.allowedAngle)
            {
                print("COLLIDING");
                pController.inAir = false;
                pController.timeSinceLastCollision = 0f;
            }
        }
        
    }
}
