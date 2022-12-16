using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public Camera camera;

    public float scrollSensitivty;

    public Vector2 mouseClickPos;
    public Vector2 mouseCurrentPos;
    public bool panning = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Scroll();
        MouseDrag();
        if (Input.GetKeyDown(KeyCode.Space)){
            camera.transform.position = new Vector3(0,0,camera.transform.position.z);
            camera.orthographicSize = 80;
        }
    }

    public void MouseDrag()
    {
        // When LMB clicked get mouse click position and set panning to true
        if (Input.GetKeyDown(KeyCode.Mouse1) && !panning)
        {
            mouseClickPos = camera.ScreenToWorldPoint(Input.mousePosition);
            panning = true;
        }
        // If LMB is already clicked, move the camera following the mouse position update
        if (panning)
        {
            mouseCurrentPos = camera.ScreenToWorldPoint(Input.mousePosition);
            var distance = mouseCurrentPos - mouseClickPos;
            camera.transform.position += new Vector3(-distance.x, -distance.y, 0);
        }

        // If LMB is released, stop moving the camera
        if (Input.GetKeyUp(KeyCode.Mouse1))
            panning = false;
    }


    public void Scroll()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0 && camera.orthographicSize > 10) //Scroll in
        {
            camera.orthographicSize -= scrollSensitivty;
        }else if(Input.GetAxis("Mouse ScrollWheel") < 0 && camera.orthographicSize < 100) // Scroll out
        {
            camera.orthographicSize += scrollSensitivty;
        }
    }
}
