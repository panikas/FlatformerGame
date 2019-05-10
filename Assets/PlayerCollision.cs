using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public PlayerController pController;
    void OnCollisionEnter(Collision other)
    {
        pController.inAir = false;
        pController.timeSinceLastCollision = 0f;
    }
}
