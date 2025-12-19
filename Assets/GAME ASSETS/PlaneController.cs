using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlaneController : MonoBehaviour
{
    [Header("Plane Stats")]
    [Tooltip("How much throttle ramps up or down.")]
    public float throttleIncrement = 0.1f;
    [Tooltip("Maximum engine thrust when at 100% throttle")]
    public float maxThrust = 200f;
    [Tooltip("How responsive the plane is when rolling, pitching, and yawing.")]
    public float responsiveness = 10f;
    [Tooltip("How much lift force this plane generates as it gains speed.")]
    public float lift = 135f;

    public VariableJoystick variableJoystick;
    public VariableJoystick variableJoystick2;

    private float throttle;                 // Percentage of maximum engine thrust currently being used.
    private float roll;                     // Tilting left to right
    private float pitch;                    // Tilting front to back
    private float yaw;                      // "Turning" left to right

    private float responceModifier          // Value used to tweak responsiveness to suit the plane's mass
    {
        get
        {
            return (rb.mass / 10f) * responsiveness;
        }
    }

    Rigidbody rb;
    [SerializeField] TextMeshProUGUI hud;

    //Throttle controls for UI buttons
    [Header("UI inputs variables")]
    private bool throttleUp = false;
    private bool throttleDown = false;
    public float throttleButtonSpeed = 1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void HandleInputs()
    {
/*        // Set the rotational values from our axis inputs.
        roll = Input.GetAxis("Horizontal");
        pitch = Input.GetAxis("Vertical");
        yaw = Input.GetAxis("Yaw");
*/
        roll = -variableJoystick.Horizontal;
        pitch = variableJoystick.Vertical;
        yaw = variableJoystick2.Horizontal;

        // Handle throttle value being sure to clamp it between 0 and 100.
        if (Input.GetKey(KeyCode.Space)) throttle += throttleIncrement;
        if (Input.GetKey(KeyCode.LeftControl)) throttle -= throttleIncrement;
        throttle = Mathf.Clamp(throttle, 0f, 100f);
        HandleThrottleUIInput();
    }

    private void Update()
    {
        HandleInputs();
        UpdateHood();
    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.forward * maxThrust * throttle);
        rb.AddTorque(transform.up * yaw * responceModifier);
        rb.AddTorque(transform.right * pitch * responceModifier);
        rb.AddTorque(transform.forward * roll * responceModifier);

        rb.AddForce(transform.up * rb.velocity.magnitude * lift);
    }

    private void UpdateHood()
    {
        hud.text = "Throttle " + throttle.ToString("F0") + "%\n";
        hud.text += "Airspeed " + (rb.velocity.magnitude * 3.6f).ToString("F0") + "km/h\n";
        hud.text += "Altitude " + (transform.position.y * 3.281f).ToString("F0") + " ft";
    }

    public void ThrottleUp()
    {
        throttleUp = true;
    }

    public void ThrottleDown()
    {
        throttleDown = true;
    }

    public void ThrottleIdle()
    {
        throttleUp = false;
        throttleDown = false;
    }

    private void HandleThrottleUIInput()
    {
        if (throttleUp) throttle += throttleIncrement * throttleButtonSpeed;
        if (throttleDown) throttle -= throttleIncrement * throttleButtonSpeed;
        throttle = Mathf.Clamp(throttle, 0f, 100f);
    }
}
