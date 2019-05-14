using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public float moveSpeed, jumpScale, sprintSpeed, gravForceModifier, sprintTimeMax;
    private float sprintMeter;
    [SerializeField]
    private AnimationCurve gravCurve, sprintDrainCurve, sprintRechargecurve;
    public float timeSinceLastCollision, inAirMoveSpeedMod;
    [SerializeField]
    private float playerGravitationalPull;
    [SerializeField]
    private Slider staminaSlider;
    public float allowedAngle;
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
            inAirMoveSpeedMod = 1f;
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
            Mathf.Clamp01(sprintMeter -= Time.deltaTime);
            if(sprintMeter >= 0) { 
                currentSprintSpeed = sprintSpeed;
                allowedAngle = 0.8f;
            }
        }
        if (!Input.GetKey(Sprint)) { 
            Mathf.Clamp01(sprintMeter += Time.deltaTime);
            allowedAngle = 0.2f;
        }
        if (!moving)
        {
            col.material.dynamicFriction = 0f;
            col.material.staticFriction = 0f;
        }
        if (moving)
        {
            col.material.dynamicFriction = 0f;
            col.material.staticFriction = 0f;
        }
        
        if (Input.GetKeyDown(Jump) && !inAir)
        {
            rBody.AddForce(playerModel.transform.up.normalized * jumpScale, ForceMode.Impulse);
            moving = true;
            //inAir = true;
        }
        staminaSlider.value = sprintMeter;


    }

    private void FixedUpdate()
    {
        inputMovement();
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
    private void LateUpdate()
    {
        lateUpdateRotation();
    }
    Vector3 storedRotation = Vector3.zero;
    void lateUpdateRotation()
    {
        float horizontal = Input.GetAxis("Mouse X") * camRotateSpeed;
        float vertical = Input.GetAxis("Mouse Y") * camRotateSpeed;

        storedRotation.y += horizontal;
        storedRotation.x -= vertical;
        if (storedRotation.x > 50f)
        {
            storedRotation.x = 50f;
        }
        else if (storedRotation.x < -20f)
        {
            storedRotation.x = -20f;
        }
        transform.rotation = Quaternion.Euler(storedRotation);

        transform.position = playerModel.transform.position;
        transform.LookAt(playerModel.transform);
        var grav = GravRotation();
        playerModel.transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(-transform.forward, grav).normalized, -grav);
    }
    void inputMovement()
    {
        rBody.AddForce((planet.transform.position - playerModel.transform.position).normalized * playerGravitationalPull * gravForceModifier);
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
    }
}
