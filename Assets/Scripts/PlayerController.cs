using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject planet;
    public KeyCode Up, Down, Left, Right, Jump, Sprint;
    [SerializeField]
    private GameObject playerModel;
    [SerializeField]
    private Rigidbody rBody;
    [SerializeField]
    private Collider col;
    [SerializeField]
    private float camRotateSpeed, currentSprintSpeed;
    public bool inAir = false;
    private bool moving = false;
    public float moveSpeed, jumpScale, sprintSpeed, gravForceModifier;
    [SerializeField]
    private AnimationCurve gravCurve;
    public float timeSinceLastCollision, inAirMoveSpeedMod;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        currentSprintSpeed = sprintSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (inAir)
        {
            inAirMoveSpeedMod = 0.5f;
            timeSinceLastCollision += Time.deltaTime;
        }
        else
        {
            inAirMoveSpeedMod = 1f;
        }
        gravForceFixer();
        currentSprintSpeed = 1f;
        if (Input.GetKey(Sprint))
        {
            currentSprintSpeed = sprintSpeed;
        }
        if (!moving)
        {
            col.material.dynamicFriction = 2f;
            col.material.staticFriction = 2f;
        }
        if (moving)
        {
            col.material.dynamicFriction = 0f;
            col.material.staticFriction = 0f;
        }
        moving = false;
        if (Input.GetKey(Up))
        {
            rBody.AddForce(-playerModel.transform.forward.normalized * moveSpeed * currentSprintSpeed * inAirMoveSpeedMod, ForceMode.Force);
            moving = true;
        }
        if (Input.GetKey(Down))
        {
            rBody.AddForce(playerModel.transform.forward.normalized * moveSpeed * currentSprintSpeed * inAirMoveSpeedMod, ForceMode.Force);
            moving = true;
        }
        if (Input.GetKey(Left))
        {
            rBody.AddForce(playerModel.transform.right.normalized * moveSpeed * currentSprintSpeed * inAirMoveSpeedMod, ForceMode.Force);
            moving = true;
        }
        if (Input.GetKey(Right))
        {
            rBody.AddForce(-playerModel.transform.right.normalized * moveSpeed * currentSprintSpeed * inAirMoveSpeedMod, ForceMode.Force);
            moving = true;
        }
        if (Input.GetKeyDown(Jump) && !inAir)
        {
            rBody.AddForce(playerModel.transform.up.normalized * jumpScale, ForceMode.Impulse);
            moving = true;
            inAir = true;
        }

        
    }
    private void gravForceFixer()
    {
        
        gravForceModifier = gravCurve.Evaluate(timeSinceLastCollision);
        
    }
    private void alignToGrav()
    {
        Vector3 v3 = new Vector3(planet.transform.position.x, planet.transform.position.y - 1000, planet.transform.position.z);
        playerModel.transform.LookAt(v3);
        playerModel.transform.Rotate(-90, 0, 0);
    }
    private Vector3 GravRotation()
    {
        var gravityOrigin = new Vector3(planet.transform.position.x, planet.transform.position.y - 250, planet.transform.position.z);
        var playerToGravityOriginVector = gravityOrigin - playerModel.transform.position;
        return playerToGravityOriginVector.normalized;
    }

    Vector3 storedRotation = Vector3.zero;

    private void LateUpdate()
    {
        float horizontal = Input.GetAxis("Mouse X") * camRotateSpeed;
        float vertical = Input.GetAxis("Mouse Y") * camRotateSpeed;

        storedRotation.y += horizontal;
        storedRotation.x -= vertical;
        if (storedRotation.x > 50f)
        {
            storedRotation.x = 50f;
        }else if (storedRotation.x < -20f)
        {
            storedRotation.x = -20f;
        }
        transform.rotation = Quaternion.Euler(storedRotation);

        transform.position = playerModel.transform.position;
        transform.LookAt(playerModel.transform);
        var grav = GravRotation();
        playerModel.transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(-transform.forward, grav).normalized, -grav);
    }
    
}
