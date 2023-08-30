using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Cinemachine Variables
    [SerializeField] public CinemachineVirtualCamera cinemachineVirtualCamera;

    //Movement Variebles
    public float panSpeed = 50f; //Screen Speed
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
    bool ControlRotation;

    void Update()
    {
        Vector3 InputDir = new Vector3(); //�nput Direction
        Vector3 Pos = transform.position; //Create Position Variable

        //Get Player Key Reaction
        if (Input.GetKey("w") /*|| Input.mousePosition.y >= Screen.height - panBorderThickness*/)
            InputDir.z = +1f;

        if (Input.GetKey("s") /*|| Input.mousePosition.y <= panBorderThickness*/)
            InputDir.z = -1f;

        if (Input.GetKey("a") /*|| Input.mousePosition.x <= panBorderThickness*/)
            InputDir.x = -1f;

        if (Input.GetKey("d") /*|| Input.mousePosition.x >= Screen.width - panBorderThickness*/)
            InputDir.x = +1f;

        //New  Variant For Movement Towards Camera Direction 
        Vector3 MoveDir = (transform.forward - new Vector3(0, transform.forward.y, 0)) * InputDir.z 
            + transform.right * InputDir.x;

        //Zoom Codes With Scrool
        if(Input.mouseScrollDelta.y > 0) targetFieldView -= 5f;
        if (Input.mouseScrollDelta.y < 0) targetFieldView += 5f;

        targetFieldView = Mathf.Clamp(targetFieldView, MinFieldView , MaxFieldView);//Clamp To Zoom
        cinemachineVirtualCamera.m_Lens.FieldOfView = //Smooth Camera Move
            Mathf.Lerp(cinemachineVirtualCamera.m_Lens.FieldOfView, targetFieldView , Time.deltaTime * zoomSpeed);

        panSpeed = targetFieldView; //Slow Down When Player Zoomed

        //Height Code With "E" And "Q" Keys
        if (Input.GetKey("e")) MoveDir.y += panSpeed * Time.deltaTime * 5;
        if (Input.GetKey("q")) MoveDir.y -= panSpeed * Time.deltaTime * 5;

        //Shift to Fast
        if(Input.GetKey("left shift")) 
            Pos += MoveDir * panSpeed * Time.deltaTime * 5;//Increase to Position With Fast Speed
        else
            Pos += MoveDir * panSpeed * Time.deltaTime;//Increase to Position With Speed


        //Clamp for Map Border
        Pos.x = Mathf.Clamp(Pos.x, -panLimit.x, panLimit.x);
        Pos.z = Mathf.Clamp(Pos.z, -panLimit.y, panLimit.y);
        Pos.y = Mathf.Clamp(Pos.y, minY, maxY); //For Height

        //Change to New Position
        transform.position = Pos;

        //Get RightMouse For Bool
        if (Input.GetMouseButtonDown(1)) ControlRotation = true;
        else if (Input.GetMouseButtonUp(1)) ControlRotation = false;

        //Rotate Camera With Mouse X Axis
        if (ControlRotation)
        {
            float MouseX = Input.GetAxis("Mouse X");
            transform.eulerAngles += new Vector3(0, MouseX * RotateSpeed * Time.deltaTime, 0);
        }
    }
}
