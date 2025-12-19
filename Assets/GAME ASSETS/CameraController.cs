using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Tooltip("An array of transofrms representing camera positions.")]
    [SerializeField] Transform[] povs;
    [Tooltip("The speed at which camera follows the plane")]
    [SerializeField] float speed;

    private int index = 1;
    private Vector3 target;

    private void Update()
    {
        // Number 1-4 represents different povs (you can add more)
        if (Input.GetKeyDown(KeyCode.Alpha1)) index = 0; 
        else if (Input.GetKeyDown(KeyCode.Alpha2)) index = 1; 
        else if (Input.GetKeyDown(KeyCode.Alpha3)) index = 2; 
        else if (Input.GetKeyDown(KeyCode.Alpha4)) index = 3;

        // Set our target to the relevant POV
        target = povs[index].position;
    }

    private void FixedUpdate()
    {
        // Move camera to desired position/orientation. Must be in FixedUpdate to avoid camera jitters.
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
        transform.forward = povs[index].forward;
    }
}
