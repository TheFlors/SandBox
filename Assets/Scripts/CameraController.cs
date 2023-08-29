using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Movement Variebles
    public float panSpeed = 20f; //Screen Speed
    public float panBorderThickness = 10f; //Screen Border Thickness
    public Vector2 panLimit; //Screen Limit For X And Y 

    //Zoom Variables
    public float scroolSpeed = 50f;
    public float maxY;
    public float minY;

    //Rotate Variables
    public float RotateSpeed = 200f;
    bool ControlRotation;

    void Update()
    {
        Vector3 InputDir = new Vector3(); //Ýnput Direction
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
        float scrool = Input.GetAxis("Mouse ScrollWheel");
        MoveDir.y -= scroolSpeed * scrool;

        //Zoom Code With "E" And "Q" Keys
        if (Input.GetKey("e")) MoveDir.y += scroolSpeed * Time.deltaTime * 5;
        if (Input.GetKey("q")) MoveDir.y -= scroolSpeed * Time.deltaTime * 5;

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
