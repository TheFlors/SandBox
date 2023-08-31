using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Camera Variables
    [SerializeField] public CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] public Camera MainCamera;

    //Movement Variebles
    private CharacterController characterController;
    public float MoveSpeed = 1f; //PlayerMovementSpeed

    public float panBorderThickness = 10f; //Screen Border Thickness
    public Vector2 panLimit; //Screen Limit For X And Y 

    //Height Variables
    public float maxY = 400f;
    public float minY = 10f;

    //Zoom Variables
    private float targetFieldView = 50f;
    public float zoomSpeed = 10f;
    public float MaxFieldView = 50f;
    public float MinFieldView = 5f;

    //Rotate Variables
    public float RotateSpeed = 150f;

    private void Start()
    {
        //CharacterController
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 CameraAngles = MainCamera.transform.eulerAngles; //Take Camera Rotation Values

        

        Vector3 InputDir = new Vector3(); //Ýnput Direction
        Vector3 Pos = transform.position; //Create Position Variable

        //Get Player Key Reaction
        if (Input.GetKey("w") )
            InputDir.z = +1f;

        if (Input.GetKey("s") )
            InputDir.z = -1f;

        if (Input.GetKey("a") )
            InputDir.x = -1f;

        if (Input.GetKey("d") )
            InputDir.x = +1f;

        //New  Variant For Movement Towards Camera Direction 
        Vector3 MoveDir = transform.forward * InputDir.z // - new Vector3(0, transform.forward.y, 0))
            + transform.right * InputDir.x;

        

        //Zoom Codes With Scrool
        if (Input.mouseScrollDelta.y > 0) targetFieldView -= 5f;
        if (Input.mouseScrollDelta.y < 0) targetFieldView += 5f;

        targetFieldView = Mathf.Clamp(targetFieldView, MinFieldView , MaxFieldView);//Clamp To Zoom
        cinemachineVirtualCamera.m_Lens.FieldOfView = //Smooth Camera Move
            Mathf.Lerp(cinemachineVirtualCamera.m_Lens.FieldOfView, targetFieldView , Time.deltaTime * zoomSpeed);

        MoveSpeed = targetFieldView / 50f; //Slow Down When Player Zoomed

        //Shift to Fast
        if (Input.GetKey("left shift"))
            //Be Faster
            MoveSpeed = Mathf.Lerp(MoveSpeed, MoveSpeed = targetFieldView / 50f * 2, 0.2f);
        else
            //Go Back to OldMoveSpeed 
            MoveSpeed = Mathf.Lerp(MoveSpeed, MoveSpeed = targetFieldView / 50f, 0.2f);

        //Clamp for Map Border
        Pos.x = Mathf.Clamp(Pos.x, -panLimit.x, panLimit.x);
        Pos.z = Mathf.Clamp(Pos.z, -panLimit.y, panLimit.y);
        Pos.y = Mathf.Clamp(Pos.y, minY, maxY); //For Height

        //Change to New Position
        characterController.Move(MoveDir * MoveSpeed);

        //Change to EulerAngles
        transform.eulerAngles = CameraAngles;
    }
}
