using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{

    public float mouseSensitivity = 100f;

    public Transform player;

    private float rotationX = 0f;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; 
    }
    
    
    void Update()
    {
        
        var MouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        var MouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        rotationX -= MouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);
        
        player.Rotate(Vector3.up * MouseX);
        transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        
        
    }
    

}