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
    private float GivenMoveSpeed;//Given Player Value
    public float MoveSpeed = 0.5f; //PlayerMovementSpeed

    private float GivenLookSpeed;//Given Player Value
    public float LookSpeed = 2f; //PlayerLookSpeed

    public Vector2 panLimit; //Screen Limit For X And Y 

    //Height Variables
    public float maxY = 400f;
    public float minY = 10f;

    //Zoom Variables
    private float targetFieldView = 50f;
    public float zoomSpeed = 10f;
    private float MaxFieldView = 50f;
    private float MinFieldView = 5f;


    private void Start()
    {
        //Move Speed Value
        GivenMoveSpeed = MoveSpeed;

        //Look Speed Vaue
        GivenLookSpeed = LookSpeed;

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

        MoveSpeed = targetFieldView / 50f * GivenMoveSpeed; //Slow Down When Player Zoomed
        LookSpeed = targetFieldView / 50f * GivenLookSpeed; //Slow Down When Player Zoomed

        //Press Shift to Slow
        if (Input.GetKey("left shift"))
            //Be Faster
            MoveSpeed = Mathf.Lerp(MoveSpeed, MoveSpeed = targetFieldView / 50f * 2 * GivenMoveSpeed, 0.2f);
        else
            //Go Back to Normal 
            MoveSpeed = Mathf.Lerp(MoveSpeed, MoveSpeed = targetFieldView / 50f * GivenMoveSpeed, 0.2f);

        //Press CTRL for Slow
        if (Input.GetKey("left ctrl"))
            //Be Faster
            MoveSpeed = Mathf.Lerp(MoveSpeed, MoveSpeed = targetFieldView / 50f / 2 * GivenMoveSpeed, 0.2f);
        else
            //Go Back to Normal
            MoveSpeed = Mathf.Lerp(MoveSpeed, MoveSpeed = targetFieldView / 50f * GivenMoveSpeed, 0.2f);

        //Clamp for Map Border
        Pos.x = Mathf.Clamp(Pos.x, -panLimit.x, panLimit.x);
        Pos.z = Mathf.Clamp(Pos.z, -panLimit.y, panLimit.y);
        Pos.y = Mathf.Clamp(Pos.y, minY, maxY); //For Height

        //Change to New Position
        characterController.Move(MoveDir * MoveSpeed);

        //MouseLook Codes
        float MouseX = Input.GetAxis("Mouse X");
        float MouseY = Input.GetAxis("Mouse Y");
        transform.eulerAngles += new Vector3(-MouseY, MouseX, 0) * LookSpeed;

    }
}
